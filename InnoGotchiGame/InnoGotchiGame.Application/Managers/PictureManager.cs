using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a picture
    /// </summary>
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

        /// <summary>
        /// Adds <paramref name="picture"/> to database
        /// </summary>
        /// <returns>Result of method execution</returns>
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

        /// <summary>
        /// Updates the picture with a special id
        /// </summary>
        /// <param name="updatedId">Id of the picture being updated</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult Update(int updatedId, PictureDTO newPicture)
        {
            var pictureData = _mapper.Map<Picture>(newPicture);
            var validationRezult = _validator.Validate(pictureData);
            var rezult = new ManagerRezult(validationRezult);
            if (validationRezult.IsValid && CheckPictureId(updatedId, rezult) && IsUniqueName(pictureData.Name, rezult))
            {
                _repository.Update(updatedId, pictureData);
                _repository.Save();
                newPicture.Id = updatedId;
            }
            return rezult;
        }

        /// <summary>
        /// Deletes the picture
        /// </summary>
        /// <param name="id">Picture id</param>
        /// <returns>Result of method execution</returns>
        public ManagerRezult Delete(int id)
        {
            var managerRez = new ManagerRezult();
            if (CheckPictureId(id, managerRez))
            {
                _repository.Delete(id);
            }
            return managerRez;
        }

        /// <returns>picture with special <paramref name="id"/> </returns>
        public PictureDTO? GetById(int id)
        {
            var picture = _mapper.Map<PictureDTO>(_repository.GetItemById(id));
            return picture;
        }

        /// <returns>Filtered list of pictures</returns>
        public IEnumerable<PictureDTO> GetAll(Filtrator<Picture>? filtrator)
        {
            var pictures = _repository.GetItems();
            pictures = filtrator != null ? filtrator.Filter(pictures) : pictures;
            pictures = pictures.OrderBy(x => x.Name);
            return _mapper.Map<IEnumerable<PictureDTO>>(pictures);

        }

        private bool IsUniqueName(string name, ManagerRezult managerRezult)
        {
            if (_repository.IsItemExist(x => x.Name == name))
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
