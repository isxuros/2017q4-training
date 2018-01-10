using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFTraining.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EFTraining.Data.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using EFTraining.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EFTraining
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer();

            var connectionString = Configuration
                .GetConnectionString("DefaultConnection");

            services.AddDbContext<DigiBookDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddIdentity<ApplicationUser, ApplicationRole>( options =>
              {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5; 
                options.Lockout.AllowedForNewUsers = true;

                // Signin settings
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // User settings
                options.User.RequireUniqueEmail = true;
              })
                .AddEntityFrameworkStores<DigiBookDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "DigBookCookie";
                options.Cookie.HttpOnly = true; 
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied"; 
                options.SlidingExpiration = true;
                // Requires `using Microsoft.AspNetCore.Authentication.Cookies;`
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });
            
            #region Authentication
            services.AddAuthentication(opts =>
                {
                    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // standard configuration
                        ValidIssuer = Configuration["Auth:Jwt:Issuer"], 
                        ValidAudience = Configuration["Auth:Jwt:Audience"], 
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"])), 
                        ClockSkew = TimeSpan.Zero,
                        // security switches
                        RequireExpirationTime = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true
                }; 
                });
            #endregion
            services.AddMvc();
         
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();

        }
    }
}
