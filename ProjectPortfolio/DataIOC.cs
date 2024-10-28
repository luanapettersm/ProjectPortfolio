using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Services;

namespace ProjectPortfolio
{
    public static class DataIOC
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            services.AddDbContextFactory<Repository>(e => e.UseSqlServer(configuration.GetConnectionString("DefaultConnection", config), ServiceLifetime.Transient));

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<ISystemUserService, SystemUserService>();
            services.AddTransient<IIssueService, IssueService>();
            services.AddTransient<IIssueNoteService, IssueNoteService>();
            services.AddTransient<IIssueNoteRepository, IssueNoteRepository>();
            services.AddTransient<IIssueRepository, IssueRepository>();
        }
    }
}
