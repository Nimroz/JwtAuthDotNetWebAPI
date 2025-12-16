using JwtAuthDotNetWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthDotNetWebAPI.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
