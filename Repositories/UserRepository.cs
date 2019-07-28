using System.Linq;
using JWTAuth.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTAuth.Repositories
{
    public class UserRepository
    {
        private readonly JwtDbContext _dbContext;
        public UserRepository(JwtDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public void Create()
        {
            _dbContext.SaveChanges();
        }
        public User Find(int userId)
        {
            return _dbContext.Users.FirstOrDefault(x => x.UserID == userId);
        }
    }
}