using Microsoft.EntityFrameworkCore;
using Telephones.API.Data.Models;

namespace Telephones.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Record> Records { get; set; }
    }
}
