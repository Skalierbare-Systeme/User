using Microsoft.EntityFrameworkCore;
using user.Models.Entities;

namespace user.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
    }
}
