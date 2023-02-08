using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Managers;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a pet 
    /// </summary>
    public class PetManager
    {
        private AbstractValidator<Pet> _validator;
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<Pet> _petRepository;
        private IRepositoryBase<PetFarm> _farmRepository;
        private IMapper _mapper;

        public PetManager(IRepositoryManager repositoryManager, IMapper mapper, AbstractValidator<Pet> validator)
        {
            _validator = validator;
            _repositoryManager = repositoryManager;
            _petRepository = _repositoryManager.Pet;
            _farmRepository = _repositoryManager.PetFarm;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a pet for the <paramref name="farmId"/> farm
        /// </summary>
        /// <param name="farmId">id of the farm containing the pet</param>
        /// <param name="name">Pet name</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> AddAsync(int farmId, string name, PetViewDTO? view)
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
            if (validationRezult.IsValid && await CheckFarmIdAsync(farmId, managerRezult) && await IsUniqueNameAsync(name, managerRezult))
            {
                _petRepository.Create(dataPet);
                _repositoryManager.SaveAsync().Wait();
            }
            return managerRezult;
        }

        /// <summary>
        /// Updates the pet name with a special <paramref name="id"/> 
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="name">New name for the pet</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> UpdateAsync(int id, string name)
        {

            var managerRez = new ManagerRezult();
            if (await IsPetAliveAsync(id, managerRez))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                dataPet!.Statistic.Name = name;
                var validationRezult = _validator.Validate(dataPet);
                managerRez = new ManagerRezult(validationRezult);
                if (validationRezult.IsValid && await IsUniqueNameAsync(dataPet.Statistic.Name, managerRez))
                {
                    _repositoryManager.SaveAsync().Wait();
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
        public async Task<ManagerRezult> FeedAsync(int id, int feederId)
        {
            var managerRez = new ManagerRezult();
            if (await IsPetAliveAsync(id, managerRez))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                if (dataPet!.Farm.Owner.Id == feederId || dataPet.Farm.Owner.GetUserColaborators().Any(x => x.Id == feederId))
                {
                    dataPet.Statistic.FeedingCount++;
                    dataPet.Statistic.DateLastFeed = DateTime.UtcNow;

                    _repositoryManager.SaveAsync().Wait();

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
        public async Task<ManagerRezult> GiveDrinkAsync(int id, int drinkerId)
        {
            var managerRez = new ManagerRezult();
            if (await IsPetAliveAsync(id, managerRez))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                if (dataPet!.Farm.Owner.Id == drinkerId || dataPet.Farm.Owner.GetUserColaborators().Any(x => x.Id == drinkerId))
                {
                    dataPet.Statistic.DrinkingCount++;
                    dataPet.Statistic.DateLastDrink = DateTime.UtcNow;

                    _repositoryManager.SaveAsync().Wait();
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
        public async Task<ManagerRezult> SetDeadStatusAsync(int id, DateTime deadDate)
        {
            var managerRez = new ManagerRezult();
            if (await IsPetAliveAsync(id, managerRez))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                dataPet!.Statistic.DeadDate = deadDate;
                dataPet.Statistic.IsAlive = false;

                _repositoryManager.SaveAsync().Wait();
            }
            return managerRez;
        }

        /// <summary>
        /// Resets HappinessDay for the pet
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> ResetHappinessDayAsync(int id)
        {
            var managerRez = new ManagerRezult();
            if (await IsPetAliveAsync(id, managerRez))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                dataPet!.Statistic.FirstHappinessDay = DateTime.UtcNow;

                _repositoryManager.SaveAsync().Wait();
            }
            return managerRez;
        }

        /// <summary>
        /// Deletes the pet 
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerRezult> DeleteAsync(int id)
        {
            var managerRez = new ManagerRezult();
            if (await CheckPetIdAsync(id, managerRez))
            {
                var pet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, false);
                _petRepository.Delete(pet!);
            }
            return managerRez;
        }

        /// <returns>pet with special <paramref name="id"/> </returns>
        public async Task<PetDTO?> GetPetByIdAsync(int id)
        {
            var petData = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, false);
            var pet = _mapper.Map<PetDTO>(petData);
            return pet;
        }

        /// <returns>Filtered and sorted list of pets</returns>
        public async Task<IEnumerable<PetDTO>> GetPetsAsync(Filtrator<Pet>? filtrator = null, Sorter<Pet>? sorter = null)
        {
            var pets = await GetPetsQuary(filtrator, sorter).ToListAsync();
            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> pets</returns>
        public async Task<IEnumerable<PetDTO>> GetPetsPageAsync(int pageSize, int pageNumber, Filtrator<Pet>? filtrator = null, Sorter<Pet>? sorter = null)
        {
            var pets = GetPetsQuary(filtrator, sorter);
            pets = pets.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            var petsList = await pets.ToListAsync();
            return _mapper.Map<IEnumerable<PetDTO>>(petsList);
        }

        private IQueryable<Pet> GetPetsQuary(Filtrator<Pet>? filtrator = null, Sorter<Pet>? sorter = null)
        {
            var pets = _petRepository.GetItems(false);
            pets = filtrator != null ? filtrator.Filter(pets) : pets;
            pets = sorter != null ? sorter.Sort(pets) : pets;
            return pets;
        }

        private async Task<bool> IsUniqueNameAsync(string name, ManagerRezult managerRezult)
        {
            if (await _petRepository.IsItemExistAsync(x => x.Statistic.Name.ToLower() == name.ToLower()))
            {
                managerRezult.Errors.Add("A pet with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckPetIdAsync(int petId, ManagerRezult rezult)
        {
            if (await _petRepository.IsItemExistAsync(x => x.Id == petId))
            {
                return true;
            }
            rezult.Errors.Add("The pet ID is not in the database");
            return false;
        }
        private async Task<bool> CheckFarmIdAsync(int farmId, ManagerRezult rezult)
        {
            if (!(await _farmRepository.IsItemExistAsync(x => x.Id == farmId)))
            {
                rezult.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
        private async Task<bool> IsPetAliveAsync(int id, ManagerRezult rezult)
        {
            if (await CheckPetIdAsync(id, rezult))
            {
                var pet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, false);
                if (pet.Statistic.IsAlive)
                {
                    return true;
                }

                rezult.Errors.Add("The pet dead already");
            }
            return false;
        }
    }
}
