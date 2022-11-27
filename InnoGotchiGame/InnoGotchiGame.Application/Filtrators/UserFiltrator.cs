using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
	public class UserFiltrator : Filtrator<User>
	{
		private string _firstName;
		private string _lastName;
		private string _email;
		private int? _petFarmId;

		public UserFiltrator(string firstName = "", string lastName = "", string email = "", int? petFarmId = null)
		{
			_firstName = firstName;
			_lastName = lastName;
			_email = email;
			_petFarmId = petFarmId;
		}

		internal override IQueryable<User> Filter(IQueryable<User> users)
		{
			return users.Where(x => x.FirstName.Contains(_firstName) &&
									x.LastName.Contains(_lastName) &&
									x.Email.Contains(_email) &&
									(_petFarmId == null || x.OwnPetFarmId == _petFarmId));
		}
	}
}
