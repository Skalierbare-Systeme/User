using Microsoft.AspNetCore.Mvc;
using user.Data;
using user.Models;
using user.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace user.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest("Email and password are required.");
            }

            // Find the user by email
            var user = await dbContext.UsersPraktikum
                                      .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                // User not found
                return Unauthorized("Invalid credentials.");
            }

            // Check if the password matches
            if (user.Password != loginDto.Password)
            {
                // NOTE: This is a direct comparison. In a real-world application,
                // you MUST hash passwords for security.
                return Unauthorized("Invalid credentials.");
            }

            // Login successful, return user data (without the password)
            var userResponse = new
            {
                user.Id,
                Name = user.Name,
                user.Email
            };

            return Ok(userResponse);
        }
        public UsersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = dbContext.UsersPraktikum.ToList();
            return Ok(users);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetUsersById(Guid id)
        {
            var user = dbContext.UsersPraktikum.Find(id);
            if (user == null) 
            { 
                return NotFound(); 
            }
            return Ok(user);

        }

        [HttpPost]
        public IActionResult AddUser(UserDto addUserDto)
        {
            var userEntity =  new User() { 
                Name = addUserDto.Name,
                Email = addUserDto.Email,
                Password = addUserDto.Password
            };

            dbContext.UsersPraktikum.Add(userEntity);
            dbContext.SaveChanges();

            return Ok(userEntity);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateUser(Guid id, UserDto updateUserDto)
        {
            var userEntity = dbContext.UsersPraktikum.Find(id);
            if (userEntity == null) 
            { 
                return NotFound(); 
            }

            userEntity.Name = updateUserDto.Name;
            userEntity.Email = updateUserDto.Email;
            userEntity.Password = updateUserDto.Password;

            dbContext.SaveChanges();

            return Ok(userEntity);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteUser(Guid id)
        {
            var userEntity = dbContext.UsersPraktikum.Find(id);
            if (userEntity == null)
            {
                return NotFound();
            }

            dbContext.UsersPraktikum.Remove(userEntity);
            dbContext.SaveChanges();

            return Ok();
        }
    }
}
