namespace InnoGotchiGame.Domain
{
    public record PetView
    {
        public Picture? BodyPicture { get; set; }
        public Picture? EyePicture { get; set; }
        public Picture? NosePicture { get; set; }
        public Picture? MouthPicture { get; set; }
    }
}
