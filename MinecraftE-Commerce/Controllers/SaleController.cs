    using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinecraftE_Commerce.Application.Dtos.SaleDto;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Controllers
{
    [ApiController]
    [Route("api/v2")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IAnnoucementService _annoucementService;
        private readonly UserManager<User> _userService;
        public SaleController(ISaleService saleService, IAnnoucementService announcementService, UserManager<User> userService)
        {
            _saleService = saleService;
            _annoucementService = announcementService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost]

        public async Task<IActionResult> CreateSale([FromBody] int idAnnouncement)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcement = await _annoucementService.GetAnnouncementById(idAnnouncement);

            if (announcement == null)
            {
                return BadRequest("Não foi póssivel efetuar a compra");
            }

            string idInAnnouncement = announcement.UserId;
            string? username = User.FindFirstValue(JwtRegisteredClaimNames.Name);

            if (username == null)
                return Forbid();

            var user = await _userService.FindByNameAsync(username!);

            if (user == null)
                return NotFound();

            string idUserBuyer = user.Id;

            decimal valueAnnouncement = announcement.PriceService;

            var model = new Sale
            {
                ReceiverId = idInAnnouncement,
                BuyerId = idUserBuyer!,
                AnnouncementId = idAnnouncement,
                SaledOn = DateTime.Now,
                AnnouncementPrice = valueAnnouncement,
            };

            await _annoucementService.ReadAndAddValueForSales(idAnnouncement);

            await _saleService.CreateSale(model);

            return Ok("Compra efetuada com sucesso");
        }

        [HttpGet("GetAllSales")]

        public async Task<IActionResult> GetAllSales()
        {
            var sales = await _saleService.GetAllSales();

            var dto = sales.Select(s => new DisplaySaleDto
            {
                idSale = s.Id,
                announcementId = s.AnnouncementId,
                receiverId = s.ReceiverId,
                buyerId = s.BuyerId,
                saledOn = s.SaledOn,
                valueAnnouncement = s.AnnouncementPrice
            }).ToList();

            return Ok(dto);
        }

        [Authorize]
        [HttpGet("IsBought")]

        public async Task<IActionResult> IsBought([FromQuery] int idAnnouncement)
        {
            var sales = await _saleService.IsBought(idAnnouncement);

            if (sales == null)
            {
                return NotFound("Anuncio ou venda nao encontrada");
            }

            string? userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
            var user = await _userService.FindByNameAsync(userName!);

            if (user == null)
            {
                return NotFound("Usuario nao encontrado");
            }

            string userId = user!.Id;

            bool wasBoughtByUser = sales.Any(s => s.BuyerId == userId);

            if (!wasBoughtByUser)
            {
                return BadRequest();
            }

            return Ok(wasBoughtByUser);

        }
    }
}
