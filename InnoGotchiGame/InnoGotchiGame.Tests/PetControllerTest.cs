using InnoGotchiGame.Domain.Interfaces;
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
            repManager.Setup(x => x.PetFarm.IsItemExistAsync(It.IsAny<Expression<Func<IPetFarm, bool>>>())).Returns(Task.FromResult(true));
            var mock = new Mock<PetManager>(repManager.Object, mapper.Object, validator.Object);
            var controller = new PetController(mock.Object);
            var result = await controller.PostAsync(addModel);

            result.Should().BeOfType<OkResult>();
        }
    }
}
