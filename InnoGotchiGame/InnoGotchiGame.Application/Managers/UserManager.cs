using InnoGotchiGame.Application.Interfaces;
using InnoGotchiGame.Application.Services;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Managers
{
	public class UserManager
	{
		/// <returns>All friends of user</returns>
		public IEnumerable<User> GetUserFriends(User user)
		{
			Func<FriendlyRelation, bool> whereFunc = x => x.Status == FriendlyRelationStatus.Friends;

			List<User> friends = new List<User>();
			friends.AddRange(user.AcceptedFriendships.Where(whereFunc).Select(x => x.FirstFriend));
			friends.AddRange(user.SentFriendships.Where(whereFunc).Select(x => x.SecondFriend));
			return friends;
		}

		/// <returns>All unconfirmed invitations to be friends</returns>
		public IEnumerable<FriendlyRelation> GetUnconfirmedInvite(User user) 
		{
			var invites = user.AcceptedFriendships.Where(x => x.Status == FriendlyRelationStatus.Undefined);
			return invites;
		}

		/// <summary>
		/// Sets the password hash
		/// </summary>
		public void SetPasswordHash(User user, string password)
		{
			user.PasswordHach = StringToHach(password);
		}

		/// <summary>
		/// Finds user in data base
		/// </summary>
		/// <param name="service">service for interacting with the database</param>
		/// <param name="email">User email</param>
		/// <param name="password">User password</param>
		/// <returns>Found user or null if the user was not found</returns>
		public User? FindUserInDb(UserService service, string email, string password)
		{
			int passwordHach = StringToHach(password);
			return service.GetUser(x => x.Email == email && x.PasswordHach == passwordHach);
		}

		private int StringToHach(string read)
		{
			int hashedValue = 3074457;
			for (int i = 0; i < read.Length; i++)
			{
				hashedValue += read[i];
				hashedValue *= 3074457;
			}
			return hashedValue;
		}
	}
}
