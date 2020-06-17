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
    [Route("cities")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        // GET: cities
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            IList<City> cities = await _cityRepository.GetAll();

            if (cities != null)
            {
                return Ok(cities);
            }

            return NotFound("Classes not found");
        }

        // GET: cities/id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCitiy(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var city = await _cityRepository.GetById(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // GET: cities/byCountry/id
        [HttpGet("byCountry/{countryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCityCountryId(int? countryId)
        {
            if (!countryId.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var city = await _cityRepository.GetCitiesByCountryWithAttractions(countryId.Value);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // POST: cities
        [HttpPost]
        public async Task<IActionResult> AddCity([FromBody] CityDTO city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            City newCity = new City()
            {
                name = city.name,
                country = city.country,
                attractions = city.attractions
            };

            if (_cityRepository.CheckIfExists(newCity))
            {
                return Conflict("City already exists");
            }

            bool created = await _cityRepository.Add(newCity);

            if (created)
            {
                return Created("", newCity);
            }

            return Conflict();
        }

        // PUT: cities
        [HttpPut]
        public async Task<IActionResult> UpdateCity([FromBody] CityDTO city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            City updatedCity = new City()
            {
                id = city.id,
                name = city.name,
                country = city.country
            };

            bool updated = await _cityRepository.Update(updatedCity);

            if (updated)
            {
                return Ok(updatedCity);
            }

            return Conflict("City doesn't exist");
        }

        // Add attraction to class
        [HttpGet("addAttraction/{cityId}/{attractionId}")]
        public async Task<IActionResult> AddAttractionToCity(int? cityId, int? attractionId)
        {
            if (!cityId.HasValue || !attractionId.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var added = await _cityRepository.AddAttractionToCity(attractionId.Value, cityId.Value);

            if (added)
                return Ok("Added attraction to city");

            return BadRequest("Attraction or city doesn't exist");
        }

        // Remove attraction from class
        [HttpGet("removeAttraction/{cityId}/{attractionId}")]
        public async Task<IActionResult> RemoveAttractionFromCity(int? cityId, int? attractionId)
        {
            if (!cityId.HasValue || !attractionId.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var added = await _cityRepository.RemoveAttractionFromCity(attractionId.Value, cityId.Value);

            if (added)
                return Ok("Removed attraction to city");

            return BadRequest("Attraction or city doesn't exist");
        }

        // DELETE: cities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var deleted = await _cityRepository.Delete(id.Value);

            if (deleted)
            {
                return Ok("City deleted");
            }

            return NotFound("City not found");
        }

        //GET citis/lastId
        [HttpGet("lastid")]
        public async Task<IActionResult> GetLastId()
        {
            var id = _cityRepository.GetLast();
            if (id != 0)
            {
                return Ok(id);
            }

            return NotFound();
        }
    }
}
