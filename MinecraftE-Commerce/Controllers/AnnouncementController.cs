using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnoucementService _annService;
        private readonly UserManager<User> _userService;

        public AnnouncementController(IAnnoucementService annService, UserManager<User> userService)
        {
            _annService = annService;
            _userService = userService;
        }

        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAllAnnouncement()
        {
            var announcements = await _annService.GetAllAnnouncements();

            return Ok(announcements);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement([FromForm] CreateAnnouncement createDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!User.Identity!.IsAuthenticated) return Unauthorized("user dont auth");

            var image = createDto.ImageAnnouncement;
            string pathImageAnn = "C:\\Users\\oisyz\\source\\repos\\MinecraftE-Commerce\\MinecraftE-Commerce\\ImagesAnnouncements\\";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);   
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), pathImageAnn, fileName);

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

             string? username = User.FindFirstValue(JwtRegisteredClaimNames.Name);

            if(username == null) return NotFound("User not found");

            var user = await _userService.FindByNameAsync(username);

            if (user == null) return NotFound("Login not found");

            string userPfp = user.Pfp;

            if (userPfp == null)
            {
                return NotFound("pfp not found");
            }

            var annModel = createDto.MapToCreateAnnouncement();
            annModel.UserId = user.Id.ToString();

            if(user.Id == null)
            {
                return NotFound("UserId not found");
            }

            annModel.UserName = username;
            annModel.UserPfp = userPfp;
            annModel.ImageAnnouncement = $"{pathImageAnn}/{fileName}";
     
            await _annService.CreateAnnouncements(annModel);

            return Ok("Criado com sucesso");
            
        }
    }
}
