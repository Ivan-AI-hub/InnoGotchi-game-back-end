using InnoGotchiGame.Application.Interfaces;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Managers
{
	public class UserManager : Service
	{
		public UserManager(IInnoGotchiGameContext context) : base(context)
		{
		}

		/// <returns>All friends of user</returns>
		public IEnumerable<User> GetUserFriends(User user)
		{
			Func<FriendlyRelation, bool> whereFunc = x => x.Status == FriendlyRelationStatus.Friends;

			List<User> friends = new List<User>();
			friends.AddRange(user.AcceptedFriendships.Where(whereFunc).Select(x => x.FirstFriend));
			friends.AddRange(user.SentFriendships.Where(whereFunc).Select(x => x.SecondFriend));
			return friends;
		}

		public IEnumerable<FriendlyRelation> GetUnconfirmedInvite(User user) 
		{
			var invites = user.AcceptedFriendships.Where(x => x.Status == FriendlyRelationStatus.Undefined);
			return invites;
		}

		public void SetPassword(User user, string password)
		{
			user.PasswordHach = StringToHach(password);
		}

		private int StringToHach(string text)
		{
			return text.GetHashCode();
		}
	}
}
