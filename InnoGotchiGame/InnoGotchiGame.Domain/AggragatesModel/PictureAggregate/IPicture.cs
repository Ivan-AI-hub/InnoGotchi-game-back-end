namespace InnoGotchiGame.Domain.AggragatesModel.PictureAggregate
{
    public interface IPicture
    {
        int Id { get; }
        string Name { get; set; }
        string Description { get; set; }
        string Format { get; set; }
        byte[] Image { get; set; }
    }
}