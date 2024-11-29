using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class Repository(DbContextOptions<Repository> options) : DbContext(options)
    {
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<SystemUserModel> SystemUsers { get; set; }
        public DbSet<IssueModel> Issues { get; set; }
        public DbSet<ClientProjectModel> ClientProjects { get; set; }
    }
}
