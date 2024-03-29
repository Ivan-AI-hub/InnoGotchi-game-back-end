﻿using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence.Managers;
using InnoGotchiGame.Persistence.Models;
using InnoGotchiGame.Persistence.Repositories;
using Moq;

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

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IPetRepository)))
                .Returns(new PetRepository(context));
            _fixture.Register<IRepositoryManager>(() => new RepositoryManager(context, serviceProvider.Object));
            _fixture.Register<IPetStatistic>(() => _fixture.Create<PetStatistic>());
            _fixture.Register<IPetView>(() => new PetView());
            _fixture.Register<IPetFarm>(() => new PetFarm(_fixture.Create<string>(), _fixture.Create<int>()));
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
            var dataPet = await repositoryManager.Pet.GetItems(false).FirstOrDefaultAsync(x => x.Id == pet.Id);

            dataPet.Should().BeNull();
        }

        private async Task<Pet> GetValidPet()
        {
            var pet = _fixture.Build<Pet>().Create();
            return pet;
        }
    }
}
