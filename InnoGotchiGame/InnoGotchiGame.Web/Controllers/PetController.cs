using AutoMapper;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Web.Models.Pets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("/api/pets")]
    public class PetController : BaseController
    {
        private PetManager _petManager;
        private IMapper _mapper;

        public PetController(PetManager petManager, IMapper mapper)
        {
            _petManager = petManager;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddPetModel addPetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            PetDTO pet = _mapper.Map<PetDTO>(addPetModel);
            pet.Statistic = new PetStatisticDTO() { Name = addPetModel.Name };

            var rezult = _petManager.Add(addPetModel.FarmId, pet);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdatePetModel updatePetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _petManager.Update(updatePetModel.UpdatedId, updatePetModel.Name);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpPut("{petId}/feed")]
        public IActionResult Feed(int petId, int feederId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _petManager.Feed(petId, feederId);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpPut("{petId}/drink")]
        public IActionResult GiveDrink(int petId, int drinkerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _petManager.GiveDrink(petId, drinkerId);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpPut("{petId}/dead")]
        public IActionResult SetDeadStatus(int petId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _petManager.SetDeadStatus(petId);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpGet]
        public IEnumerable<PetDTO> Get([FromBody] PetFiltrationViewModel filtration)
        {
            var pets = _petManager.GetPets(filtration.Filtrator, filtration.Sorter);
            return pets;
        }

        [HttpGet("{pageSize}/{pageNumber}")]
        public IEnumerable<PetDTO> GetPage(int pageSize, int pageNumber, PetFiltrationViewModel filtration)
        {
            var pets = _petManager.GetPetsPage(pageSize, pageNumber, filtration.Filtrator, filtration.Sorter);
            return pets;
        }

        [HttpGet("{petId}")]
        public IActionResult GetById(int petId)
        {
            var pet = _petManager.GetPetById(petId);
            if (pet == null)
                return BadRequest(new { errorText = "Invalid id." });

            return new ObjectResult(pet);
        }
    }
}
