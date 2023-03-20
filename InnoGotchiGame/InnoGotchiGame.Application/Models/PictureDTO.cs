namespace InnoGotchiGame.Application.Models
{
    public class PictureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
    }
}
