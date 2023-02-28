using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class ColaborationRequestRepository : RepositoryBase<IColaborationRequest, ColaborationRequest>, IColaborationRequestRepository
    {
        public ColaborationRequestRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<IColaborationRequest> GetItems(bool trackChanges)
        {
            var requests = Context.ColaborationRequests
                .AsNoTracking()
                .Include(x => x.RequestReceiver)
                .Include(x => x.RequestSender);

            return trackChanges ? requests : requests.AsNoTracking();
        }
    }
}
