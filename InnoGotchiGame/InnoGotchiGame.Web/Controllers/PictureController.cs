using InnoGotchiGame.Application.Filtrators;
using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api/pictures")]
    public class PictureController : BaseController
    {
        private PictureManager _manager;

        public PictureController(PictureManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Adds <paramref name="picture"/> to database
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Post([FromBody] PictureDTO picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rezult = _manager.Add(picture);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <summary>
        /// Updates the picture with a special id
        /// </summary>
        /// <param name="updatedId">Id of the picture being updated</param>
        [HttpPut("{updatedId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public IActionResult Put([FromBody] PictureDTO picture, int updatedId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = _manager.Update(updatedId, picture);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <returns>Filtered list of pictures</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PictureDTO>), 200)]
        public IEnumerable<PictureDTO> Get(PictureFiltrator filtrator)
        {
            var pictures = _manager.GetAll(filtrator);
            return pictures;
        }

        /// <returns>picture with special <paramref name="pictureId"/> </returns>
        [HttpGet("{pictureId}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetById(int pictureId)
        {
            var picture = _manager.GetById(pictureId);
            if (picture == null)
                return BadRequest(new { errorText = "Invalid id." });

            return new ObjectResult(picture);
        }
    }
}
