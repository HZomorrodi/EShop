using EShop.DataLayer.Context;
using EShop.Entities.WebApi;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ticket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IRoleService roleService, IUnitOfWork uow) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IRoleService _roleService = roleService;
        private readonly IUnitOfWork _uow = uow;

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<User> users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            User user = await _userService.FindByIdAsync(id);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUserViewModel model)
        {
            bool checkForDuplicateUserName = _userService.IsExistsByUserNameForAdd(model.UserName);
            if (checkForDuplicateUserName)
            {
                return BadRequest("نام کاربری تکراری میباشد");
            }
            User user = new()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                PassWord = model.Password,
            };
            List<Role> existRoles = _roleService.GetRolesBy(model.Roles).ToList();
            model.Roles.ForEach(role =>
            {
                Role? currentRole = existRoles.SingleOrDefault(existRole => existRole.Title == role);
                user.Roles.Add(currentRole ?? new Role { Title = role });
            });
            await _userService.AddAsync(user);
            await _uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { user.Id }, model);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] AddUserViewModel model)
        {
            User user = await _userService.FindByIdAsync(id);
            if (user is null)
            {
                return BadRequest();
            }
            bool checkForDuplicateUserName = _userService.IsExistsByUserNameForAdd(model.UserName);
            if (checkForDuplicateUserName)
            {
                return BadRequest("نام کاربری تکراری میباشد");
            }

            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.PassWord = model.Password;
            user.Roles.Clear();
            List<Role> existRoles = _roleService.GetRolesBy(model.Roles).ToList();
            model.Roles.ForEach(role =>
            {
                Role? currentRole = existRoles.SingleOrDefault(existRole => existRole.Title == role);
                user.Roles.Add(currentRole ?? new Role { Title = role });
            });
            _userService.Update(user);
            //await _uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { user.Id }, model);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await _userService.FindByIdAsync(id);
            if (user is null)
            {
                return BadRequest();
            }
            _userService.Remove(id);
            await _uow.SaveChangesAsync();
            return Ok();
        }
    }
}
