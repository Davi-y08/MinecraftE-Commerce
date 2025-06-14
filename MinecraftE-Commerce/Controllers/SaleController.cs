﻿    using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinecraftE_Commerce.Application.Dtos.SaleDto;
using MinecraftE_Commerce.Domain.Interfaces;
using MinecraftE_Commerce.Domain.Models;
using MinecraftE_Commerce.Infrastructure.Migrations;

namespace MinecraftE_Commerce.Controllers
{
    [ApiController]
    [Route("api/v2")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        private readonly IAnnoucementService _annoucementService;
        private readonly UserManager<User> _userService;
        private readonly ChatService _chatService;
        public SaleController(ISaleService saleService, IAnnoucementService announcementService, UserManager<User> userService, ChatService chatService)
        {
            _saleService = saleService;
            _annoucementService = announcementService;
            _userService = userService;
            _chatService = chatService;
        }

        [Authorize]
        [HttpPost("CriarVenda")]
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

            if (idUserBuyer == idInAnnouncement)
            {
                return Unauthorized("Voce é criador do anuncio, nao pode compra-lo");
            }

            var model = new Sale
            {
                ReceiverId = idInAnnouncement,
                BuyerId = idUserBuyer!,
                AnnouncementId = idAnnouncement,
                SaledOn = DateTime.Now,
                AnnouncementPrice = valueAnnouncement,
            };

            await _annoucementService.ReadAndAddValueForSales(idAnnouncement);

            var savedSale = await _saleService.CreateSale(model); 

            var chat = new Chat
            {
                SaleId = savedSale.Id,
                BuyerId = savedSale.BuyerId,
                ReceiverId = savedSale.ReceiverId,
                MyProperty = new List<Message>()
            };

            await _chatService.CreateChat(chat);

            return Ok(new
            {
                message = "Compra efetuada com sucesso",
                chatId = chat.Id
            });
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

        [Authorize]
[HttpPost("GetOrCreateChat")]
public async Task<IActionResult> GetOrCreateChat([FromBody] int idAnnouncement)
{
    var userName = User.FindFirstValue(JwtRegisteredClaimNames.Name);
    if (userName == null)
        return Unauthorized();

    var buyer = await _userService.FindByNameAsync(userName);
    if (buyer == null)
        return Unauthorized("Usuário não encontrado.");

    var announcement = await _annoucementService.GetAnnouncementById(idAnnouncement);
    if (announcement == null)
        return NotFound("Anúncio não encontrado.");

    if (announcement.UserId == buyer.Id)
        return BadRequest("Você é o dono do anúncio.");

    // Verifica se já existe um chat entre comprador e vendedor para esse anúncio
    var existingChat = await _chatService.GetChatByParticipantsAsync(buyer.Id, announcement.UserId, idAnnouncement);
    if (existingChat != null)
    {
        return Ok(new { chatId = existingChat.Id });
    }

    // Cria novo chat
    var chat = new Chat
    {
        BuyerId = buyer.Id,
        ReceiverId = announcement.UserId,
        SaleId = null,
        AnnouncementId = idAnnouncement,
        MyProperty = new List<Message>()
    };

    await _chatService.CreateChat(chat);

    return Ok(new { chatId = chat.Id });
}
    }
}
