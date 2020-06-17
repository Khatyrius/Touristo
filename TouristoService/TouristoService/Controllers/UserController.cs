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
    [Route("users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //GET : users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            IList<User> users = await _userRepository.GetAll();

            if (users.Count != 0)
            {
                return Ok(users);
            }

            return NotFound("Brak użytkowników do wyświetlenia");
        }

        // GET users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            var user = await _userRepository.GetById(id.Value);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound("User not found");
        }

        // POST /users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            User newUser = new User()
            {
                id = user.id,
                username = user.username,
                password = user.password,
                role = user.role
            };

            if (_userRepository.CheckIfExists(newUser))
            {
                return Conflict("User already exists");
            }

            bool created = await _userRepository.Add(newUser);

            if (created)
            {
                return Created("", newUser);
            }

            return Conflict();
        }

        //Post /login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> ValidateUser([FromBody] UserDTO userAuthentication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            User user = await _userRepository.GetByUsername(userAuthentication.username);

            if (!user.role.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest("User not allowed");
            }

            if (await _userRepository.Validate(userAuthentication.username, userAuthentication.password))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("VERY SECRET, MUCH HIDDEN");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userAuthentication.username, userAuthentication.role)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenValue = tokenHandler.WriteToken(token);

                return Ok(tokenValue);
            }

            return BadRequest("Wrong password or username");
        }


        // PUT /users
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            User oldUser = await _userRepository.GetById(user.id);

            if (user.password != null) oldUser.password = user.password;

            if (!user.role.Equals(oldUser.role)) oldUser.role = user.role;

            bool updated = await _userRepository.Update(oldUser);

            if (updated)
            {
                return Ok(oldUser);
            }

            return Conflict();
        }


        // DELETE users/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Id must be given");
            }

            bool deleted = await _userRepository.Delete(id.Value);

            if (deleted)
            {
                return Ok("User deleted");
            }

            return NotFound("User not found");
        }

        //Get users/lastid
        [HttpGet("lastid")]
        public async Task<IActionResult> GetLastId()
        {
            var id =  _userRepository.GetLast();
            if (id != 0)
            {
                return Ok(id);
            }

            return NotFound();
        }
    }
}
