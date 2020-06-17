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
    [Route("countries")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        // GET: countries
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountries()
        {
            IList<Country> countries = await _countryRepository.GetAll();

            if (countries != null)
            {
                return Ok(countries);
            }

            return NotFound("Classes not found");
        }

        // GET: countries/id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCountry(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var city = await _countryRepository.GetById(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // POST: countries
        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] CountryDTO country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            Country newCountry = new Country()
            {
                name = country.name,
                cities = country.cities
            };

            if (_countryRepository.CheckIfExists(newCountry))
            {
                return Conflict("Country already exists");
            }

            bool created = await _countryRepository.Add(newCountry);

            if (created)
            {
                return Created("", newCountry);
            }

            return Conflict();
        }

        // PUT: countries
        [HttpPut]
        public async Task<IActionResult> UpdateCountry([FromBody] CountryDTO city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            Country updatedCountry = new Country()
            {
                id = city.id,
                name = city.name
            };

            bool updated = await _countryRepository.Update(updatedCountry);

            if (updated)
            {
                return Ok(updatedCountry);
            }

            return Conflict("Country doesn't exist");
        }

        // Add city to country
        [HttpGet("addCity/{countryId}/{CityId}")]
        public async Task<IActionResult> AddCityToCountry(int? countryId, int? cityId)
        {
            if (!countryId.HasValue || !cityId.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var added = await _countryRepository.AddCityToCountry(cityId.Value, countryId.Value);

            if (added)
                return Ok("Added city to country");

            return BadRequest("Country or city doesn't exist");
        }

        // Remove city from country
        [HttpGet("removeCity/{countryId}/{CityId}")]
        public async Task<IActionResult> RemoveCityFromCountry(int? countryId, int? cityId)
        {
            if (!countryId.HasValue || !cityId.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var added = await _countryRepository.RemoveCityFromCountry(cityId.Value, countryId.Value);

            if (added)
                return Ok("Removed city from country");

            return BadRequest("Country or city doesn't exist");
        }

        // DELETE: countires/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var deleted = await _countryRepository.Delete(id.Value);

            if (deleted)
            {
                return Ok("Country deleted");
            }

            return NotFound("Country not found");
        }

        //GET countries/lastId
        [HttpGet("lastid")]
        public async Task<IActionResult> GetLastId()
        {
            var id = _countryRepository.GetLast();
            if (id != 0)
            {
                return Ok(id);
            }

            return NotFound();
        }
    }
}
