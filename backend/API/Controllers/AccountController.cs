using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            accountService.RegisterUser(registerUserDto);
            return Ok();
        }
        //Login user with provided credentials
        [HttpPost("login")]
        public ActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {

            string token = accountService.GenerateJwt(loginUserDto);
            return Ok(token);
        }

    }
}
