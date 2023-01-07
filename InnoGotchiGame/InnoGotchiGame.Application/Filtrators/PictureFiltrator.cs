using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PictureFiltrator : Filtrator<Picture>
    {
        public string Name { get; set; } = "";
        public string Format { get; set; } = "";
        public string Description { get; set; } = "";
        internal override IQueryable<Picture> Filter(IQueryable<Picture> query)
        {
            return query.Where(x => x.Name.Contains(Name) &&
                                    x.Format.Contains(Format) &&
                                    x.Description.Contains(Description));
        }
    }
}
