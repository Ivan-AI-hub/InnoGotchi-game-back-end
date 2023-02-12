﻿using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.SortRules;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Models.ErrorModel;
using InnoGotchiGame.Web.Models.Pets;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api/pets")]
    public class PetController : BaseController
    {
        private readonly PetManager _petManager;

        public PetController(PetManager petManager)
        {
            _petManager = petManager;
        }

        /// <summary>
        /// Creates a pet
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> PostAsync([FromBody] AddPetModel addPetModel)
        {
            var result = await _petManager.AddAsync(addPetModel.FarmId, addPetModel.Name, addPetModel.View);
            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Updates a pet
        /// </summary>
        [HttpPut]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> PutAsync([FromBody] UpdatePetModel updatePetModel)
        {
            var result = await _petManager.UpdateAsync(updatePetModel.UpdatedId, updatePetModel.Name);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Accepted();
        }

        /// <summary>
        /// Feeds a pet with a special id
        /// </summary>
        /// <param name="petId">Pet id</param>
        [HttpPut("{petId}/feed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> FeedAsync(int petId)
        {
            var userId = int.Parse(User.GetUserId()!);
            var result = await _petManager.FeedAsync(petId, userId);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Feeds a pet with a special id
        /// </summary>
        /// <param name="petId">Pet id</param>
        [HttpPut("{petId}/resetHappinessDay")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> ResetHappinessDayAsync(int petId)
        {
            var result = await _petManager.ResetHappinessDayAsync(petId);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Gives a drink to a pet with a special id
        /// </summary>
        /// <param name="petId">Pet id</param>
        [HttpPut("{petId}/drink")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> GiveDrinkAsync(int petId)
        {
            var userId = int.Parse(User.GetUserId()!);
            var result = await _petManager.GiveDrinkAsync(petId, userId);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Sets the dead status to the pet
        /// </summary>
        /// <param name="petId">Pet id</param>
        [HttpPut("{petId}/dead")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> SetDeadStatusAsync(int petId, DateTime deadDate)
        {
            var result = await _petManager.SetDeadStatusAsync(petId, deadDate);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <returns>Filtered and sorted list of pets</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetDTO>), 200)]
        public async Task<IEnumerable<PetDTO>> GetAsync(PetFiltrator filtrator, string sortField = "Age", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var pets = await _petManager.GetPetsAsync(filtrator, sorter);
            return pets;
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> pets</returns>
        [HttpGet("{pageSize}/{pageNumber}")]
        [ProducesResponseType(typeof(IEnumerable<PetDTO>), 200)]
        public async Task<IEnumerable<PetDTO>> GetPageAsync(int pageSize, int pageNumber, PetFiltrator filtrator,
            string sortField = "Age", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var pets = await _petManager.GetPetsPageAsync(pageSize, pageNumber, filtrator, sorter);
            return pets;
        }

        /// <returns>pet with special <paramref name="petId"/> </returns>
        [HttpGet("{petId}")]
        [ProducesResponseType(typeof(PetDTO), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 404)]
        public async Task<IActionResult> GetByIdAsync(int petId)
        {
            var pet = await _petManager.GetPetByIdAsync(petId);
            if (pet == null)
                return NotFound(new ErrorDetails(404, "Invalid id."));

            return new ObjectResult(pet);
        }

        private PetSorter GetSorter(string sortRule, bool isDescendingSort)
        {
            var sorter = new PetSorter();
            sorter.SortRule = Enum.Parse<PetSortRule>(sortRule);
            sorter.IsDescendingSort = isDescendingSort;
            return sorter;
        }
    }
}
