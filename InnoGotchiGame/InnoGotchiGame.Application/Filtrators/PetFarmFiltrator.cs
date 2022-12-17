using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PetFarmFiltrator : Filtrator<PetFarm>
    {
        public string Name { get; set; } = "";
        internal override IQueryable<PetFarm> Filter(IQueryable<PetFarm> query)
        {
            return query.Where(x => x.Name.Contains(Name));
        }
    }
}
