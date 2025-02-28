using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinecraftE_Commerce.Application.Dtos.UserDto;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infrastructure.Data;
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
        private readonly AppDbContext _context;

        public UserController(SignInManager<User>? inManger, UserManager<User>? userManager, ITokenService tokenService, AppDbContext context)
        {
            _InManger = inManger;
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> RegisterUser([FromForm] CreateUser userDto)
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

                var createUser = await _userManager!.CreateAsync(user, userDto.Password);

                if (createUser.Succeeded)
                {
                    var tokenGenerate = _tokenService.CreateToken(user);
                    user.Pfp = $"{pathPfp}/{fileName}";
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return Ok(new TokenGenerateModelView(tokenGenerate, user.Pfp));
                }

                else return BadRequest(createUser.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserForLogin loginDto)
        {
            var user = await _userManager!.Users.FirstOrDefaultAsync(x => x.Email == loginDto.EmailForLogin);

            if (user == null)
            {
                string message = "User not found";
                return BadRequest(new NoAuthorizedModelView(message));
            }

            var responseLogin = await _InManger!.PasswordSignInAsync(user!, loginDto.PasswordForLogin, true, false);

            if (responseLogin.Succeeded)
            {
                var token = _tokenService.CreateToken(user);
                var pfp = user.Pfp;
                return Ok(new LoginSucessModelView(pfp, token));
            }

            string not = "not authorized";
            return BadRequest(new NoAuthorizedModelView(not));
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetUserByName([FromQuery] GetUserByname userDto)
        {
            var userName = userDto.UserName;

            if (userName == null)
            {
                return BadRequest("");
            }

            var findUser = await _userManager!.FindByNameAsync(userName);

            if (findUser == null)
            {
                string userNotFound = "User not found";
                return BadRequest(new UserNotFound(userNotFound));
            }

            return Ok(findUser);
        }

        [HttpGet("GetAllUsers")]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {

            var user = await _userManager!.FindByIdAsync(id);

            try
            {
                await _userManager.DeleteAsync(user!);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}
