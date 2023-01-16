using InnoGotchiGame.Web.Controllers;
using InnoGotchiGame.Web.Models.Pets;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InnoGotchiGame.Tests
{
    public class PetControllerTest
    {
        [Theory]
        [AutoMoqData]
        public void Post_Successfully(AddPetModel addModel,
                Mock<IRepository<Pet>> petRepository,
                Mock<IRepository<PetFarm>> farmRepository,
                Mock<IMapper> mapper,
                Mock<AbstractValidator<Pet>> validator)
        {
            farmRepository.Setup(x => x.IsItemExist(It.IsAny<int>())).Returns(true);
            var mock = new Mock<PetManager>(petRepository.Object, farmRepository.Object, mapper.Object, validator.Object);
            var controller = new PetController(mock.Object);
            var rezult = controller.Post(addModel);

            rezult.Should().BeOfType<OkResult>();
        }

        [Theory]
        [AutoMoqData]
        public void Put_Successfully(UpdatePetModel updateModel,
                    Mock<IRepository<Pet>> petRepository,
                    Mock<IRepository<PetFarm>> farmRepository,
                    Mock<IMapper> mapper,
                    Mock<AbstractValidator<Pet>> validator)
        {
            petRepository.Setup(x => x.IsItemExist(It.IsAny<int>())).Returns(true);
            petRepository.Setup(x => x.GetItemById(It.IsAny<int>())).Returns(new Pet() { Statistic = new PetStatistic() { IsAlive = true} });
            var mock = new Mock<PetManager>(petRepository.Object, farmRepository.Object, mapper.Object, validator.Object);
            var controller = new PetController(mock.Object);
            var rezult = controller.Put(updateModel);

            rezult.Should().BeOfType<AcceptedResult>();
        }
    }
}
