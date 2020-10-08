using GraemSheppardOnline.Models;
using GraemSheppardOnline.Services.MongoDBContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using GraemSheppardOnline.Controllers;
using GraemSheppardOnline.Code;
using System.Security.Claims;

namespace GraemSheppardOnline
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            ConfigManager.SetInstance(Configuration);

            services.AddControllersWithViews();
            services.AddRazorPages();
            
            services.AddSession();

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "UserCookie";
                    config.LoginPath = "/Login";
                    config.AccessDeniedPath = "/Login";
                    config.Events.OnSigningOut = async x => await AuthenticateManager.SignUserOut(x);
                    config.Events.OnSigningIn = async x => await AuthenticateManager.SignUserIn(x);
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                    
                });

            services.AddHttpContextAccessor();

            

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            

        }
    }
}
