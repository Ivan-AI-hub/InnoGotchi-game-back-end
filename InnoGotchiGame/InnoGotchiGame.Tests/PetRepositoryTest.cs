using InnoGotchiGame.Persistence.Managers;

namespace InnoGotchiGame.Tests
{
    public class PetRepositoryTest
    {
        private IFixture _fixture;

        public PetRepositoryTest()
        {
            var contextOptions = new DbContextOptionsBuilder<InnoGotchiGameContext>()
                    .UseInMemoryDatabase(nameof(PetRepositoryTest))
                    .Options;

            var context = new InnoGotchiGameContext(contextOptions);
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _fixture.Register<IRepositoryManager>(() => new RepositoryManager(context));
        }

        [Fact]
        public void Add_Successfuly()
        {
            var repManager = _fixture.Create<IRepositoryManager>();
            var pet = _fixture.Create<Pet>();

            repManager.Pet.Create(pet);

            pet.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async void Delete_Successfuly()
        {
            var repositoryManager = _fixture.Create<IRepositoryManager>();
            var pet = await GetValidPetAsync();

            repositoryManager.Pet.Create(pet);
            repositoryManager.SaveAsync().Wait();
            repositoryManager.Pet.Delete(pet);
            repositoryManager.SaveAsync().Wait();
            var dataPet = await repositoryManager.Pet.FirstOrDefaultAsync(x => x.Id == pet.Id, false);

            dataPet.Should().BeNull();
        }

        private async Task<Pet> GetValidPetAsync()
        {
            var pet = _fixture.Build<Pet>()
                .Without(x => x.Id)
                .Without(x => x.FarmId)
                .Without(x => x.Farm)
                .Without(x => x.View)
                .Create();
            pet.View = _fixture.Build<PetView>()
                .Without(x => x.Picture).Create();
            pet.FarmId = await GetFarmIdAsync();
            return pet;
        }
        private async Task<int> GetFarmIdAsync()
        {
            var repositoryManager = _fixture.Create<IRepositoryManager>();
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

            repositoryManager.PetFarm.Create(farm);
            await repositoryManager.SaveAsync();
            return farm.Id;
        }
    }
}
