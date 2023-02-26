using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Persistence.Managers;

namespace InnoGotchiGame.Tests
{
    public class PetManagerTest
    {
        private IFixture _fixture;
        public PetManagerTest()
        {
            var contextOptions = new DbContextOptionsBuilder<InnoGotchiGameContext>()
                    .UseInMemoryDatabase("PetManagerTest")
                    .Options;

            var context = new InnoGotchiGameContext(contextOptions);
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());


            _fixture.Register<IRepositoryManager>(() => new RepositoryManager(context));
            _fixture.Register<IValidator<Pet>>(() => new PetValidator());

            var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile>() { new AssemblyMappingProfile() }));
            _fixture.Register<IMapper>(() => new Mapper(config));
        }

        [Fact]
        public async void Add_Valid_Pet()
        {

            var farmId = GetFarmId();
            var petName = _fixture.Create<string>();
            var view = _fixture.Create<PetViewDTO>();
            var manager = _fixture.Create<PetManager>();

            var result = await manager.AddAsync(farmId, petName, view);

            Assert.True(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Add_Invalid_Pet()
        {

            var farmId = GetFarmId();
            var petName = "";
            var view = _fixture.Create<PetViewDTO>();
            var manager = _fixture.Create<PetManager>();

            var result = await manager.AddAsync(farmId, petName, view);

            Assert.False(result.IsComplete, String.Concat(result.Errors));
        }

        [Fact]
        public async void Update_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);
            var petSecondName = _fixture.Create<string>();

            //act

            var result = await manager.UpdateAsync(pet.Id, petSecondName);
            var updatedPet = new PetDTO();
            if (result.IsComplete)
            {
                updatedPet = (await manager.GetPetsAsync(new PetFiltrator() { Name = petSecondName })).First();
            }

            //assert
            Assert.True(result.IsComplete, String.Concat(result.Errors));
            updatedPet.Statistic.Name.Should().Be(petSecondName);
        }

        [Fact]
        public async void Feed_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);

            //act
            var result = await manager.FeedAsync(pet.Id, pet.Farm.OwnerId);
            var newPet = await manager.GetPetByIdAsync(pet.Id);
            //assert
            Assert.True(result.IsComplete, String.Concat(result.Errors));
            newPet!.Statistic.FeedingCount.Should().BeGreaterThan(pet.Statistic.FeedingCount);
        }

        [Fact]
        public async void Give_Drink_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);

            //act
            var result = await manager.GiveDrinkAsync(pet.Id, pet.Farm.OwnerId);
            var newPet = await manager.GetPetByIdAsync(pet.Id);
            //assert
            Assert.True(result.IsComplete, String.Concat(result.Errors));
            newPet!.Statistic.DrinkingCount.Should().BeGreaterThan(pet.Statistic.DrinkingCount);
        }

        [Fact]
        public async void Set_Dead_Status_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);

            //act
            var result = await manager.SetDeadStatusAsync(pet.Id, DateTime.UtcNow);
            var newPet = await manager.GetPetByIdAsync(pet.Id);
            //assert
            Assert.True(result.IsComplete, String.Concat(result.Errors));
            newPet!.Statistic.IsAlive.Should().BeFalse();
            newPet!.Statistic.DeadDate.Should().NotBeNull();
        }

        private int GetFarmId()
        {
            var repManager = _fixture.Create<IRepositoryManager>();
            var farm = _fixture.Build<PetFarm>().Create();

            repManager.PetFarm.Create(farm);
            repManager.SaveAsync().Wait();
            return farm.Id;
        }

        private PetDTO GetValidPet(PetManager manager)
        {
            var farmId = GetFarmId();
            var name = _fixture.Create<string>();
            var view = _fixture.Create<PetViewDTO>();

            var repository = _fixture.Create<IRepositoryManager>();
            var mapper = _fixture.Create<IMapper>();

            manager.AddAsync(farmId, name, view).Wait();
            var pet = repository.Pet.FirstOrDefaultAsync(x => x.Statistic.Name == name, false).Result;
            return mapper.Map<PetDTO>(pet);
        }
    }
}
