﻿using InnoGotchiGame.Domain.Enums;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Domain
{
    public class User : IUser
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHach { get; set; }

        public IPicture? Picture { get; set; }
        public int OwnPetFarmId { get; set; }
        public IPetFarm? OwnPetFarm { get; set; }

        public IList<IColaborationRequest> SentColaborations { get; private set; }
        public IList<IColaborationRequest> AcceptedColaborations { get; private set; }

        private User() { }
        private User(string firstName, string lastName, string email, string passwordHach, IPicture? picture)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHach = passwordHach;
            Picture = picture;
            SentColaborations = new List<IColaborationRequest>();
            AcceptedColaborations = new List<IColaborationRequest>();
        }
        public User(string firstName, string lastName, string email, string passwordHach, IPicture? picture, IPetFarm ownPetFarm) 
            : this(firstName, lastName, email, passwordHach, picture)
        {
            OwnPetFarm = ownPetFarm;
            OwnPetFarmId = ownPetFarm.Id;
        }
        public User(string firstName, string lastName, string email, string passwordHach, IPicture? picture, int ownPetFarmId)
            : this(firstName, lastName, email, passwordHach, picture)
        {
            OwnPetFarmId = ownPetFarmId;
        }

        /// <returns>All colaborators of user</returns>
        public IEnumerable<IUser> GetColaborators()
        {
            Func<IColaborationRequest, bool> whereFunc = x => x.Status == ColaborationRequestStatus.Colaborators;

            var friends = new List<IUser>();
            friends.AddRange(AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
            friends.AddRange(SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
            return friends;
        }
    }
}
