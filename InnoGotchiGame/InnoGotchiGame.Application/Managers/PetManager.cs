using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a pet 
    /// </summary>
    public class PetManager
    {
        private IValidator<IPet> _validator;
        private IRepositoryManager _repositoryManager;
        private IPetRepository _petRepository;
        private IPetFarmRepository _farmRepository;
        private IMapper _mapper;

        public PetManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<IPet> validator)
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
        /// <param name="farmId">petId of the farm containing the pet</param>
        /// <param name="name">IPet name</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> AddAsync(int farmId, string name, PetViewDTO? view, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();

            if (!await IsNameUniqueAsync(name, managerResult, cancellationToken) || !await IsFarmIdExistAsync(farmId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var petStatistic = new PetStatistic(name);
            var dataPet = new Pet(petStatistic, _mapper.Map<IPetView>(view), farmId);

            var validationResult = _validator.Validate(dataPet);

            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            _petRepository.Create(dataPet);
            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataPet);

            return managerResult;
        }

        /// <summary>
        /// Updates the pet name with a special <paramref name="petId"/> 
        /// </summary>
        /// <param name="petId">IPet petId</param>
        /// <param name="name">New name for the pet</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateAsync(int petId, string name, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();

            if (!await IsNameUniqueAsync(name, managerResult, cancellationToken) || !await IsPetAliveAsync(petId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataPet = await _petRepository.GetItems(true).FirstAsync(x => x.Id == petId, cancellationToken);
            dataPet.Statistic.Name = name;

            var validationResult = _validator.Validate(dataPet);
            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataPet);
            return managerResult;
        }

        /// <summary>
        /// Feeds a pet with a special petId
        /// </summary>
        /// <param name="petId">IPet petId</param>
        /// <param name="feederId">petId of the user who initiated the feeding</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> FeedAsync(int petId, int feederId, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsPetAliveAsync(petId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataPet = await _petRepository.GetFullData(true).FirstAsync(x => x.Id == petId, cancellationToken);
            if (dataPet.Farm.Owner.Id != feederId && !dataPet.Farm.Owner.GetColaborators().Any(x => x.Id != feederId))
            {
                managerResult.Errors.Add($"a user with petId = {feederId} cannot feed a pet");
                return managerResult;
            }

            dataPet.Statistic.Feed();

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataPet);

            return managerResult;
        }

        /// <summary>
        /// Gives a drink to a  pet with a special petId
        /// </summary>
        /// <param name="petId">IPet petId</param>
        /// <param name="drinkerId">petId of the user who initiated the drinking</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> GiveDrinkAsync(int petId, int drinkerId, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsPetAliveAsync(petId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataPet = await _petRepository.GetFullData(true).FirstAsync(x => x.Id == petId, cancellationToken);
            if (dataPet.Farm.Owner.Id != drinkerId && !dataPet.Farm.Owner.GetColaborators().Any(x => x.Id != drinkerId))
            {
                managerResult.Errors.Add($"a user with id = {drinkerId} cannot give a drink to a pet");
                return managerResult;
            }

            dataPet.Statistic.Drink();

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataPet);

            return managerResult;
        }

        /// <summary>
        /// Sets the dead status to the pet
        /// </summary>
        /// <param name="petId">IPet petId</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> SetDeadStatusAsync(int petId, DateTime deadDate, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsPetAliveAsync(petId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataPet = await _petRepository.GetItems(true).FirstAsync(x => x.Id == petId, cancellationToken);
            dataPet.Statistic.DeadDate = deadDate;

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataPet);

            return managerResult;
        }

        /// <summary>
        /// Resets HappinessDay for the pet
        /// </summary>
        /// <param name="petId">IPet petId</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> ResetHappinessDayAsync(int petId, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsPetAliveAsync(petId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataPet = await _petRepository.GetItems(true).FirstAsync(x => x.Id == petId, cancellationToken);
            dataPet.Statistic.ResetFirstHappinessDay();

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataPet);

            return managerResult;
        }

        /// <summary>
        /// Deletes the pet 
        /// </summary>
        /// <param name="petId">IPet petId</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int petId, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsPetIdExistAsync(petId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var pet = await _petRepository.GetItems(false).FirstAsync(x => x.Id == petId, cancellationToken);
            _petRepository.Delete(pet);

            return managerResult;
        }

        /// <returns>pet with special <paramref name="petId"/> </returns>
        public async Task<PetDTO?> GetPetByIdAsync(int petId, CancellationToken cancellationToken = default)
        {
            var petData = await _petRepository.GetItems(false).FirstAsync(x => x.Id == petId, cancellationToken);
            return _mapper.Map<PetDTO>(petData);
        }

        /// <returns>Filtered and sorted list of pets</returns>
        public async Task<IEnumerable<PetDTO>> GetPetsAsync(Filtrator<IPet>? filtrator = null,
                                                            Sorter<IPet>? sorter = null,
                                                            CancellationToken cancellationToken = default)
        {
            var pets = await GetPetsQuary(filtrator, sorter).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> pets</returns>
        public async Task<IEnumerable<PetDTO>> GetPetsPageAsync(int pageSize,
                                                                int pageNumber,
                                                                Filtrator<IPet>? filtrator = null,
                                                                Sorter<IPet>? sorter = null,
                                                                CancellationToken cancellationToken = default)
        {
            var pets = GetPetsQuary(filtrator, sorter);
            pets = pets.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            var petsList = await pets.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PetDTO>>(petsList);
        }

        private IQueryable<IPet> GetPetsQuary(Filtrator<IPet>? filtrator = null, Sorter<IPet>? sorter = null)
        {
            var pets = _petRepository.GetItems(false);
            pets = filtrator != null ? filtrator.Filter(pets) : pets;
            pets = sorter != null ? sorter.Sort(pets) : pets;
            return pets;
        }

        private async Task<bool> IsNameUniqueAsync(string name, ManagerResult managerResult, CancellationToken cancellationToken = default)
        {
            if (await _petRepository.IsItemExistAsync(x => x.Statistic.Name.ToLower() == name.ToLower(), cancellationToken))
            {
                managerResult.Errors.Add("A pet with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> IsPetIdExistAsync(int petId, ManagerResult managerResult, CancellationToken cancellationToken = default)
        {
            if (!await _petRepository.IsItemExistAsync(x => x.Id == petId, cancellationToken))
            {
                managerResult.Errors.Add("The pet ID is not in the database");
                return false;
            }
            return true;
        }
        private async Task<bool> IsFarmIdExistAsync(int farmId, ManagerResult managerResult, CancellationToken cancellationToken = default)
        {
            if (!await _farmRepository.IsItemExistAsync(x => x.Id == farmId, cancellationToken))
            {
                managerResult.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
        private async Task<bool> IsPetAliveAsync(int petId, ManagerResult managerResult, CancellationToken cancellationToken = default)
        {
            if (!await IsPetIdExistAsync(petId, managerResult, cancellationToken))
            {
                return false;
            }

            var pet = await _petRepository.GetItems(false).FirstAsync(x => x.Id == petId, cancellationToken);

            if (!pet.Statistic.IsAlive)
            {
                managerResult.Errors.Add("The pet dead already");
                return false;
            }

            return true;
        }
    }
}
