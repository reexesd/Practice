using Microsoft.EntityFrameworkCore;
using Practice.Models;

namespace Practice
{
    public class TimeManagementDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public TimeManagementDBContext(DbContextOptions<TimeManagementDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
