using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;

namespace InnoGotchiGame.Persistence.Models
{
    public record PetView : IPetView
    {
        public IPicture? Picture { get; set; }
    }
}
