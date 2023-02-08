using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class ColaborationRequestRepository : RepositoryBase<ColaborationRequest>, IColaborationRequestRepository
    {
        public ColaborationRequestRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<ColaborationRequest> GetItems(bool trackChanges)
        {
            var requests = Context.ColaborationRequests
                .AsNoTracking()
                .Include(x => x.RequestReceiver)
                .Include(x => x.RequestSender);

            return trackChanges ? requests : requests.AsNoTracking();
        }
    }
}
