using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class Repository(DbContextOptions<Repository> options) : DbContext(options)
    {
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<SystemUserModel> SystemUsers { get; set; }
        public DbSet<StateModel> States { get; set; }
        public DbSet<IssueModel> Issues { get; set; }
        public DbSet<IssueNoteModel> IssueNotes { get; set; }
    }
}
