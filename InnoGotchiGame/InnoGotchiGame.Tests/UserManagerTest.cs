namespace InnoGotchiGame.Tests
{
    public class UserManagerTest
    {
        private static int emailId;
        private IFixture _fixture;

        public UserManagerTest()
        {
            var contextOptions = new DbContextOptionsBuilder<InnoGotchiGameContext>()
                            .UseInMemoryDatabase("UserManagerTest")
                            .Options;

            var context = new InnoGotchiGameContext(contextOptions);
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register<IRepository<User>>(() => new UserRepository(context));
            _fixture.Register<AbstractValidator<User>>(() => new UserValidator());
            
            var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile>() { new AssemblyMappingProfile()}));
            _fixture.Register<IMapper>(() => new Mapper(config));
        }

        [Fact]
        public void Add_Valid_User()
        {
            
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var rez = manager.Add(user, "Test_1234");
            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Add_Invalid_User()
        {
            var user = GetInvalidUser();
            var manager = _fixture.Create<UserManager>();

            var rez = manager.Add(user, "Test_1234");

            Assert.False(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Update_Data_Valid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            manager.Add(user, "Test_1234");

            user.FirstName = "SecondUpdate";

            var rez = manager.UpdateData(user.Id, user);

            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
            manager.GetUserById(user.Id)!.FirstName.Should().Be("SecondUpdate");
        }

        [Fact]
        public void Update_Data_Invalid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            manager.Add(user, "Test_1234");

            user.FirstName = "";

            var rez = manager.UpdateData(user.Id, user);

            Assert.False(rez.IsComplete);
        }

        [Fact]
        public void Update_Password_Valid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var firstPassword = _fixture.Create<string>();
            var secondPassword = _fixture.Create<string>();
            manager.Add(user, firstPassword);

            var rez = manager.UpdatePassword(user.Id, firstPassword, secondPassword);

            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Update_Password_InValid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var firstPassword = _fixture.Create<string>();
            var secondPassword = _fixture.Create<string>();

            manager.Add(user, firstPassword);

            var rez = manager.UpdatePassword(user.Id, secondPassword, firstPassword);

            Assert.False(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Update_non_existent_id()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var rez = manager.UpdateData(100, user);

            Assert.False(rez.IsComplete);
        }

        [Fact]
        public void Delete_Valid_User()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            manager.Add(user, "Test_1234");
            var rez = manager.Delete(user.Id);

            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Get_Users_Successfully()
        {
            
            var manager = _fixture.Create<UserManager>();
            var users = new List<UserDTO>();
            int usersCount = 4;
            for (int i = 0; i < usersCount; i++)
            {
                var user = GetValidUser();
                users.Add(user);
            }

            users.ForEach(x => manager.Add(x, "Test_1234"));

            var dataBaseUsers = manager.GetUsers().ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].FirstName.Should().Be(dataBaseUsers.FirstOrDefault(x => x.Id == users[i].Id)?.FirstName);
            }
        }

        [Fact]
        public void FindUser_Successfully()
        {

            var user = GetValidUser();
            var password = _fixture.Create<string>();
            var manager = _fixture.Create<UserManager>();

            manager.Add(user, password);
            var findedUser = manager.FindUserInDb(user.Email, password);

            findedUser!.Email.Should().Be(user.Email);
            findedUser.Id.Should().Be(user.Id);
        }

        private UserDTO GetValidUser()
        {
            var user = GetInvalidUser();
            user.Email = $"test{emailId}@mail.com";
            emailId++;
            return user;
        }

        private UserDTO GetInvalidUser()
        {
            var user = _fixture.Build<UserDTO>()
                .Without(x => x.Id)
                .Without(x => x.OwnPetFarm)
                .Without(x => x.Picture)
                .Without(x => x.AcceptedColaborations)
                .Without(x => x.SentColaborations)
                .Create();
            return user;
        }
    }
}