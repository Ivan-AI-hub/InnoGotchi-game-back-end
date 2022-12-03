using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Web.Models.Pets;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
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
		public IActionResult Post(AddPetModel addPetModel)
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
		public IActionResult Put(UpdatePetModel updatePetModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			PetDTO pet = _mapper.Map<PetDTO>(updatePetModel);

			var rezult = _petManager.Update(updatePetModel.UpdatedId, pet);

			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}

		[HttpGet]
		public IEnumerable<PetDTO> Get(PetFiltrator? filtrator = null, PetSorter? sorter = null)
		{
			var Pets = _petManager.GetPets(filtrator, sorter);
			return Pets;
		}

		[HttpGet("{PetId}")]
		public IActionResult GetById(int PetId)
		{
			var Pet = _petManager.GetPetById(PetId);
			if (Pet == null)
				return BadRequest(new { errorText = "Invalid id." });

			return new ObjectResult(Pet);
		}
	}
}
