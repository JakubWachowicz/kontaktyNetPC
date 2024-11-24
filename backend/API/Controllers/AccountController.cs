using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        //Register user with provided details
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await accountService.RegisterUser(registerUserDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Description);
            }
            return Ok();
        }
        //Login user with provided credentials
        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {

            var (result, token) = await accountService.GenerateJwt(loginUserDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.Description);
            }
            return Ok(token);
        }

    }
}
