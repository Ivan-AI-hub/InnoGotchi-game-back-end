using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Application.Managers
{
    public class PetFarmManager
    {
        private AbstractValidator<PetFarm> _validator;
        private IRepository<PetFarm> _repository;
        private IMapper _mapper;

        public PetFarmManager(IRepository<PetFarm> repository, IMapper mapper, AbstractValidator<PetFarm> validator)
        {
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
        }

        public ManagerRezult Add(int ownerId, string name)
        {
            var dataFarm = new PetFarm();
            dataFarm.CreateDate = DateTime.Now;
            dataFarm.Name = name;
            dataFarm.OwnerId = ownerId;

            var validationRezult = _validator.Validate(dataFarm);
            var rezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && IsUniqueName(dataFarm.Name, rezult))
            {
                _repository.Add(dataFarm);
                _repository.Save();
            }
            return rezult;
        }

        public ManagerRezult UpdateName(int id, string newName)
        {

            var managerRez = new ManagerRezult();
            if (CheckFarmId(id, managerRez))
            {
                var dataFarm = _repository.GetItemById(id);
                dataFarm.Name = newName;
                var validationRezult = _validator.Validate(dataFarm);
                managerRez = new ManagerRezult(validationRezult);
                if (validationRezult.IsValid && IsUniqueName(newName, managerRez))
                {
                    _repository.Update(id, dataFarm);
                    _repository.Save();
                }
            }
            return managerRez;
        }

        public ManagerRezult Delete(int id)
        {
            var managerRez = new ManagerRezult();
            if (CheckFarmId(id, managerRez))
            {
                _repository.Delete(id);
            }
            return managerRez;
        }

        public PetFarmDTO? GetFarmById(int id)
        {
            var dataFarm = _repository.GetItemById(id);
            var farm = _mapper.Map<PetFarmDTO>(dataFarm);
            return farm;
        }

        public IEnumerable<PetFarmDTO> GetPetFarms(PetFarmFiltrator? filtrator = null, PetFarmSorter? sorter = null)
        {
            var farms = GetPetFarmsQuary(filtrator, sorter);
            return QueryablePetFarmToPetFarmDTO(farms);
        }

        public IEnumerable<PetFarmDTO> GetPetFarmsPage(int pageSize, int pageNumber, PetFarmFiltrator? filtrator = null, PetFarmSorter? sorter = null)
        {
            var farms = GetPetFarmsQuary(filtrator, sorter);
            farms = farms.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return QueryablePetFarmToPetFarmDTO(farms);
        }

        private bool IsUniqueName(string name, ManagerRezult managerRezult)
        {
            var quary = GetPetFarmsQuary();
            if (quary.Any(x => x.Name == name))
            {
                managerRezult.Errors.Add("A farm with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private IQueryable<PetFarm> GetPetFarmsQuary(PetFarmFiltrator? filtrator = null, PetFarmSorter? sorter = null)
        {
            var farms = _repository.GetItems();
            farms = filtrator != null ? filtrator.Filter(farms) : farms;
            farms = sorter != null ? sorter.Sort(farms) : farms;
            return farms;
        }

        private IEnumerable<PetFarmDTO> QueryablePetFarmToPetFarmDTO(IQueryable<PetFarm> farms)
        {
            var DTOFarms = new List<PetFarmDTO>();
            foreach (var PetFarm in farms)
                DTOFarms.Add(_mapper.Map<PetFarmDTO>(PetFarm));
            return DTOFarms;
        }

        private bool CheckFarmId(int id, ManagerRezult rezult)
        {
            if (!_repository.IsItemExist(id))
            {
                rezult.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
    }
}
