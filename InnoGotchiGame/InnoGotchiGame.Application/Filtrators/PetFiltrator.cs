using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
	public class PetFiltrator : Filtrator<Pet>
	{
		public string Name { get; set; } = "";


		internal override IQueryable<Pet> Filter(IQueryable<Pet> query)
		{
			return query.Where(x => x.Statistic.Name.Contains(Name));
		}
	}
}
