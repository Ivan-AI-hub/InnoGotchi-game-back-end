using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Managers;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a picture
    /// </summary>
    public class PictureManager
    {
        private IValidator<Picture> _validator;
        private IRepositoryManager _repositoryManager;
        private IRepositoryBase<Picture> _pictureRepository;
        private IMapper _mapper;
        public PictureManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<Picture> validator)
        {
            _repositoryManager = repositoryManager;
            _pictureRepository = repositoryManager.Picture;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>
        /// Adds <paramref name="picture"/> to database
        /// </summary>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> AddAsync(PictureDTO picture)
        {
            var pictureData = _mapper.Map<Picture>(picture);
            var validationResult = _validator.Validate(pictureData);
            var result = new ManagerResult(validationResult);
            if (validationResult.IsValid && await IsUniqueNameAsync(pictureData.Name, result))
            {
                _pictureRepository.Create(pictureData);
                _repositoryManager.SaveAsync().Wait();
            }
            return result;
        }

        /// <summary>
        /// Updates the picture with a special id
        /// </summary>
        /// <param name="updatedId">Id of the picture being updated</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateAsync(int updatedId, PictureDTO newPicture)
        {
            var pictureData = _mapper.Map<Picture>(newPicture);
            var validationResult = _validator.Validate(pictureData);
            var result = new ManagerResult(validationResult);
            if (validationResult.IsValid && await CheckPictureIdAsync(updatedId, result) && await IsUniqueNameAsync(pictureData.Name, result))
            {
                pictureData.Id = updatedId;
                _pictureRepository.Update(pictureData);
                _repositoryManager.SaveAsync().Wait();
                newPicture.Id = updatedId;
            }
            return result;
        }

        /// <summary>
        /// Deletes the picture
        /// </summary>
        /// <param name="id">Picture id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int id)
        {
            var managerRez = new ManagerResult();
            if (await CheckPictureIdAsync(id, managerRez))
            {
                _pictureRepository.Delete(await _pictureRepository.FirstOrDefaultAsync(x => x.Id == id, false));
                _repositoryManager.SaveAsync().Wait();
            }
            return managerRez;
        }

        /// <returns>picture with special <paramref name="id"/> </returns>
        public async Task<PictureDTO?> GetByIdAsync(int id)
        {
            var picture = _mapper.Map<PictureDTO>(await _pictureRepository.FirstOrDefaultAsync(x => x.Id == id, false));
            return picture;
        }

        /// <returns>Filtered list of pictures</returns>
        public async Task<IEnumerable<PictureDTO>> GetAllAsync(Filtrator<Picture>? filtrator)
        {
            var pictures = _pictureRepository.GetItems(false);
            pictures = filtrator != null ? filtrator.Filter(pictures) : pictures;
            pictures = pictures.OrderBy(x => x.Name);
            return _mapper.Map<IEnumerable<PictureDTO>>(await pictures.ToListAsync());

        }

        private async Task<bool> IsUniqueNameAsync(string name, ManagerResult managerResult)
        {
            if (await _pictureRepository.IsItemExistAsync(x => x.Name.ToLower() == name.ToLower()))
            {
                managerResult.Errors.Add("A picture with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckPictureIdAsync(int id, ManagerResult result)
        {
            if (!await _pictureRepository.IsItemExistAsync(x => x.Id == id))
            {
                result.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
    }
}
