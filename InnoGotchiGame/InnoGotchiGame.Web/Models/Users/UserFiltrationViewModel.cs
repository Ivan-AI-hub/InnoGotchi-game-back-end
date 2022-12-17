using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Sorters;

namespace InnoGotchiGame.Web.Models.Users
{
    public class UserFiltrationViewModel
    {
        public UserFiltrator? Filtrator { get; set; }
        public UserSorter? Sorter { get; set; }
    }
}
