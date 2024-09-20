using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class Repository(DbContextOptions<Repository> options) : DbContext(options)
    {
        public DbSet<ClientModel> Clients { get; set; }   
    }
}
