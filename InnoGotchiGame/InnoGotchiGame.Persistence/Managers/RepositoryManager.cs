﻿using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence.Repositories;

namespace InnoGotchiGame.Persistence.Managers
{
    public class RepositoryManager : IRepositoryManager
    {
        public InnoGotchiGameContext _context;
        public IColaborationRequestRepository _colaborationRequestRepository;
        public IPetFarmRepository _petFarmRepository;
        public IPetRepository _petRepository;
        public IPictureRepository _pictureRepository;
        public IUserRepository _userRepository;

        public RepositoryManager(InnoGotchiGameContext context)
        {
            _context = context;
        }

        public IColaborationRequestRepository ColaborationRequest
        {
            get
            {
                if (_colaborationRequestRepository == null)
                    _colaborationRequestRepository = new ColaborationRequestRepository(_context);
                return _colaborationRequestRepository;
            }
        }

        public IPetFarmRepository PetFarm
        {
            get
            {
                if (_petFarmRepository == null)
                    _petFarmRepository = new PetFarmRepository(_context);
                return _petFarmRepository;
            }
        }

        public IPetRepository Pet
        {
            get
            {
                if (_petRepository == null)
                    _petRepository = new PetRepository(_context);
                return _petRepository;
            }
        }

        public IPictureRepository Picture
        {
            get
            {
                if (_pictureRepository == null)
                    _pictureRepository = new PictureRepository(_context);
                return _pictureRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_context);
                return _userRepository;
            }
        }

        public void Detach(object item)
        {
            _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
