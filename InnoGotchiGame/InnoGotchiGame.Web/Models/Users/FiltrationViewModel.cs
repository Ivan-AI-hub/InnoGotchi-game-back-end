using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Sorters;

namespace InnoGotchiGame.Web.Models.Users
{
    public class FiltrationViewModel
    {
        public UserFiltrator? Filtrator { get; set; }
        public UserSorter? Sorter { get; set; }
    }
}
