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
    /// Manager for working with a pet farm
    /// </summary>
    public class PetFarmManager
    {
        private IValidator<PetFarm> _validator;
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<PetFarm> _farmRepository;
        private IMapper _mapper;

        public PetFarmManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<PetFarm> validator)
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
        public async Task<ManagerResult> AddAsync(int ownerId, string name)
        {
            var dataFarm = new PetFarm();
            dataFarm.CreateDate = DateTime.Now;
            dataFarm.Name = name;
            dataFarm.OwnerId = ownerId;

            var validationResult = _validator.Validate(dataFarm);
            var result = new ManagerResult(validationResult);
            if (validationResult.IsValid && await IsUniqueNameAsync(dataFarm.Name, result))
            {
                _farmRepository.Create(dataFarm);
                _repositoryManager.SaveAsync().Wait();
            }
            return result;
        }

        /// <summary>
        /// Updates the farm name with a special <paramref name="id"/> 
        /// </summary>
        /// <param name="id">Farm id</param>
        /// <param name="newName">New name for the farm</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateNameAsync(int id, string newName)
        {
            var managerRez = new ManagerResult();
            if (await CheckFarmIdAsync(id, managerRez))
            {
                var dataFarm = await _farmRepository.FirstOrDefaultAsync(x => x.Id == id, false);
                dataFarm!.Name = newName;
                var validationResult = _validator.Validate(dataFarm);
                managerRez = new ManagerResult(validationResult);
                if (validationResult.IsValid && await IsUniqueNameAsync(newName, managerRez))
                {
                    _farmRepository.Update(dataFarm);
                    _repositoryManager.SaveAsync().Wait();
                }
            }
            return managerRez;
        }

        /// <summary>
        /// Deletes the pet farm
        /// </summary>
        /// <param name="id">Farm id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int id)
        {
            var managerRez = new ManagerResult();
            if (await CheckFarmIdAsync(id, managerRez))
            {
                var farm = await _farmRepository.FirstOrDefaultAsync(x => x.Id == id, false);
                _farmRepository.Delete(farm!);
                _repositoryManager.SaveAsync().Wait();
            }
            return managerRez;
        }

        /// <returns>farm with special <paramref name="id"/> </returns>
        public async Task<PetFarmDTO?> GetFarmByIdAsync(int id)
        {
            var dataFarm = await _farmRepository.FirstOrDefaultAsync(x => x.Id == id, false);
            var farm = _mapper.Map<PetFarmDTO>(dataFarm);
            return farm;
        }

        /// <returns>Filtered and sorted list of farms</returns>
        public async Task<IEnumerable<PetFarmDTO>> GetPetFarmsAsync(Filtrator<PetFarm>? filtrator = null, Sorter<PetFarm>? sorter = null)
        {
            var farms = await GetPetFarmsQuary(filtrator, sorter).ToListAsync();
            return _mapper.Map<IEnumerable<PetFarmDTO>>(farms);
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> farms</returns>
        public async Task<IEnumerable<PetFarmDTO>> GetPetFarmsPageAsync(int pageSize, int pageNumber, Filtrator<PetFarm>? filtrator = null, Sorter<PetFarm>? sorter = null)
        {
            var farms = GetPetFarmsQuary(filtrator, sorter);
            farms = farms.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            var farmsList = await farms.ToListAsync();
            return _mapper.Map<IEnumerable<PetFarmDTO>>(farmsList);
        }

        private async Task<bool> IsUniqueNameAsync(string name, ManagerResult managerResult)
        {
            if (await _farmRepository.IsItemExistAsync(x => x.Name.ToLower() == name.ToLower()))
            {
                managerResult.Errors.Add("A farm with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private IQueryable<PetFarm> GetPetFarmsQuary(Filtrator<PetFarm>? filtrator = null, Sorter<PetFarm>? sorter = null)
        {
            var farms = _farmRepository.GetItems(false);
            farms = filtrator != null ? filtrator.Filter(farms) : farms;
            farms = sorter != null ? sorter.Sort(farms) : farms;
            return farms;
        }

        private async Task<bool> CheckFarmIdAsync(int id, ManagerResult result)
        {
            if (!(await _farmRepository.IsItemExistAsync(x => x.Id == id)))
            {
                result.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
    }
}
