using Microsoft.AspNetCore.Mvc;
using user.Data;
using user.Models;
using user.Models.Entities;

namespace user.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

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
