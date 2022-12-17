using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Sorters;

namespace InnoGotchiGame.Web.Models.PetFarms
{
    public class FarmFiltrationViewModel
    {
        public PetFarmFiltrator? Filtrator { get; set; }
        public PetFarmSorter? Sorter { get; set; }
    }
}
