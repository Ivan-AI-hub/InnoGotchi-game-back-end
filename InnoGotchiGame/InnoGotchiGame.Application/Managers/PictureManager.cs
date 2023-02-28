using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Managers;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working with a picture
    /// </summary>
    public class PictureManager
    {
        private IValidator<IPicture> _validator;
        private IRepositoryManager _repositoryManager;
        private IPictureRepository _pictureRepository;
        private IMapper _mapper;
        public PictureManager(IRepositoryManager repositoryManager, IMapper mapper, IValidator<IPicture> validator)
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
            var result = new ManagerResult();

            if (!await IsUniqueNameAsync(picture.Name, result))
            {
                return result;
            }

            var pictureData = _mapper.Map<IPicture>(picture);
            var validationResult = _validator.Validate(pictureData);
            result = new ManagerResult(validationResult);

            if (validationResult.IsValid)
            {
                _pictureRepository.Create((Picture)pictureData);//BAD
                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(pictureData);
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
            var result = new ManagerResult();

            if (!await CheckPictureIdAsync(updatedId, result))
            {
                return result;
            }

            var pictureData = await _pictureRepository.FirstOrDefaultAsync(x => x.Id == updatedId, false);
            pictureData.Description = newPicture.Description;
            pictureData.Format = newPicture.Format;
            pictureData.Image = newPicture.Image;

            var validationResult = _validator.Validate(pictureData);
            result = new ManagerResult(validationResult);

            if (validationResult.IsValid)
            {
                _pictureRepository.Update(pictureData);
                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(pictureData);

                newPicture.Id = updatedId;
            }
            return result;
        }

        /// <summary>
        /// Deletes the picture
        /// </summary>
        /// <param name="id">IPicture id</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int id)
        {
            var result = new ManagerResult();
            if (!await CheckPictureIdAsync(id, result))
            {
                return result;
            }

            _pictureRepository.Delete(await _pictureRepository.FirstOrDefaultAsync(x => x.Id == id, false));
            await _repositoryManager.SaveAsync();

            return result;
        }

        /// <returns>picture with special <paramref name="id"/> </returns>
        public async Task<PictureDTO?> GetByIdAsync(int id)
        {
            var picture = _mapper.Map<PictureDTO>(await _pictureRepository.FirstOrDefaultAsync(x => x.Id == id, false));
            return picture;
        }

        /// <returns>Filtered list of pictures</returns>
        public async Task<IEnumerable<PictureDTO>> GetAllAsync(Filtrator<IPicture>? filtrator)
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
