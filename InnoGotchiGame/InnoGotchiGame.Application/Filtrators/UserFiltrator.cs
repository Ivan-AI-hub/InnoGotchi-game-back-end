using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
    public class UserFiltrator : Filtrator<User>
	{
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public string Email { get; set; } = "" ;
		public int PetFarmId { get; set; } = -1;

		internal override IQueryable<User> Filter(IQueryable<User> users)
		{
			return users.Where(x => x.FirstName.Contains(FirstName) &&
									x.LastName.Contains(LastName) &&
									x.Email.Contains(Email) &&
									(PetFarmId == -1 || x.OwnPetFarmId == PetFarmId));
		}
	}
}
