using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Domain
{
    public class Picture : IPicture
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

        public Picture(string name, string format, string description, byte[] image)
        {
            Name = name;
            Format = format;
            Description = description;
            Image = image;
        }
    }
}
