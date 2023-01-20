﻿using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Web.Models.PetFarms;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api/farms")]
    public class PetFarmController : BaseController
    {
        private PetFarmManager _farmManager;

        public PetFarmController(PetFarmManager farmManager)
        {
            _farmManager = farmManager;
        }

        /// <summary>
        /// Creates a farm
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Post([FromBody] AddPetFarmModel addFarmModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = GetAuthUserId();
            var rezult = _farmManager.Add(userId, addFarmModel.Name);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// updates a farm
        /// </summary>
        [HttpPut]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Put([FromBody] UpdatePetFarmModel updatePetFarmModel)
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
        /// <returns>All farms from database</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetFarmDTO>), 200)]
        public IActionResult Get(PetFarmFiltrator filtrator, string sortField = "Name", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
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

        private PetFarmSorter GetSorter(string sortRule, bool isDescendingSort)
        {
            var sorter = new PetFarmSorter();
            sorter.SortRule = Enum.Parse<PetFarmSortRule>(sortRule);
            sorter.IsDescendingSort = isDescendingSort;
            return sorter;
        }
    }
}
