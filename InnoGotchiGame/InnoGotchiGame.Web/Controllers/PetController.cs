using AutoMapper;
using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Application.Sorters;
using InnoGotchiGame.Application.Sorters.SortRules;
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

        /// <summary>
        /// Creates a pet
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Post([FromBody] AddPetModel addPetModel)
        {
            var rezult = _petManager.Add(addPetModel.FarmId, addPetModel.Name, addPetModel.View);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// Updates a pet
        /// </summary>
        [HttpPut]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Put([FromBody] UpdatePetModel updatePetModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _petManager.Update(updatePetModel.UpdatedId, updatePetModel.Name);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Accepted();
        }

        /// <summary>
        /// Feeds a pet with a special id
        /// </summary>
        /// <param name="petId">Pet id</param>
        /// <param name="feederId">id of the user who initiated the feeding</param>
        [HttpPut("{petId}/feed")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// Gives a drink to a pet with a special id
        /// </summary>
        /// <param name="petId">Pet id</param>
        /// <param name="drinkerId">id of the user who initiated the drinking</param>
        [HttpPut("{petId}/drink")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// Sets the dead status to the pet
        /// </summary>
        /// <param name="petId">Pet id</param>
        [HttpPut("{petId}/dead")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <returns>Filtered and sorted list of pets</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PetDTO>), 200)]
        public IEnumerable<PetDTO> Get(PetFiltrator filtrator, string sortField = "Age", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var pets = _petManager.GetPets(filtrator, sorter);
            return pets;
        }

        /// <returns>A filtered and sorted page containing <paramref name="pageSize"/> pets</returns>
        [HttpGet("{pageSize}/{pageNumber}")]
        [ProducesResponseType(typeof(IEnumerable<PetDTO>), 200)]
        public IEnumerable<PetDTO> GetPage(int pageSize, int pageNumber, PetFiltrator filtrator,
            string sortField = "Age", bool isDescendingSort = false)
        {
            var sorter = GetSorter(sortField, isDescendingSort);
            var pets = _petManager.GetPetsPage(pageSize, pageNumber, filtrator, sorter);
            return pets;
        }

        /// <returns>pet with special <paramref name="petId"/> </returns>
        [HttpGet("{petId}")]
        [ProducesResponseType(typeof(PetDTO), 200)]
        public IActionResult GetById(int petId)
        {
            var pet = _petManager.GetPetById(petId);
            if (pet == null)
                return BadRequest(new { errorText = "Invalid id." });

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
