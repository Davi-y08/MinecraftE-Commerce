using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memCache;
        private readonly UserManager<User>? _userManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;

        public UserController(SignInManager<User>? inManger, UserManager<User>? userManager, ITokenService tokenService, AppDbContext context, IMemoryCache memCache)
        {
            _InManger = inManger;
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _memCache = memCache;
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
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(pfp.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Pfps", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pfp.CopyToAsync(stream);
                }

                var createUser = await _userManager!.CreateAsync(user, userDto.Password);

                if (createUser.Succeeded)
                {
                    var tokenGenerate = _tokenService.CreateToken(user);
                    user.Pfp = $"Pfps/{fileName}";
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
        public async Task<IActionResult> Login([FromBody] UserForLogin loginDto)
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
            List<User> users = new List<User>();


            if (!_memCache.TryGetValue("users", out users!))
            {                   
                users = await _context.Users.ToListAsync();

                var memCacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(365));

                _memCache.Set("users", users);
            }

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
