﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MinecraftE_Commerce.Infrastructure.Data
{
    public class Factory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseMySql("Server=localhost;User=root;Password=eltinho123;Database=bd_myminecraft"
                , ServerVersion.AutoDetect("Server=localhost;User=root;Password=eltinho123;Database=bd_myminecraft"));

            return new AppDbContext(optionBuilder.Options);
        }
    }
}
