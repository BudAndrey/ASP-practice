using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Config;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<BicycleContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer=true,
                        ValidIssuer=AuthOptions.ISSUER,
                        ValidateAudience=true,
                        ValidAudience=AuthOptions.AUDIENCE,
                        ValidateLifetime=true,
                        IssuerSigningKey=AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey=true,
                        ClockSkew=TimeSpan.FromSeconds(0)
                    };
                });

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders(); 
            services.Configure<IdentityOptions>(options =>
            { 
               
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                
            }) ;
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/Login";

                options.LoginPath = "/Account/Login";

            });
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath=new PathString("/Account/Login");
            //        options.AccessDeniedPath = new PathString("/Account/Login");
            //    });
            services.AddSession();
        }

       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages("text/html","<h1 style='text-align:center'>Error 404</h1>");
            //app.UseDefaultFiles();
            app.UseRouting();
            app.UseStaticFiles();
            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}" 
                    );
                endpoints.MapControllers();
            });
        }
    }
}
