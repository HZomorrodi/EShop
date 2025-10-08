using EShop.DataLayer.Context;
using EShop.Services.Contracts.Identity.WebApi;
using EShop.ViewModels.Account;
using EShop.ViewModels.Users.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ticket.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController(IUserService userService, ITokenService tokenService, IConfiguration configuration) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IConfiguration _configuration = configuration;
        /// <summary>
        /// Login action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Everything is OK and you get the JWT token</response>
        /// <response code="400">Check the model state OR ```UserName``` OR ```Password``` is incorrect</response>
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            UserToBuildJwtTokenViewModel? user = await _userService.GetUserBy(model.UserName, model.Password);
            if (user is not null)
            {
                string generatedToken = _tokenService.BuildToken(_configuration["JWT:Key"], _configuration["JWT:Issuer"], user, model.RememberMe);
                if (generatedToken is null)
                {
                    return BadRequest("user is null");
                }
                else
                {
                    return Ok(generatedToken);
                }
            }
            return BadRequest("Invalid credentials");  // Custom error message
        }

        [HttpGet("Admin1")]
        [Authorize()]
        public IActionResult Admin1()
        {
            return Ok();
        }

        [HttpGet("Admin2")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin2()
        {
            return Ok();
        }

        [HttpGet("Admin3")]
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult Admin3()
        {
            IEnumerable<System.Security.Claims.Claim> claims = User.Claims;
            return Ok();
        }
        [HttpGet("Admin4")]
        public IActionResult Admin4()
        {
            IEnumerable<System.Security.Claims.Claim> claims = User.Claims;
            return Ok();
        }
    }
}
