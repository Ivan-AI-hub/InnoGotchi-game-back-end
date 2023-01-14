using InnoGotchiGame.Application.Filtrators;

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

            _fixture.Register<IRepository<Pet>>(() => new PetRepository(context));
            _fixture.Register<AbstractValidator<Pet>>(() => new PetValidator());
            _fixture.Register<IRepository<PetFarm>>(() => new PetFarmRepository(context));

            var config = new MapperConfiguration(cnf => cnf.AddProfiles(new List<Profile>() { new AssemblyMappingProfile() }));
            _fixture.Register<IMapper>(() => new Mapper(config));
        }

        [Fact]
        public void Add_Valid_Pet()
        {

            var farmId = GetFarmId();
            var petName = _fixture.Create<string>();
            var view = _fixture.Create<PetViewDTO>();
            var manager = _fixture.Create<PetManager>();

            var rez = manager.Add(farmId, petName, view);

            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Add_Invalid_Pet()
        {

            var farmId = GetFarmId();
            var petName = "";
            var view = _fixture.Create<PetViewDTO>();
            var manager = _fixture.Create<PetManager>();

            var rez = manager.Add(farmId, petName, view);

            Assert.False(rez.IsComplete, String.Concat(rez.Errors));
        }

        [Fact]
        public void Update_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);
            var petSecondName = _fixture.Create<string>();

            //act

            var rez = manager.Update(pet.Id, petSecondName);
            var updatedPet = new PetDTO();
            if (rez.IsComplete)
            {
                updatedPet = manager.GetPets(new PetFiltrator() { Name = petSecondName }).First();
            }

            //assert
            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
            updatedPet.Statistic.Name.Should().Be(petSecondName);
        }

        [Fact]
        public void Feed_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);

            //act
            var rez = manager.Feed(pet.Id, pet.Farm.OwnerId);
            var newPet = manager.GetPetById(pet.Id);
            //assert
            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
            newPet!.Statistic.FeedingCount.Should().BeGreaterThan(pet.Statistic.FeedingCount);
        }

        [Fact]
        public void Give_Drink_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);

            //act
            var rez = manager.GiveDrink(pet.Id, pet.Farm.OwnerId);
            var newPet = manager.GetPetById(pet.Id);
            //assert
            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
            newPet!.Statistic.DrinkingCount.Should().BeGreaterThan(pet.Statistic.DrinkingCount);
        }

        [Fact]
        public void Set_Dead_Status_Successfully()
        {
            //arrange
            var manager = _fixture.Create<PetManager>();
            var pet = GetValidPet(manager);

            //act
            var rez = manager.SetDeadStatus(pet.Id);
            var newPet = manager.GetPetById(pet.Id);
            //assert
            Assert.True(rez.IsComplete, String.Concat(rez.Errors));
            newPet!.Statistic.IsAlive.Should().BeFalse();
            newPet!.Statistic.DeadDate.Should().NotBeNull();
        }

        private int GetFarmId()
        {
            var farmRepository = _fixture.Create<IRepository<PetFarm>>();
            var farm = _fixture.Build<PetFarm>()
                .Without(x => x.Id)
                .Without(x => x.OwnerId)
                .Without(x => x.Owner)
                .Create();

            farm.Owner = _fixture.Build<User>()
                .Without(x => x.Id)
                .Without(x => x.OwnPetFarm)
                .Without(x => x.Picture)
                .Without(x => x.AcceptedColaborations)
                .Without(x => x.SentColaborations)
                .Create();

            var farmId = farmRepository.Add(farm);
            farmRepository.Save();
            return farmId;
        }

        private PetDTO GetValidPet(PetManager manager)
        {
            var farmId = GetFarmId();
            var name = _fixture.Create<string>();
            var view = _fixture.Create<PetViewDTO>();

            manager.Add(farmId, name, view);
            var pet = manager.GetPets(new PetFiltrator() { Name = name }).First();
            return manager.GetPetById(pet.Id)!;
        }
    }
}
