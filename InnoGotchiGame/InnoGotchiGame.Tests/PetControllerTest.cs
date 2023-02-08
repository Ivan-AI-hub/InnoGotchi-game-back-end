using InnoGotchiGame.Persistence.Managers;
using InnoGotchiGame.Web.Controllers;
using InnoGotchiGame.Web.Models.Pets;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

namespace InnoGotchiGame.Tests
{
    public class PetControllerTest
    {
        [Theory]
        [AutoMoqData]
        public async void Post_Successfully(AddPetModel addModel,
                Mock<IRepositoryManager> repManager,
                Mock<IMapper> mapper,
                Mock<AbstractValidator<Pet>> validator)
        {
            repManager.Setup(x => x.PetFarm.IsItemExistAsync(It.IsAny<Expression<Func<PetFarm, bool>>>())).Returns(Task.FromResult(true));
            var mock = new Mock<PetManager>(repManager.Object, mapper.Object, validator.Object);
            var controller = new PetController(mock.Object);
            var result = await controller.PostAsync(addModel);

            result.Should().BeOfType<OkResult>();
        }

        [Theory]
        [AutoMoqData]
        public async void Put_Successfully(UpdatePetModel updateModel,
                    Mock<IRepositoryManager> repManager,
                    Mock<IMapper> mapper,
                    Mock<AbstractValidator<Pet>> validator)
        {
            repManager
                .Setup(x => x.Pet.IsItemExistAsync(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(false));
            repManager
                .Setup(x => x.Pet.FirstOrDefaultAsync(It.IsAny<Expression<Func<Pet, bool>>>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new Pet() { Statistic = new PetStatistic() { IsAlive = true } }));

            var mock = new Mock<PetManager>(repManager.Object, mapper.Object, validator.Object);
            var controller = new PetController(mock.Object);
            var result = await controller.PutAsync(updateModel);

            result.Should().BeOfType<AcceptedResult>();
        }
    }
}
