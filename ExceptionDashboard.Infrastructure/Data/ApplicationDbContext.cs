using ExceptionDashboard.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExceptionDashboard.Infrastructure.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Applications>ApplicationDb{get; set;}
        public DbSet<Exceptions> ExceptionDb { get; set; }
        public DbSet<ExceptionHeader> ExceptionHeaderDb { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {
            
        }
    }
}
