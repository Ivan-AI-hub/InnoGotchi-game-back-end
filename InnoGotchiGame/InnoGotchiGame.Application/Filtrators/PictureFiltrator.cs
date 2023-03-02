using InnoGotchiGame.Application.Filtrators.Base;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;

namespace InnoGotchiGame.Application.Filtrators
{
    public class PictureFiltrator : Filtrator<IPicture>
    {
        public string Name { get; set; } = "";
        public string Format { get; set; } = "";
        public string Description { get; set; } = "";
        internal override IQueryable<IPicture> Filter(IQueryable<IPicture> pictures)
        {
            return pictures.Where(x => x.Name.Contains(Name) &&
                                    x.Format.Contains(Format) &&
                                    x.Description.Contains(Description));
        }
    }
}
