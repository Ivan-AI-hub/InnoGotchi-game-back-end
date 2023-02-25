using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Domain
{
    public record PetView : IPetView
    {
        public IPicture? Picture { get; set; }
    }
}
