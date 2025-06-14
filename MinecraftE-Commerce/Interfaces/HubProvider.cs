﻿using MinecraftE_Commerce.Domain.Models;

namespace MinecraftE_Commerce.Interfaces
{
    public interface IHubClient
    {
        Task SendMessage(int chatId, string messageText);
        Task ReceiveMessage(object message);
    }
}
