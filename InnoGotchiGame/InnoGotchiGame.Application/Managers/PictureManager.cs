using AutoMapper;
using FluentValidation;
using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;
using InnoGotchiGame.Domain.BaseModels;
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
        public async Task<ManagerResult> AddAsync(PictureDTO picture, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await IsNameUniqueAsync(picture.Name, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var pictureData = _mapper.Map<IPicture>(picture);
            var validationResult = _validator.Validate(pictureData);

            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            _pictureRepository.Create(pictureData);
            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(pictureData);

            return managerResult;
        }

        /// <summary>
        /// Updates the picture with a special pictureId
        /// </summary>
        /// <param name="updatedId">Id of the picture being updated</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> UpdateAsync(int updatedId, PictureDTO newPicture, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await CheckPictureIdAsync(updatedId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            var pictureData = await _pictureRepository.GetItems(true).FirstAsync(x => x.Id == updatedId, cancellationToken);
            pictureData.Description = newPicture.Description;
            pictureData.Format = newPicture.Format;
            pictureData.Image = newPicture.Image;

            var validationResult = _validator.Validate(pictureData);

            if (!validationResult.IsValid)
            {
                return new ManagerResult(validationResult);
            }

            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(pictureData);

            newPicture.Id = updatedId;
            return managerResult;
        }

        /// <summary>
        /// Deletes the picture
        /// </summary>
        /// <param name="pictureId">IPicture pictureId</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteAsync(int pictureId, CancellationToken cancellationToken = default)
        {
            var managerResult = new ManagerResult();
            if (!await CheckPictureIdAsync(pictureId, managerResult, cancellationToken))
            {
                return managerResult;
            }

            _pictureRepository.Delete(await _pictureRepository.GetItems(false).FirstAsync(x => x.Id == pictureId, cancellationToken));
            await _repositoryManager.SaveAsync(cancellationToken);

            return managerResult;
        }

        /// <returns>picture with special <paramref name="pictureId"/> </returns>
        public async Task<PictureDTO?> GetByIdAsync(int pictureId, CancellationToken cancellationToken = default)
        {
            var picture = _mapper.Map<PictureDTO>(await _pictureRepository.GetItems(false).FirstAsync(x => x.Id == pictureId, cancellationToken));
            return picture;
        }

        /// <returns>Filtered list of pictures</returns>
        public async Task<IEnumerable<PictureDTO>> GetAllAsync(Filtrator<IPicture>? filtrator, CancellationToken cancellationToken = default)
        {
            var pictures = _pictureRepository.GetItems(false);
            pictures = filtrator != null ? filtrator.Filter(pictures) : pictures;
            pictures = pictures.OrderBy(x => x.Name);
            return _mapper.Map<IEnumerable<PictureDTO>>(await pictures.ToListAsync(cancellationToken));

        }

        private async Task<bool> IsNameUniqueAsync(string name, ManagerResult managerResult, CancellationToken cancellationToken = default)
        {
            if (await _pictureRepository.IsItemExistAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken))
            {
                managerResult.Errors.Add("A picture with the same Name already exists in the database");
                return false;
            }
            return true;
        }

        private async Task<bool> CheckPictureIdAsync(int pictureId, ManagerResult managerResult, CancellationToken cancellationToken = default)
        {
            if (!await _pictureRepository.IsItemExistAsync(x => x.Id == pictureId, cancellationToken))
            {
                managerResult.Errors.Add("The farm ID is not in the database");
                return false;
            }
            return true;
        }
    }
}
