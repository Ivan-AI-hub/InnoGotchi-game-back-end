using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Application.Filtrators
{
    public class UserFiltrator : Filtrator<IUser>
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public int PetFarmId { get; set; } = -1;

        internal override IQueryable<IUser> Filter(IQueryable<IUser> users)
        {
            return users.Where(x => x.FirstName.Contains(FirstName) &&
                                    x.LastName.Contains(LastName) &&
                                    x.Email.Contains(Email) &&
                                    (PetFarmId == -1 || x.OwnPetFarmId == PetFarmId));
        }
    }
}
