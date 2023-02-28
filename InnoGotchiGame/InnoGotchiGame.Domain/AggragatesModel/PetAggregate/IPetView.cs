using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;

namespace InnoGotchiGame.Domain.AggragatesModel.PetAggregate
{
    public interface IPetView
    {
        IPicture? Picture { get; set; }
    }
}