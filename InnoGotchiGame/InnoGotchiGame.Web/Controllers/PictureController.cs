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

        [HttpPost]
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

        [HttpPut("{updatedId}")]
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

        [HttpGet]
        public IEnumerable<PictureDTO> Get(PictureFiltrator filtrator)
        {
            var pictures = _manager.GetAll(filtrator);
            return pictures;
        }

        [HttpGet("{pictureId}")]
        public IActionResult GetById(int pictureId)
        {
            var picture = _manager.GetById(pictureId);
            if (picture == null)
                return BadRequest(new { errorText = "Invalid id." });

            return new ObjectResult(picture);
        }
    }
}
