using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Sorters;

namespace InnoGotchiGame.Web.Models.Pets
{
    public class PetFiltrationViewModel
    {
        public PetFiltrator? Filtrator { get; set; }
        public PetSorter? Sorter { get; set; }
    }
}
