using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Application.Dtos.UserDto;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.ModelView;

namespace MinecraftE_Commerce.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User>? _InManger;
        private readonly UserManager<User>? _userManager;
        private readonly ITokenService _tokenService;

        public UserController(SignInManager<User>? inManger, UserManager<User>? userManager, ITokenService tokenService)
        {
            _InManger = inManger;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> RegisterUser([FromForm]CreateUser userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Error in model state" + ModelState);

                var user = new User { UserName = userDto.UserName, Email = userDto.Email };

                var pfp = userDto.Pfp;
                string pathPfp = "C:\\Users\\oisyz\\source\\repos\\MinecraftE-Commerce\\MinecraftE-Commerce\\Pfps\\";
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pfp.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), pathPfp, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pfp.CopyToAsync(stream);
                }

                user.Pfp = $"{pathPfp}/{fileName}";

                var createUser = await _userManager!.CreateAsync(user, userDto.Password);

                if (createUser.Succeeded)
                {
                    var tokenGenerate = _tokenService.CreateToken(user);
                    return Ok(new TokenGenerateModelView(tokenGenerate));
                }

                else return BadRequest(createUser.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserForLogin loginDto)
        {
            var user = new User();
            user.Email = loginDto.EmailForLogin;

            var searchEmail = await _userManager!.Users.FirstOrDefaultAsync(
                x => x.Email == user.Email
                );

            if (searchEmail == null)
            {
                string message = "User not found";
                return BadRequest(new NoAuthorizedModelView(message));
            }

            var responseLogin = await _InManger!.PasswordSignInAsync(user!, loginDto.PasswordForLogin, true, false);

            if (responseLogin.Succeeded)
            {
                var token = _tokenService.CreateToken(searchEmail);
                var pfp = user.Pfp;
                return Ok(new LoginSucessModelView(pfp, token));
            }

            string not = "not authorized";
            return BadRequest(new NoAuthorizedModelView(not));
        }
    }
}
