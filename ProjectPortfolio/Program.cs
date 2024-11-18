using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Services;

namespace ProjectPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<IClientService, ClientService>();
            builder.Services.AddTransient<IClientProjectService, ClientProjectService>();
            builder.Services.AddTransient<ISystemUserService, SystemUserService>();
            builder.Services.AddTransient<IIssueService, IssueService>();
            builder.Services.AddTransient<IIssueNoteService, IssueNoteService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<ISystemUserRepository, SystemUserRepository>();
            builder.Services.AddTransient<IClientRepository, ClientRepository>();
            builder.Services.AddTransient<IIssueRepository, IssueRepository>();
            builder.Services.AddTransient<IIssueNoteRepository, IssueNoteRepository>();

            builder.Services.AddDbContextFactory<Repository>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
