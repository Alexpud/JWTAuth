using System;
using System.Text;
using JWTAuth.Entities;
using JWTAuth.Helpers;
using JWTAuth.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private UserRepository _userRepository;
        
        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser([FromRoute]int userId)
        {
            var user = _userRepository.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            UserDTO dto = new UserDTO();
            dto.ID = user.UserID;
            return Ok(dto);
        }

        [HttpPost("")]
        public IActionResult Create([FromBody]UserDTO dto)
        {
            byte[] salt = Security.GenerateSalt(256);
            var passwordBytes = Encoding.ASCII.GetBytes(dto.Password);
            var hashedPassword = Security.GenerateHash(passwordBytes, salt, 1, 256);
            
            User user = new User();
            user.Password = Convert.ToBase64String(hashedPassword);
            user.SALT = Convert.ToBase64String(salt);
            _userRepository.Create(user);

            dto.ID = user.UserID;
            return CreatedAtAction(nameof(GetUser), new { userID = user.UserID }, dto);
        }
    }
}