using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence.Managers;
using InnoGotchiGame.Persistence.Repositories;
using Moq;

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
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IUserRepository)))
                .Returns(new UserRepository(context));

            _fixture.Register<IRepositoryManager>(() => new RepositoryManager(context, serviceProvider.Object));
            _fixture.Register<IValidator<IUser>>(() => new UserValidator());

            var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile>() { new AssemblyMappingProfile() }));
            _fixture.Register<IMapper>(() => new Mapper(config));
        }

        [Fact]
        public async void Add_Valid_User()
        {

            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var result = await manager.AddAsync(user, "Test_1234");
            Assert.True(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Add_Invalid_User()
        {
            var user = GetInvalidUser();
            var manager = _fixture.Create<UserManager>();

            var result = await manager.AddAsync(user, "Test_1234");

            Assert.False(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Update_Data_Valid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            await manager.AddAsync(user, "Test_1234");

            user.FirstName = "SecondUpdate";

            var result = await manager.UpdateDataAsync(user.Id, user);

            Assert.True(result.IsComplete, String.Concat(result.Errors));
            (await manager.GetUserByIdAsync(user.Id)).FirstName.Should().Be("SecondUpdate");
        }

        [Fact]
        public async void Update_Data_Invalid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            await manager.AddAsync(user, "Test_1234");

            user.FirstName = "";

            var result = await manager.UpdateDataAsync(user.Id, user);

            Assert.False(result.IsComplete);
        }

        [Fact]
        public async void Update_Password_Valid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var firstPassword = _fixture.Create<string>();
            var secondPassword = _fixture.Create<string>();
            await manager.AddAsync(user, firstPassword);

            var result = await manager.UpdatePasswordAsync(user.Id, firstPassword, secondPassword);

            Assert.True(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Update_Password_InValid()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var firstPassword = _fixture.Create<string>();
            var secondPassword = _fixture.Create<string>();

            await manager.AddAsync(user, firstPassword);

            var result = await manager.UpdatePasswordAsync(user.Id, secondPassword, firstPassword);

            Assert.False(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Update_non_existent_id()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            var result = await manager.UpdateDataAsync(100, user);

            Assert.False(result.IsComplete);
        }

        [Fact]
        public async void Delete_Valid_User()
        {
            var user = GetValidUser();
            var manager = _fixture.Create<UserManager>();

            await manager.AddAsync(user, "Test_1234");
            var result = await manager.DeleteAsync(user.Id);

            Assert.True(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Get_Users_Successfully()
        {

            var manager = _fixture.Create<UserManager>();
            var users = new List<UserDTO>();
            int usersCount = 4;
            for (int i = 0; i < usersCount; i++)
            {
                var user = GetValidUser();
                users.Add(user);
            }

            users.ForEach(async x => await manager.AddAsync(x, "Test_1234"));

            var dataBaseUsers = (await manager.GetUsersAsync()).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].FirstName.Should().Be(dataBaseUsers.FirstOrDefault(x => x.Id == users[i].Id)?.FirstName);
            }
        }

        [Fact]
        public async void FindUser_Successfully()
        {

            var user = GetValidUser();
            var password = _fixture.Create<string>();
            var manager = _fixture.Create<UserManager>();

            await manager.AddAsync(user, password);
            var findedUser = await manager.FindUserInDbAsync(user.Email, password);

            findedUser!.Email.Should().Be(user.Email);
            findedUser.Id.Should().Be(user.Id);
        }

        private UserDTO GetValidUser()
        {
            var user = GetInvalidUser();
            user.FirstName = $"test{emailId}";
            user.LastName = $"test{emailId}";
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