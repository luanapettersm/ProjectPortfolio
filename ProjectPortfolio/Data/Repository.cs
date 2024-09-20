using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class Repository(DbContextOptions<Repository> options) : DbContext(options)
    {
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<ClientModel> SystemUsers { get; set; }
        public DbSet<ClientModel> States { get; set; }
        public DbSet<ClientModel> Issues { get; set; }
        public DbSet<ClientModel> IssueNotes { get; set; }
    }
}
