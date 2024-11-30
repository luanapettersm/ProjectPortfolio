using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProjectPortfolio.Data;
using ProjectPortfolio.Services;

namespace ProjectPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<IClientService, ClientService>();
            builder.Services.AddTransient<IClientProjectService, ClientProjectService>();
            builder.Services.AddTransient<ISystemUserService, SystemUserService>();
            builder.Services.AddTransient<IIssueService, IssueService>();
            builder.Services.AddTransient<ISystemUserRepository, SystemUserRepository>();
            builder.Services.AddTransient<IClientRepository, ClientRepository>();
            builder.Services.AddTransient<IIssueRepository, IssueRepository>();
            builder.Services.AddTransient<IClientProjectRepository, ClientProjectRepository>();

            builder.Services.AddDbContextFactory<Repository>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
