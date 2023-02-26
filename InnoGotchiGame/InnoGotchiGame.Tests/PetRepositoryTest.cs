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
            var pet = await GetValidPet();

            repositoryManager.Pet.Create(pet);
            repositoryManager.SaveAsync().Wait();
            repositoryManager.Pet.Delete(pet);
            repositoryManager.SaveAsync().Wait();
            var dataPet = await repositoryManager.Pet.FirstOrDefaultAsync(x => x.Id == pet.Id, false);

            dataPet.Should().BeNull();
        }

        private async Task<Pet> GetValidPet()
        {
            var pet = _fixture.Build<Pet>().Create();
            return pet;
        }
    }
}
