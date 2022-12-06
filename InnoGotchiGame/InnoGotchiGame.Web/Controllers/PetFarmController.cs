using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Web.Models.PetFarms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
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

		/// <summary>
		/// Creates a farm
		/// </summary>
		/// <param name="addFarmModel"></param>
		[HttpPost]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(List<string>), 400)]
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

		/// <summary>
		/// updated a farm
		/// </summary>
		/// <param name="updatePetFarmModel"></param>
		[HttpPut]
		[ProducesResponseType(202)]
		[ProducesResponseType(typeof(List<string>), 400)]
		public IActionResult Put(UpdatePetFarmModel updatePetFarmModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var rezult = _farmManager.UpdateName(updatePetFarmModel.UpdatedId, updatePetFarmModel.Name);

			if (!rezult.IsComplete)
				return BadRequest(rezult.Errors);

			return Accepted();
		}

		/// <param name="filtrator">Filtration rules</param>
		/// <param name="sorter">Sorting rules</param>
		/// <returns>All users from database</returns>
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<PetFarmDTO>),200)]
		public IActionResult Get(PetFarmFiltrator? filtrator = null, PetFarmSorter? sorter = null)
		{
			var farms = _farmManager.GetPetFarms(filtrator, sorter);
			return new ObjectResult(farms);
		}

		/// <param name="farmId"></param>
		/// <returns>a farm with same Id</returns>
		[HttpGet("{farmId}")]
		[ProducesResponseType(typeof(PetFarmDTO), 200)]
		[ProducesResponseType(typeof(string), 400)]
		public IActionResult GetById(int farmId)
		{
			var farm = _farmManager.GetFarmById(farmId);
			if (farm == null)
				return BadRequest(new { errorText = "Invalid id." });

			return new ObjectResult(farm);
		}
	}
}
