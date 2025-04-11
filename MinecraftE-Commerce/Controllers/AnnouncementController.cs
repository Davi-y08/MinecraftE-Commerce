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
            var announcements = await _annService.GetAllAnnouncements();
            return Ok(announcements);
        }

        [Authorize]
        [HttpPost("CreateAdd")]
        public async Task<IActionResult> CreateAnnouncement([FromForm] CreateAnnouncement createDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = createDto.ImageAnnouncement;
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);   
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "ImagesAnnouncements", fileName);

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
            annModel.ImageAnnouncement = $"ImagesAnnouncements/{fileName}";
            //await _annService.CreateAnnouncements(annModel);

            var responseCreated = await _annService.CreateAnnouncements(annModel);
            //return Ok(new TokenGenerateModelView(tokenGenerate, user.Pfp));

            if (responseCreated == null)
            {
                return BadRequest(new CreatedAd("Erro ao criar anúncio!"));
            }

            return Ok(new CreatedAd("Anúncio criado com sucesso"));
            
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
            //string? userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            //var user = await _userService.FindByNameAsync(userName!);
            //string userId = user!.Id;
            //var verifyAnnouncent = await _context.Announcements.FirstOrDefaultAsync(x => x.Id == idAnnouncement);
            //string beforeImage = verifyAnnouncent!.ImageAnnouncement;
            //var idUserInAnnouncement = verifyAnnouncent!.UserId;

            //if (userId != idUserInAnnouncement)
            //{
            //    return Forbid();
            //}

            string? userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);

            if (userName == null)
            {
                return Forbid();
            }

            var user = await _userService.FindByNameAsync(userName!);
            string userId = user!.Id;

            var verifyAnnouncement = await _context.Announcements.FirstOrDefaultAsync(a => a.Id == id);
            int idAnnouncement = verifyAnnouncement!.Id;
            string userIDInAnnouncement = verifyAnnouncement.UserId;

            if (!(userId == userIDInAnnouncement))
            {
                return Forbid("Inautorizado");
            }

            else
            {
                var announcementDel = await _annService.DeleteAnnouncement(id);

                if (announcementDel == null)
                {
                    return NotFound("Nao foi encontrado");
                }

                return Ok(announcementDel);
            }
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

        [HttpGet]
        public async Task<IActionResult> ReturnAnnouncementInRandomOrder()
        {
            var announcements = await _annService.GetAllAnnouncements();
            var array = announcements.ToArray();
            int lengthArray = array.Length;
            int[] numerosJaEscolhidos = new int[lengthArray];
            List<int> numerosDisponiveis = Enumerable.Range(0, lengthArray).ToList();

            Random rng = new Random();

            for (int i = 0; i < lengthArray; i++)
            {
                int indexAleatorio = rng.Next(numerosDisponiveis.Count);
                numerosJaEscolhidos[i] = numerosDisponiveis[indexAleatorio];
                numerosDisponiveis.RemoveAt(indexAleatorio);
            }

            for (int i = lengthArray - 1; i > 0; i--)
            {
                int j = rng.Next(0, lengthArray);
                (array[i], array[j]) = (array[j], array[i]);
            }

            var arrayToListBack = array.ToList();

            return Ok(arrayToListBack);   
        }
    }
}