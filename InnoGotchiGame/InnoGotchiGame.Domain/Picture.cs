namespace InnoGotchiGame.Domain
{
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
    }
}
