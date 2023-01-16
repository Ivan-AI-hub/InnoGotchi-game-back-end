namespace InnoGotchiGame.Domain
{
    public class Pet
    {
        public int Id { get; set; }

        public PetStatistic Statistic { get; set; }
        public PetView View { get; set; }

        public int FarmId { get; set; }
        public PetFarm Farm { get; set; }

        public Pet()
        {
            Statistic = new PetStatistic();
            View = new PetView();
        }
    }
}
