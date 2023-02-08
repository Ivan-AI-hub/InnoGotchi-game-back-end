﻿using InnoGotchiGame.Application.Filtrators;
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
        public async Task<IActionResult> PostAsync([FromBody] PictureDTO picture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rezult = await _manager.AddAsync(picture);
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
        public async Task<IActionResult> PutAsync([FromBody] PictureDTO picture, int updatedId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var rezult = await _manager.UpdateAsync(updatedId, picture);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        /// <returns>Filtered list of pictures</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PictureDTO>), 200)]
        public async Task<IEnumerable<PictureDTO>> GetAsync(PictureFiltrator filtrator)
        {
            var pictures = await _manager.GetAllAsync(filtrator);
            return pictures;
        }

        /// <returns>picture with special <paramref name="pictureId"/> </returns>
        [HttpGet("{pictureId}")]
        [ProducesResponseType(typeof(UserDTO), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetByIdAsync(int pictureId)
        {
            var picture = await _manager.GetByIdAsync(pictureId);
            if (picture == null)
                return BadRequest(new { errorText = "Invalid id." });

            return new ObjectResult(picture);
        }
    }
}
