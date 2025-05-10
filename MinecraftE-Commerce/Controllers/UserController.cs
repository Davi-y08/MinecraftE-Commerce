using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Application.Dtos.UserDto;
using MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper;
using MinecraftE_Commerce.Application.Mappers.SaleMappers;
using MinecraftE_Commerce.Application.Mappers.UserMapper;
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
        private readonly IUserService _userService;
        private readonly IAnnoucementService _annoucementService;

        public UserController(SignInManager<User>? inManger,
            UserManager<User>? userManager,
            ITokenService tokenService,
            AppDbContext context,
            IMemoryCache memCache,
            IUserService userService,
            IAnnoucementService annoucementService)
        {
            _InManger = inManger;
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _memCache = memCache;
            _userService = userService;
            _annoucementService = annoucementService;
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

                var refreshToken = GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
                await _userManager.UpdateAsync(user);

                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                });

                var pfp = user.Pfp;
                return Ok(new LoginSucessModelView(pfp, token));
            }

            string not = "not authorized";
            return BadRequest(new NoAuthorizedModelView(not));
        }

        [HttpPost("RefreshToken")]

        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token não encontrado" });
            }

            var user = await _userManager!.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            var newAccessToken = _tokenService.CreateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(5);
            await _userManager.UpdateAsync(user);

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(5),
            });

            return Ok(new { accessToken = newAccessToken });
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
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

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromQuery] string id)
        {
            var user = await _userManager!.FindByIdAsync(id);
            string retorno = "Usuário criado com sucesso!";
            try
            {
                await _userManager.DeleteAsync(user!);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(new CreatedUser(retorno));
        }

        [Authorize]
        [HttpGet("getPurchasesByUser")]
        public async Task<IActionResult> getPurchasesByUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var user = await _userManager!.FindByNameAsync(userName!);
            string userId = user!.Id;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Usuário não identificado.");

            var listPurchases = await _userService.getSalesByUser(userId!);

            if (listPurchases == null || !listPurchases.Any())
                return NotFound("Nenhuma compra encontrada para este usuário.");

            var saleDto = listPurchases.Select(s => MapDisplaySale.MapToSaleDisplay(s)).ToList();

            var idAnnnouncements = listPurchases.
                Select(s => s.AnnouncementId)
                .ToList();

            List<Announcement> listAnnouncements = new List<Announcement>();

            for (int i = 0; i < listPurchases.Count(); i++)
            {
                int idAnnouncement = listPurchases[i].AnnouncementId;
                var announcementById = await _annoucementService.GetAnnouncementById(idAnnouncement);
                listAnnouncements.Add(announcementById);
            }


            var announcementDto = listAnnouncements
            .Select(a => DisplayAnnouncement.MapToDisplay(a))
            .ToList();

            return Ok(announcementDto);
        }

        [Authorize]
        [HttpPut("Mudar email")]
        public async Task<IActionResult> changeEmail([FromForm] changeEmail emailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modelEmail = emailDto.MapToChangeEmail();
            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var user = await  _userManager!.FindByNameAsync(userName!);
            string userEmail = user?.Email!;
            string oldEmail = emailDto.oldEmail;

            if (userEmail != oldEmail)
            {
                return Forbid("Emails diferentes.");
            }

            modelEmail.Email = emailDto.newEmail;

            await _context.SaveChangesAsync();
            return Ok(modelEmail);
        }
    }
}
