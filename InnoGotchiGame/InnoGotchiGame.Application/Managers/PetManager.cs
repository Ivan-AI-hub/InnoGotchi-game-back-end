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
        private IValidator<Pet> _validator;
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<Pet> _petRepository;
        private IRepositoryBase<PetFarm> _farmRepository;
        private IMapper _mapper;

        public PetManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<Pet> validator)
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
        public async Task<ManagerResult> AddAsync(int farmId, string name, PetViewDTO? view)
        {
            var petStatistic = new PetStatistic(name);
            var dataPet = new Pet(petStatistic, _mapper.Map<PetView>(view), farmId);

            var validationResult = _validator.Validate(dataPet);
            var managerResult = new ManagerResult(validationResult);
            if (validationResult.IsValid && await CheckFarmIdAsync(farmId, managerResult) && await IsUniqueNameAsync(name, managerResult))
            {
                _petRepository.Create(dataPet);
                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(dataPet);
            }
            return managerResult;
        }

        /// <summary>
        /// Updates the pet name with a special <paramref name="id"/> 
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="name">New name for the pet</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateAsync(int id, string name)
        {

            var result = new ManagerResult();
            if (await IsPetAliveAsync(id, result))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                dataPet!.Statistic.Name = name;
                var validationResult = _validator.Validate(dataPet);
                result = new ManagerResult(validationResult);
                if (validationResult.IsValid && await IsUniqueNameAsync(dataPet.Statistic.Name, result))
                {
                    await _repositoryManager.SaveAsync();
                    _repositoryManager.Detach(dataPet);
                }
            }
            return result;
        }

        /// <summary>
        /// Feeds a pet with a special id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="feederId">id of the user who initiated the feeding</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> FeedAsync(int id, int feederId)
        {
            var result = new ManagerResult();
            if (await IsPetAliveAsync(id, result))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                if (dataPet!.Farm.Owner.Id == feederId || dataPet.Farm.Owner.GetColaborators().Any(x => x.Id == feederId))
                {
                    dataPet.Statistic.Feed();

                    await _repositoryManager.SaveAsync();
                    _repositoryManager.Detach(dataPet);
                }
                else
                {
                    result.Errors.Add($"a user with id = {feederId} cannot feed a pet");
                }
            }
            return result;
        }

        /// <summary>
        /// Gives a drink to a  pet with a special id
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <param name="drinkerId">id of the user who initiated the drinking</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> GiveDrinkAsync(int id, int drinkerId)
        {
            var result = new ManagerResult();
            if (await IsPetAliveAsync(id, result))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                if (dataPet!.Farm.Owner.Id == drinkerId || dataPet.Farm.Owner.GetColaborators().Any(x => x.Id == drinkerId))
                {
                    dataPet.Statistic.Drink();

                    await _repositoryManager.SaveAsync();
                    _repositoryManager.Detach(dataPet);
                }
                else
                {
                    result.Errors.Add($"a user with id = {drinkerId} cannot give a drink to a pet");
                }
            }
            return result;
        }

        /// <summary>
        /// Sets the dead status to the pet
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> SetDeadStatusAsync(int id, DateTime deadDate)
        {
            var result = new ManagerResult();
            if (await IsPetAliveAsync(id, result))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                dataPet!.Statistic.DeadDate = deadDate;

                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(dataPet);
            }
            return result;
        }

        /// <summary>
        /// Resets HappinessDay for the pet
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> ResetHappinessDayAsync(int id)
        {
            var result = new ManagerResult();
            if (await IsPetAliveAsync(id, result))
            {
                var dataPet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, true);
                dataPet!.Statistic.ResetFirstHappinessDay();

                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(dataPet);
            }
            return result;
        }

        /// <summary>
        /// Deletes the pet 
        /// </summary>
        /// <param name="id">Pet id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int id)
        {
            var result = new ManagerResult();
            if (await CheckPetIdAsync(id, result))
            {
                var pet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, false);
                _petRepository.Delete(pet!);
            }
            return result;
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

        private async Task<bool> IsUniqueNameAsync(string name, ManagerResult managerResult)
        {
            if (await _petRepository.IsItemExistAsync(x => x.Statistic.Name.ToLower() == name.ToLower()))
            {
                managerResult.Errors.Add("A pet with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckPetIdAsync(int petId, ManagerResult result)
        {
            if (await _petRepository.IsItemExistAsync(x => x.Id == petId))
            {
                return true;
            }
            result.Errors.Add("The pet ID is not in the database");
            return false;
        }
        private async Task<bool> CheckFarmIdAsync(int farmId, ManagerResult result)
        {
            if (await _farmRepository.IsItemExistAsync(x => x.Id == farmId))
            {
                return true;
            }
            result.Errors.Add("The farm ID is not in the database");
            return false;
        }
        private async Task<bool> IsPetAliveAsync(int id, ManagerResult result)
        {
            if (await CheckPetIdAsync(id, result))
            {
                var pet = await _petRepository.FirstOrDefaultAsync(x => x.Id == id, false);
                if (pet.Statistic.IsAlive)
                {
                    return true;
                }

                result.Errors.Add("The pet dead already");
            }
            return false;
        }
    }
}
