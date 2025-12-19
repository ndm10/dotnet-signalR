using Microsoft.EntityFrameworkCore;
using signalR.Entity;

namespace signalR.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public DbSet<User> Users{ get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
    }
}
