﻿using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters.Base;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a pet farm
    /// </summary>
    public class PetFarmManager
    {
        private IValidator<IPetFarm> _validator;
        private IRepositoryManager _repositoryManager;
        private IPetFarmRepository _farmRepository;
        private IMapper _mapper;

        public PetFarmManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<IPetFarm> validator)
        {
            _validator = validator;
            _repositoryManager = repositoryManager;
            _farmRepository = repositoryManager.PetFarm;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a pet farm for the <paramref name="ownerId"/> user with the name <paramref name="name"/>
        /// </summary>
        /// <param name="ownerId">Id of the user who owns the farm</param>
        /// <param name="name">Farm name</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> AddAsync(int ownerId, string name, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsNameUniqueAsync(name, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataFarm = new PetFarm(name, ownerId);

            var validationResult = _validator.Validate(dataFarm);
            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            _farmRepository.Create(dataFarm);
            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataFarm);

            return managerResult;
        }

        /// <summary>
        /// Updates the farm name with a special <paramref name="farmId"/> 
        /// </summary>
        /// <param name="farmId">Farm farmId</param>
        /// <param name="newName">New name for the farm</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateNameAsync(int farmId, string newName, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsNameUniqueAsync(newName, managerResult, cancellationToken) || !await IsFarmIdExistAsync(farmId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var dataFarm = await _farmRepository.GetItems(true).FirstAsync(x => x.Id == farmId, cancellationToken);
            dataFarm.Name = newName;

            var validationResult = _validator.Validate(dataFarm);
            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(dataFarm);
            return managerResult;
        }

        /// <summary>
        /// Deletes the pet farm
        /// </summary>
        /// <param name="farmId">Farm farmId</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int farmId, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsFarmIdExistAsync(farmId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var farm = await _farmRepository.GetItems(false).FirstAsync(x => x.Id == farmId, cancellationToken);
            _farmRepository.Delete(farm);
            await _repositoryManager.SaveAsync(cancellationToken);
            return managerResult;
        }

        /// <returns>farm with special <paramref name="farmId"/> </returns>
        public async Task<PetFarmDTO?> GetFarmByIdAsync(int farmId, CancellationToken cancellationToken = default)
        {
            var dataFarm = await _farmRepository.GetItems(false).FirstOrDefaultAsync(x => x.Id == farmId, cancellationToken);
            return _mapper.Map<PetFarmDTO>(dataFarm);
        }

        /// <returns>Filtered and sorted list of farms</returns>
        public async Task<IEnumerable<PetFarmDTO>> GetPetFarmsAsync(Filtrator<IPetFarm>? filtrator = null,
                                                                    Sorter<IPetFarm>? sorter = null,
                                                                    CancellationToken cancellationToken = default)
        {
            var farms = await GetPetFarmsQuary(filtrator, sorter).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PetFarmDTO>>(farms);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> farms</returns>
        public async Task<IEnumerable<PetFarmDTO>> GetPetFarmsPageAsync(int pageSize,
                                                                        int pageNumber,
                                                                        Filtrator<IPetFarm>? filtrator = null,
                                                                        Sorter<IPetFarm>? sorter = null,
                                                                        CancellationToken cancellationToken = default)
        {
            var farms = GetPetFarmsQuary(filtrator, sorter);
            farms = farms.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            var farmsList = await farms.ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PetFarmDTO>>(farmsList);
        }

        private IQueryable<IPetFarm> GetPetFarmsQuary(Filtrator<IPetFarm>? filtrator = null, Sorter<IPetFarm>? sorter = null)
        {
            var farms = _farmRepository.GetItems(false);
            farms = filtrator != null ? filtrator.Filter(farms) : farms;
            farms = sorter != null ? sorter.Sort(farms) : farms;
            return farms;
        }

        private async Task<bool> IsNameUniqueAsync(string name, ManagerResult managerResult, CancellationToken cancellationToken)
        {
            if (await _farmRepository.IsItemExistAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken))
            {
                managerResult.Errors.Add("A farm with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> IsFarmIdExistAsync(int farmId, ManagerResult managerResult, CancellationToken cancellationToken)
        {
            if (await _farmRepository.IsItemExistAsync(x => x.Id == farmId, cancellationToken))
            {
                return true;
            }

            managerResult.Errors.Add("The farm ID is not in the database");
            return false;
        }
    }
}
