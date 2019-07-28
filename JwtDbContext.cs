using JWTAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth
{
    public class JwtDbContext : DbContext
    {
        public JwtDbContext(DbContextOptions<JwtDbContext> options) : base(options)
        {    
        }
        
        public DbSet<User> Users {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}