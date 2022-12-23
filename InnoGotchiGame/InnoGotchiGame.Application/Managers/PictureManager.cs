using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Application.Managers
{
    public class PictureManager
    {
        private AbstractValidator<Picture> _validator;
        private IRepository<Picture> _repository;
        private IMapper _mapper;
        public PictureManager(IRepository<Picture> repository, IMapper mapper, AbstractValidator<Picture> validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public ManagerRezult Add(PictureDTO picture)
        {
            var pictureData = _mapper.Map<Picture>(picture);
            var validationRezult = _validator.Validate(pictureData);
            var rezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && IsUniqueName(pictureData.Name, rezult))
            {
                picture.Id = _repository.Add(pictureData);
                _repository.Save();
            }
            return rezult;
        }

        public ManagerRezult Update(int updatedId, PictureDTO picture)
        {
            var pictureData = _mapper.Map<Picture>(picture);
            var validationRezult = _validator.Validate(pictureData);
            var rezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && CheckPictureId(updatedId, rezult) && IsUniqueName(pictureData.Name, rezult))
            {
                _repository.Update(updatedId, pictureData);
                _repository.Save();
                picture.Id = updatedId;
            }
            return rezult;
        }
        public ManagerRezult Delete(int id)
        {
            var managerRez = new ManagerRezult();
            if (CheckPictureId(id, managerRez))
            {
                _repository.Delete(id);
            }
            return managerRez;
        }

        public PictureDTO? GetById(int id)
        {
            var picture = _mapper.Map<PictureDTO>(_repository.GetItemById(id));
            return picture;
        }

        public IEnumerable<PictureDTO> GetAll(string nameTemplate = "")
        {
            var pictures = _repository.GetItems().Where(x => x.Name.Contains(nameTemplate)).OrderBy(x => x.Name);
            return _mapper.Map<IEnumerable<PictureDTO>>(pictures);

        }

        private bool IsUniqueName(string name, ManagerRezult managerRezult)
        {
            var quary = _repository.GetItems();
            if (quary.Any(x => x.Name == name))
            {
                managerRezult.Errors.Add("A picture with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private bool CheckPictureId(int id, ManagerRezult rezult)
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
