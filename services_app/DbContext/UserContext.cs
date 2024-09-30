using Microsoft.EntityFrameworkCore;
using services_app.Models;

namespace services_app
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) 
            : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}
