using Microsoft.EntityFrameworkCore;
using Telephones.Data.Models;

namespace Telephones.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Record> Records { get; set; }
    }
}
