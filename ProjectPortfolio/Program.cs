using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectPortfolio.Data;
using ProjectPortfolio.Services;
using System.Text;

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
            builder.Services.AddTransient<ITokenService, TokenService>();
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project Portfolio - NP Interiores", Version = "v1" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Name = "Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securitySchema, new string[] { } }
                });
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:JwtSettings:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("JwtToken"))
                        {
                            context.Token = context.Request.Cookies["JwtToken"];
                        }
                        return Task.CompletedTask;
                    }
                };
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
