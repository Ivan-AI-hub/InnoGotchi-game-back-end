namespace InnoGotchiGame.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHach { get; set; }

        public Picture? Picture { get; set; }
        public int OwnPetFarmId { get; set; }
        public PetFarm? OwnPetFarm { get; set; }

        public List<ColaborationRequest> SentColaborations { get; set; }
        public List<ColaborationRequest> AcceptedColaborations { get; set; }

        public User()
        {
            SentColaborations = new List<ColaborationRequest>();
            AcceptedColaborations = new List<ColaborationRequest>();
        }

        /// <returns>All colaborators of user</returns>
        public IEnumerable<User> GetUserColaborators()
        {
            Func<ColaborationRequest, bool> whereFunc = x => x.Status == ColaborationRequestStatus.Colaborators;

            List<User> friends = new List<User>();
            friends.AddRange(AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
            friends.AddRange(SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
            return friends;
        }
    }
}
