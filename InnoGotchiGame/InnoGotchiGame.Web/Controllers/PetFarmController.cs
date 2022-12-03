using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Web.Models.PetFarms;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
	[Route("/api/farms")]
	public class PetFarmController : BaseController
	{
		private PetFarmManager _farmManager;
		private IMapper _mapper;

		public PetFarmController(PetFarmManager farmManager, IMapper mapper)
		{
			_farmManager = farmManager;
			_mapper = mapper;
		}

		[HttpPost]
		public IActionResult Post(AddPetFarmModel addFarmModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var rezult = _farmManager.Add(addFarmModel.OwnerId, addFarmModel.Name);
			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}

		[HttpPut]
		public IActionResult Put(UpdatePetFarmModel updatePetFarmModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var rezult = _farmManager.UpdateName(updatePetFarmModel.UpdatedId, updatePetFarmModel.Name);

			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Ok();
		}

		[HttpGet]
		public IActionResult Get(PetFarmFiltrator? filtrator = null, PetFarmSorter? sorter = null)
		{
			var farms = _farmManager.GetPetFarms(filtrator, sorter);
			return new ObjectResult(farms);
		}

		[HttpGet("{farmId}")]
		public IActionResult GetById(int farmId)
		{
			var farm = _farmManager.GetFarmById(farmId);
			if (farm == null)
				return BadRequest(new { errorText = "Invalid id." });

			return new ObjectResult(farm);
		}
	}
}
