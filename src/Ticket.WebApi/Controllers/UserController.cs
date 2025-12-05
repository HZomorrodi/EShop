using EShop.Common.Extensions;
using EShop.DataLayer.Context;
using EShop.Entities;
using EShop.Entities.WebApi;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Users.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ticket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [EnableCors("CustomCORS")]
    public class UserController(IUserService userService, IRoleService roleService, IUnitOfWork uow) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IRoleService _roleService = roleService;
        private readonly IUnitOfWork _uow = uow;

        [HttpGet("TestData")]
        //[DisableCors]
        public List<string> TestData()
        {
            return ["Payam Ahmadi, Sina Rezaei, Ali Modares"];
        }
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
        public async Task<IActionResult> Post([FromForm] AddUserViewModel model)
        {
            bool duplicate = _userService.IsExistsByUserNameForAdd(model.UserName);
            if (duplicate)
            {
                return BadRequest("نام کاربری تکراری میباشد");
            }
            User user = new()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                PassWord = model.Password.ToHash(),
            };
            if (model.Roles?.Count > 0)
            {
                List<Role> existRoles = _roleService.GetRolesBy(model.Roles);
                model.Roles.ForEach(role =>
                {
                    Role? currentRole = existRoles.SingleOrDefault(existRole => existRole.Title == role);
                    user.Roles.Add(currentRole ?? new Role { Title = role });
                });
            }
            //upload image
            string avatarName = Guid.NewGuid().ToString("N");
            string avatarExtension = Path.GetExtension(model.Avatar.FileName);
            model.Avatar.SaveImage(avatarName, avatarExtension, "avatars");
            user.Avatar = avatarName + avatarExtension;
            //
            await _userService.AddAsync(user);
            await _uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { user.Id }, new
            {
                user.Id,
                model.UserName,
                model.FullName,
                Password = model.Password.ToHash(),
                user.Avatar,
                Roles = user.Roles.Select(role => role.Title).ToList()
            });
        }

        [HttpPost("Base64")]
        public async Task<IActionResult> Post(AddUserViewModelBase64 model)
        {
            bool duplicate = _userService.IsExistsByUserNameForAdd(model.UserName);
            if (duplicate)
            {
                return BadRequest(new
                {
                    Code = 10,
                    Message = "نام کاربری تکراری می‌باشد"
                });
            }
            User user = new()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                PassWord = model.Password.ToHash(),
            };
            if (model.Roles?.Count > 0)
            {
                List<Role> existRoles = _roleService.GetRolesBy(model.Roles);
                model.Roles.ForEach(role =>
                {
                    Role? currentRole = existRoles.SingleOrDefault(existRole => existRole.Title == role);
                    user.Roles.Add(currentRole ?? new Role { Title = role });
                });
            }
            //upload image
            string avatarName = Guid.NewGuid().ToString("N");
            string avatarExtension = model.Avatar.SaveBase64Image(avatarName, "avatars");
            user.Avatar = avatarName + avatarExtension;
            //
            await _userService.AddAsync(user);
            await _uow.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { user.Id }, new
            {
                user.Id,
                model.UserName,
                model.FullName,
                Password = model.Password.ToHash(),
                user.Avatar,
                Roles = user.Roles.Select(role => role.Title).ToList()
            });
        }
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromForm] AddUserViewModel model)
        {
            User? user = await _userService.GetUserToEdit(id);
            if (user is null)
            {
                return BadRequest();
            }
            bool checkForDuplicateUserName = _userService.IsExistsByUserNameForEdit(model.UserName, id);
            if (checkForDuplicateUserName)
            {
                return BadRequest("نام کاربری تکراری میباشد");
            }

            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.PassWord = model.Password.ToHash();
            user.Roles.Clear();
            if (model.Roles?.Count > 0)
            {
                List<Role> existRoles = _roleService.GetRolesBy(model.Roles);
                model.Roles.ForEach(role =>
                {
                    Role? currentRole = existRoles.SingleOrDefault(existRole => existRole.Title == role);
                    user.Roles.Add(currentRole ?? new Role { Title = role });
                });
            }
            WorkWithImages.RemoveImage(user.Avatar, "avatars");
            var imageExtension = Path.GetExtension(model.Avatar.FileName);
            var imageName = Guid.NewGuid().ToString("N");
            model.Avatar.SaveImage(imageName, imageExtension, "avatars");
            user.Avatar = imageName + imageExtension;
            await _uow.SaveChangesAsync();
            return Ok(new
            {
                user.Id,
                model.UserName,
                model.FullName,
                Password = model.Password.ToHash(),
                user.Avatar,
                Roles = user.Roles.Select(role => role.Title).ToList()
            });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(int id, [FromForm] PatchUserViewModel model)
        {
            var user = await _userService.GetUserToEdit(id);
            if (user is null)
                return NotFound();

            // --- Update only what’s provided ---

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                bool duplicate = _userService.IsExistsByUserNameForEdit(model.UserName, id);
                if (duplicate)
                    return BadRequest("نام کاربری تکراری می‌باشد");

                user.UserName = model.UserName;
            }

            if (!string.IsNullOrWhiteSpace(model.FullName))
                user.FullName = model.FullName;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.PassWord = model.Password.ToHash();

            if (model.Roles?.Count > 0)
            {
                user.Roles.Clear();
                List<Role> existRoles = _roleService.GetRolesBy(model.Roles);
                foreach (string role in model.Roles)
                {
                    Role? existing = existRoles.SingleOrDefault(r => r.Title == role);
                    user.Roles.Add(existing ?? new Role { Title = role });
                }
            }

            if (model.Avatar is not null)
            {
                // remove old image
                WorkWithImages.RemoveImage(user.Avatar, "avatars");

                string imageName = Guid.NewGuid().ToString("N");
                string imageExt = Path.GetExtension(model.Avatar.FileName);
                model.Avatar.SaveImage(imageName, imageExt, "avatars");

                user.Avatar = imageName + imageExt;
            }

            await _uow.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.FullName,
                user.Avatar,
                Roles = user.Roles.Select(r => r.Title).ToList()
            });
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
            _userService.Remove(user);
            await _uow.SaveChangesAsync();
            return Ok();
        }

    }
}
