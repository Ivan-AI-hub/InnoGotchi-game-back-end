using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a pet 
    /// </summary>
    public class PetManager
    {
        private AbstractValidator<Pet> _validator;
        private IRepository<Pet> _petRepository;
        private IRepository<PetFarm> _farmRepository;
        private IMapper _mapper;

        public PetManager(IRepository<Pet> petRepository, IRepository<PetFarm> farmRepository, IMapper mapper, AbstractValidator<Pet> validator)
        {
            _validator = validator;
            _petRepository = petRepository;
            _farmRepository = farmRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a pet for the <paramref name="farmId"/> farm
        /// </summary>
        /// <param name="farmId">id of the farm containing the pet</param>
        /// <param name="name">Pet name</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult Add(int farmId, string name, PetViewDTO? view)
        {
            var dataPet = new Pet()
            {
                FarmId = farmId,
                View = _mapper.Map<PetView>(view)
            };

            dataPet.Statistic = new PetStatistic()
            {
                Name = name,
                BornDate = DateTime.UtcNow,
                IsAlive = true,
                FirstHappinessDay = DateTime.UtcNow,
                DateLastFeed = DateTime.UtcNow,
                DateLastDrink = DateTime.UtcNow,
                FeedingCount = 1,
                DrinkingCount = 1
            };

            var validationRezult = _validator.Validate(dataPet);
            var managerRezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && CheckFarmId(farmId, managerRezult) && IsUniqueName(name, managerRezult))
            {
                _petRepository.Add(dataPet);
                _petRepository.Save();
            }
            return managerRezult;
        }

        /// <summary>
        /// Updates the pet name with a special <paramref name="id"/> 
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="name">New name for the pet</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult Update(int id, string name)
        {

            var managerRez = new ManagerRezult();
            if (IsPetAlive(id, managerRez))
            {
                var dataPet = _petRepository.GetItemById(id);
                dataPet!.Statistic.Name = name;
                var validationRezult = _validator.Validate(dataPet);
                managerRez = new ManagerRezult(validationRezult);
                if (validationRezult.IsValid && IsUniqueName(dataPet.Statistic.Name, managerRez))
                {
                    _petRepository.Update(id, dataPet);
                    _petRepository.Save();
                }
            }
            return managerRez;
        }

        /// <summary>
        /// Feeds a pet with a special id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="feederId">id of the user who initiated the feeding</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult Feed(int id, int feederId)
        {
            var managerRez = new ManagerRezult();
            if (IsPetAlive(id, managerRez))
            {
                var dataPet = _petRepository.GetItemById(id);
                if (dataPet.Farm.Owner.Id == feederId || dataPet.Farm.Owner.GetUserColaborators().Any(x => x.Id == feederId))
                {
                    dataPet.Statistic.FeedingCount++;
                    dataPet.Statistic.DateLastFeed = DateTime.UtcNow;

                    _petRepository.Update(id, dataPet);
                    _petRepository.Save();

                }
                else
                {
                    managerRez.Errors.Add($"a user with id = {feederId} cannot feed a pet");
                }
            }
            return managerRez;
        }

        /// <summary>
        /// Gives a drink to a  pet with a special id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="drinkerId">id of the user who initiated the drinking</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult GiveDrink(int id, int drinkerId)
        {
            var managerRez = new ManagerRezult();
            if (IsPetAlive(id, managerRez))
            {
                var dataPet = _petRepository.GetItemById(id);
                if (dataPet.Farm.Owner.Id == drinkerId || dataPet.Farm.Owner.GetUserColaborators().Any(x => x.Id == drinkerId))
                {
                    dataPet.Statistic.DrinkingCount++;
                    dataPet.Statistic.DateLastDrink = DateTime.UtcNow;

                    _petRepository.Update(id, dataPet);
                    _petRepository.Save();
                }
                else
                {
                    managerRez.Errors.Add($"a user with id = {drinkerId} cannot give a drink to a pet");
                }
            }
            return managerRez;
        }

        /// <summary>
        /// Sets the dead status to the pet
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult SetDeadStatus(int id, DateTime deadDate)
        {
            var managerRez = new ManagerRezult();
            if (IsPetAlive(id, managerRez))
            {
                var dataPet = _petRepository.GetItemById(id);
                dataPet!.Statistic.DeadDate = deadDate;
                dataPet.Statistic.IsAlive = false;

                _petRepository.Update(id, dataPet);
                _petRepository.Save();
            }
            return managerRez;
        }

        /// <summary>
        /// Deletes the pet 
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult Delete(int id)
        {
            var managerRez = new ManagerRezult();
            if (CheckPetId(id, managerRez))
            {
                _petRepository.Delete(id);
            }
            return managerRez;
        }

        /// <returns>pet with special <paramref name="id"/> </returns>
        public PetDTO? GetPetById(int id)
        {
            var petData = _petRepository.GetItemById(id);
            var pet = _mapper.Map<PetDTO>(petData);
            return pet;
        }

        /// <returns>Filtered and sorted list of pets</returns>
        public IEnumerable<PetDTO> GetPets(Filtrator<Pet>? filtrator = null, Sorter<Pet>? sorter = null)
        {
            var pets = GetPetsQuary(filtrator, sorter);
            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> pets</returns>
        public IEnumerable<PetDTO> GetPetsPage(int pageSize, int pageNumber, Filtrator<Pet>? filtrator = null, Sorter<Pet>? sorter = null)
        {
            var pets = GetPetsQuary(filtrator, sorter);
            pets = pets.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }

        private IQueryable<Pet> GetPetsQuary(Filtrator<Pet>? filtrator = null, Sorter<Pet>? sorter = null)
        {
            var pets = _petRepository.GetItems();
            pets = filtrator != null ? filtrator.Filter(pets) : pets;
            pets = sorter != null ? sorter.Sort(pets) : pets;
            return pets;
        }

        private bool IsUniqueName(string name, ManagerRezult managerRezult)
        {
            if (_petRepository.IsItemExist(x => x.Statistic.Name == name))
            {
                managerRezult.Errors.Add("A pet with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private bool CheckPetId(int petId, ManagerRezult rezult)
        {
            if (!_petRepository.IsItemExist(petId))
            {
                rezult.Errors.Add("The pet ID is not in the database");
                return false;
            }
            return true;
        }
        private bool CheckFarmId(int farmId, ManagerRezult rezult)
        {
            if (!_farmRepository.IsItemExist(farmId))
            {
                rezult.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
        private bool IsPetAlive(int id, ManagerRezult rezult)
        {
            if (CheckPetId(id, rezult))
            {
                var pet = _petRepository.GetItemById(id);
                if (pet!.Statistic.IsAlive != false)
                {
                    return true;
                }

                rezult.Errors.Add("The pet dead already");
            }
            return false;
        }
    }
}
