using System.Formats.Asn1;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MinecraftE_Commerce.Application.Dtos.AnnouncementDto;
using MinecraftE_Commerce.Application.Mappers.AnnnouncementMapper;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infra.Services;
using MinecraftE_Commerce.Infrastructure.Data;
using MinecraftE_Commerce.ModelView;

namespace MinecraftE_Commerce.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IMemoryCache _memCache;
        private readonly IAnnoucementService _annService;
        private readonly UserManager<User> _userService;
        private readonly AppDbContext _context;
        private readonly IMailService _mailSender;

        public AnnouncementController(IAnnoucementService annService, UserManager<User> userService, AppDbContext context, IMailService mailSender, IMemoryCache memCache)
        {
            _annService = annService;
            _context = context; 
            _userService = userService;
            _memCache = memCache;
            _mailSender = mailSender;
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetAnnById(int id)
        {
            //if (id == null)
            //return BadRequest("Id not found");

            //var search =  await _annService.GetAnnouncementById(id);
            //var searchDto = search.MapToDisplay();

            //if (search == null) return NotFound("Announcement not found");

            //return Ok(searchDto);

            var announcement = await _annService.GetAnnouncementById(id);

            if (!_memCache.TryGetValue("announcement", out announcement))
            {
                announcement = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == id);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(365));

                _memCache.Set("announcement", cacheEntryOptions);
            }

            return Ok(announcement);
        }

        [HttpGet("GetAll")]

        public async Task<IActionResult> GetAllAnnouncement()
        {
            List<Announcement> announcements = null!;
           
            if (!_memCache.TryGetValue("all_announcements", out announcements))
            {
                 announcements = await _context.Announcements.ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(365));

                _memCache.Set("all_announcements", announcements, cacheEntryOptions);
            }

            return Ok(announcements);
        }

        [Authorize]
        [HttpPost("CreateAdd")]
        public async Task<IActionResult> CreateAnnouncement([FromForm] CreateAnnouncement createDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

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

            return Ok();
            
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

        [HttpDelete]

        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            var announcementDel = await _annService.DeleteAnnouncement(id);

            if (announcementDel == null)
            {
                return NotFound("Nao foi encontrado");
            }

            return Ok(announcementDel);
        }

        [HttpPut]
        public async Task<IActionResult> EditAnnouncement([FromForm] EditAnnouncement annDto, int idAnnouncement)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string? userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var user = await _userService.FindByNameAsync(userName!);
            string userId = user!.Id;
            var verifyAnnouncent = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == idAnnouncement);
            string beforeImage = verifyAnnouncent!.ImageAnnouncement;
            var idUserInAnnouncement = verifyAnnouncent!.UserId;

            if (userId != idUserInAnnouncement)
            {
                return Forbid();
            }

            var modelAnn = annDto.MapToEditAnnouncement();
            modelAnn.Title = annDto.Title;
            modelAnn.Descripton = annDto.Description;
            modelAnn.PriceService = annDto.PriceService;

            if (annDto.ImageAnnouncement != null)
            {
                var newImage = annDto.ImageAnnouncement;
                string pathImageTo = "C:\\Users\\oisyz\\source\\repos\\MinecraftE-Commerce\\MinecraftE-Commerce\\ImagesAnnouncements\\";
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage!.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), pathImageTo, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                modelAnn.ImageAnnouncement = Path.Combine("ImagesAnnouncements", fileName);
            }

            else
            {
                modelAnn.ImageAnnouncement = verifyAnnouncent.ImageAnnouncement;
            }

            await _annService.EditAnnouncemenet(modelAnn, idAnnouncement);

            return Ok();
        }
    }
}