using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
	public class PetFiltrator : Filtrator<Pet>
	{
		private string _name;

		public PetFiltrator(string name)
		{
			_name = name;
		}

		internal override IQueryable<Pet> Filter(IQueryable<Pet> query)
		{
			return query.Where(x => x.Statistic.Name.Contains(_name));
		}
	}
}
