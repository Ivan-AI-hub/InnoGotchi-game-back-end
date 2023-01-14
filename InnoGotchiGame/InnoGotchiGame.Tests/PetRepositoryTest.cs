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

            _fixture.Register<PetRepository>(() => new PetRepository(context));
            _fixture.Register<IRepository<PetFarm>>(() => new PetFarmRepository(context));
        }

        [Fact]
        public void Add_Successfuly()
        {
            var repository = _fixture.Create<PetRepository>();
            var pet = _fixture.Create<Pet>();

            var petId = repository.Add(pet);

            petId.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Delete_Successfuly()
        {
            var repository = _fixture.Create<PetRepository>();
            var pet = GetValidPet();

            var petId = repository.Add(pet);
            repository.Save();
            repository.Delete(petId);
            repository.Save();
            var dataPet = repository.GetItemById(petId);

            dataPet.Should().BeNull();
        }

        private Pet GetValidPet()
        {
            var pet = _fixture.Build<Pet>()
                .Without(x => x.Id)
                .Without(x => x.FarmId)
                .Without(x => x.Farm)
                .Without(x => x.View)
                .Create();
            pet.View = _fixture.Build<PetView>()
                .Without(x => x.Picture).Create();
            pet.FarmId = GetFarmId();
            return pet;
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
    }
}
