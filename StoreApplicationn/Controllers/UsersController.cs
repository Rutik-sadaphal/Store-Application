
using System;
using System.Threading.Tasks;
using System.Web.Http;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;
using StoreApplicationn.Services;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUserService userService;

        public UsersController()
        {
            IUserRepository userRepository = new UserRepository();
            userService = new UserService(userRepository);
        }

        
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsers();
            return Ok(users);
        }

     
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetUserById(int id)
        {
            var user = await userService.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterUser(
            [FromBody] UserRegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await userService.RegisterUser(request);

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(
            [FromBody] UserLoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userService.Login(request);

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }

       
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> UpdateUser(
            int id, [FromBody] UserUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await userService.UpdateUser(id, request);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            bool deleted = await userService.DeleteUser(id);

            if (!deleted)
                return NotFound();

            return Ok("User deleted successfully.");
        }
    }
}
