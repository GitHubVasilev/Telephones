﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Telephones.IdentityServer.Entities;

namespace Telephones.IdentityServer.Data
{
    /// <summary>
    /// Класс для работы с источником данных
    /// </summary>
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
