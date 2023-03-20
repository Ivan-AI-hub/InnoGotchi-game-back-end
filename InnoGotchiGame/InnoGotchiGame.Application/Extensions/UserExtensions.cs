using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Application.Extensions
{
    internal static class UserExtensions
    {

        public static IEnumerable<IUser> GetColaborators(this IUser user)
        {
            Func<IColaborationRequest, bool> whereFunc = x => x.Status == ColaborationRequestStatus.Colaborators;

            var colaborators = new List<IUser>();
            colaborators.AddRange(user.AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
            colaborators.AddRange(user.SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
            return colaborators;
        }
    }
}
