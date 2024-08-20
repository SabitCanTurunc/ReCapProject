using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public ActionResult Login([FromForm] UserForLoginDto user)
        {
            var userToLogin = _authService.Login(user);
            if (!userToLogin.IsSuccess)
            {
                return BadRequest(userToLogin.Message);
            }
            var token = _authService.CreateAccessToken(userToLogin.Data);
            if (!token.IsSuccess)
            {
                return BadRequest(token.Message);
            }
            return Ok(token.Data);
        }

        [HttpPost("register")]
        public ActionResult Register([FromForm] UserForRegisterDto user)
        {
            var userExists= _authService.UserExists(user.Email);
            if (!userExists.IsSuccess) 
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _authService.Register(user);
            var token= _authService.CreateAccessToken(registerResult.Data);
            if (token.IsSuccess)
            {
                return Ok(token.Data);
            }
            return BadRequest(token.Message);


        }

    }
}
