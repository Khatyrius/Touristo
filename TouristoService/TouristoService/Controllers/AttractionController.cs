using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TouristoService.DTO;
using TouristoService.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TouristoService.DataBase.Repositories.Interfaces;

namespace TouristoService.Controllers
{
    [Authorize]
    [Route("attractions")]
    [ApiController]
    public class AttractionController : Controller
    {
        private readonly IAttractionRepostiory _attractionRepostiory;

        public AttractionController(IAttractionRepostiory attractionRepository)
        {
            _attractionRepostiory = attractionRepository;
        }

        //GET : attractions
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttractions()
        {
            IList<Attraction> attractions = await _attractionRepostiory.GetAll();

            if (attractions.Count != 0)
            {
                return Ok(attractions);
            }

            return NotFound("Brak atrakcjii do wyświetlenia");
        }

        // GET attractions/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttraction(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var attraction = await _attractionRepostiory.GetById(id.Value);
            if (attraction != null)
            {
                return Ok(attraction);
            }

            return NotFound("Attraction not found");
        }

        // GET attractions/{id}
        [HttpGet("byCity/{cityId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttractionByCity(int? cityId)
        {
            if (!cityId.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var attraction = await _attractionRepostiory.GetAttractionsByCity(cityId.Value);
            if (attraction != null)
            {
                return Ok(attraction);
            }

            return NotFound("Attraction not found");
        }

        // GET attractions/{id}
        [HttpGet("byCity/{type}/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttractionByTypeAndCity(string type, int? cityId)
        {
            if (!cityId.HasValue || string.IsNullOrEmpty(type))
            {
                return BadRequest("Id or type must be given");
            }

            var attraction = await _attractionRepostiory.GetAttractionsByTypeAndCity(type, cityId.Value);
            if (attraction != null)
            {
                return Ok(attraction);
            }

            return NotFound("Attraction not found");
        }


        // POST attractions
        [HttpPost]
        public async Task<IActionResult> AddAttraction([FromBody] AttractionDTO attraction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            Attraction newAttraction = new Attraction
            {
                name = attraction.name,
                address = attraction.address,
                city = attraction.city,
                type = attraction.type
            };

            if (_attractionRepostiory.CheckIfExists(newAttraction))
            {
                return Conflict("Attraction already exists");
            }

            bool created = await _attractionRepostiory.Add(newAttraction);

            if (created)
            {
                return Created("", newAttraction);
            }

            return Conflict();
        }

        //DELETE attractions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttraction(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var deleted = await _attractionRepostiory.Delete(id.Value);

            if (deleted)
            {
                return Ok("Attraction deleted succesfuly");
            }

            return NotFound("Attraction doesn't exist");
        }

        //PUT attractions
        [HttpPut]
        public async Task<IActionResult> UpdateAttraction([FromBody] AttractionDTO attraction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            Attraction updateAttraction = new Attraction()
            {
                id = attraction.id,
                name = attraction.name,
                address = attraction.address,
                city = attraction.city,
                type = attraction.type
            };

            bool updated = await _attractionRepostiory.Update(updateAttraction);

            if (updated)
            {
                return Ok(updateAttraction);
            }

            return Conflict();
        }

        [HttpGet("lastid")]
        public async Task<IActionResult> GetLastId()
        {
            var id = _attractionRepostiory.GetLast();
            if (id != 0)
            {
                return Ok(id);
            }

            return NotFound();
        }
    }
}
