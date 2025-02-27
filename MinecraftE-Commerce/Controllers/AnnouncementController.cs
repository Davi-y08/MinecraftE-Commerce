using System.Formats.Asn1;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infrastructure.Data;

namespace MinecraftE_Commerce.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnoucementService _annService;
        private readonly UserManager<User> _userService;
        private readonly AppDbContext _context;

        public AnnouncementController(IAnnoucementService annService, UserManager<User> userService, AppDbContext context)
        {
            _annService = annService;
            _context = context; 
            _userService = userService;
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetAnnById(int id)
        {
            if (id == null)
            return BadRequest("Id not found");

          var search =  await _annService.GetAnnouncementById(id);
          var searchDto = search.MapToDisplay();

          if (search == null) return NotFound("Announcement not found");

          

          return Ok(searchDto);
        }

        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAllAnnouncement()
        {
            var announcements = await _annService.GetAllAnnouncements();

            return Ok(announcements);
        }

        [HttpGet]

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

        [HttpGet("SearchAn")]

        public async Task<IActionResult> SearchAnnouncement(string strSearch)
        {
            if (strSearch == null) return BadRequest("The string is empty");

            var announcements = from a in _context.Announcements select a;

            if (!String.IsNullOrEmpty(strSearch))
            {
                announcements = announcements.Where(s => s.Title!.ToLower().Contains(strSearch.ToLower()));
            }

            return Ok(await announcements.ToListAsync());
        }
    }
}
