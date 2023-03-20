using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;
using InnoGotchiGame.Domain.BaseModels;
using Microsoft.Extensions.DependencyInjection;

namespace InnoGotchiGame.Persistence.Managers
{
    public class RepositoryManager : IRepositoryManager
    {
        private InnoGotchiGameContext _context;
        private IServiceProvider _serviceProvider;
        private IColaborationRequestRepository _colaborationRequestRepository;
        private IPetFarmRepository _petFarmRepository;
        private IPetRepository _petRepository;
        private IPictureRepository _pictureRepository;
        private IUserRepository _userRepository;

        public RepositoryManager(InnoGotchiGameContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IColaborationRequestRepository ColaborationRequest
        {
            get
            {
                if (_colaborationRequestRepository == null)
                    _colaborationRequestRepository = _serviceProvider.GetRequiredService<IColaborationRequestRepository>();
                return _colaborationRequestRepository;
            }
        }

        public IPetFarmRepository PetFarm
        {
            get
            {
                if (_petFarmRepository == null)
                    _petFarmRepository = _serviceProvider.GetRequiredService<IPetFarmRepository>();
                return _petFarmRepository;
            }
        }

        public IPetRepository Pet
        {
            get
            {
                if (_petRepository == null)
                    _petRepository = _serviceProvider.GetRequiredService<IPetRepository>();
                return _petRepository;
            }
        }

        public IPictureRepository Picture
        {
            get
            {
                if (_pictureRepository == null)
                    _pictureRepository = _serviceProvider.GetRequiredService<IPictureRepository>();
                return _pictureRepository;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
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
