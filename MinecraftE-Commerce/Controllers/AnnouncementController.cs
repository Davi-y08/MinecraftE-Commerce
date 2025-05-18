using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
            string cacheKey = $"announcement_{id}";

            if (!_memCache.TryGetValue(cacheKey, out announcement))
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

            var announcementsDto = announcements.Select(a => a.MapToDisplay());

            return Ok(announcementsDto);
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
        [HttpGet("GetInRandomOrder")]
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

            var arrayToListBack = array.Select(a => a.MapToDisplay()).ToList();
            return Ok(arrayToListBack);
        }

        [HttpGet("cliquesem30dias")]
        public async Task<IActionResult> GetClicksAnnouncementsIn30Days()
        {
            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var user = await _userService.FindByNameAsync(userName!);

            if (user == null)
            {
                return Unauthorized();
            }

            string userId = user.Id!;

            int clicks = await _annService.ClicksInMounth(userId);

            return Ok(clicks);
        }

        [Authorize]
        [HttpGet("MeusAnuncios")]

        public async Task<IActionResult> MyAnnouncements() 
        {
            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var user = await _userService.FindByNameAsync(userName!);
            string userId =  user!.Id;

            if (user == null) 
            {
                return Unauthorized("Usuario nao encontrado");
            }

            List<Announcement> announcements = await _annService.MyAnnouncement(userId);

            if (announcements == null) {
                return BadRequest("Nenhum anuncio encontrado");
            }

            var announcementDto = announcements
            .Select(a => DisplayAnnouncement.MapToDisplay(a))
            .ToList();

            return Ok(announcementDto);
        }

        [Authorize]
        [HttpPost("CreateAdd")]
        public async Task<IActionResult> CreateAnnouncement([FromForm] CreateAnnouncement createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imagePaths = new List<ImagesAnnouncement>();

            foreach (var image in createDto.ImagesAnnouncements)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var relativePath = Path.Combine("ImagesAnnouncements", fileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imagePaths.Add(new ImagesAnnouncement { ImagePath = relativePath });
            }

            string? username = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            if (username == null) return NotFound("User not found");

            var user = await _userService.FindByNameAsync(username);
            if (user == null) return NotFound("Login not found");

            string userPfp = user.Pfp;
            if (userPfp == null)
            {
                return NotFound("pfp not found");
            }

            var annModel = createDto.MapToCreateAnnouncement();
            annModel.UserId = user.Id.ToString();

            if (user.Id == null)
            {
                return NotFound("UserId not found");
            }

            annModel.UserName = username;
            annModel.UserPfp = userPfp;
            annModel.Images = imagePaths;

            var responseCreated = await _annService.CreateAnnouncements(annModel);

            if (responseCreated == null)
            {
                return BadRequest(new CreatedAd("Erro ao criar anúncio!"));
            }

            return Ok(new CreatedAd("Anúncio criado com sucesso"));
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnnouncement([FromRoute] int id)
        {
            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name); 
            if (userName == null) return NotFound();
            var user = _userService.FindByNameAsync(userName);

            if(user == null)
                return NotFound("Usuario nao encontrado");

            string userId = user.Id.ToString();

            var verifyAnnouncement = await _context.Announcements.FirstOrDefaultAsync(a => a.Id == id);
            int idAnnouncement = verifyAnnouncement!.Id;
            string userIDInAnnouncement = verifyAnnouncement.UserId;

            if (!(userId == userIDInAnnouncement))
                return Forbid("Inautorizado");

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

        [HttpPut("{id:int}")]
        public async Task<IActionResult> AddClickForAnnouncement([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var announcement = await _annService.GetAnnouncementById(id);

            if (announcement == null)
            {
                return NotFound("Anúncio não encontrado");
            }


            string? userName = ".";
            string idUser = ".";
            string idUserInAnnoucement = announcement.UserId;

            if (Request.Headers.ContainsKey("Authorization"))
            {
                userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
                if (userName == null) return NotFound();
                var user = _userService.FindByNameAsync(userName);

                idUser = user!.Id.ToString();

            }

            if (!(idUser == idUserInAnnoucement))
            {
                var newClick = new Clicks
                {
                    AnnouncementId = announcement.Id,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Clickss.AddAsync(newClick);
                await _context.SaveChangesAsync();

                return Ok("clique adicionado");
            }

            return BadRequest("Não foi possível adicionar o clique");
        }

        [HttpPut]
        public async Task<IActionResult> EditAnnouncement([FromForm] EditAnnouncement annDto, int idAnnouncement)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            if (userName == null) return NotFound();
            var user = _userService.FindByNameAsync(userName);

            if (user == null)
                return NotFound("Usuario nao encontrado");
            var announcement = await _context.Announcements
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == idAnnouncement);

            if (announcement == null)
                return NotFound("Anúncio não encontrado.");

            if (announcement.UserId != user.Id.ToString())
                return Forbid("Você não tem permissão para editar este anúncio.");

            announcement.Title = annDto.Title;
            announcement.Descripton = annDto.Description;
            announcement.PriceService = annDto.PriceService;

            if (annDto.Images != null && annDto.Images.Any())
            {
                foreach (var oldImage in announcement.Images)
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), oldImage.ImagePath);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                _context.ImagesAnnouncements.RemoveRange(announcement.Images);

                var newImages = new List<ImagesAnnouncement>();
                foreach (var image in annDto.Images)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var relativePath = Path.Combine("ImagesAnnouncements", fileName);
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    newImages.Add(new ImagesAnnouncement { ImagePath = relativePath });
                }

                announcement.Images = newImages;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}