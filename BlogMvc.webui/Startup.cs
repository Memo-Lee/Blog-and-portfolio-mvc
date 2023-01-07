using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogMvc.data;
using BlogMvc.data.Abstract;
using BlogMvc.data.Concrete.EfCore;
using BlogMvc.services;
using BlogMvc.webui.EmailServices;
using BlogMvc.webui.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace BlogMvc.webui
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(b => b.UseSqlite("Data Source=blogDb"));
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                //Password
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                //Lockout
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/logout";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".BlogApp.Security.Cookie",
                    // B kullanıcısı A kullanıcısının cookie'sine sahip olsa bile işlem yapamaz.
                    SameSite = SameSiteMode.Strict
                };
            });
            //...
            
            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                new SmtpEmailSender(
                    _configuration["EmailSender:Host"],
                   // getValue : istenilen formata dönüştürür
                   _configuration.GetValue<int>("EmailSender:Port"),
                   _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                    _configuration["EmailSender:UserName"],
                    _configuration["EmailSender:Password"]
                    )
                );
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddInfrastructureServices();
            
        }

        public void Configure(WebApplication app, IWebHostEnvironment env,
        IConfiguration configuration , UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider=new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(),"node_modules")),
                    RequestPath="/modules"
            });
            if (!app.Environment.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "blogdetails",
                    pattern:"{url}",
                    defaults: new {controller="Blog", action="details"}
                );
                
                endpoints.MapControllerRoute(
                    name: "projedetails",
                    pattern:"{url}",
                    defaults: new {controller="Project", action="details"}
                );

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            
            });
            SeedIdentity.Seed(userManager,roleManager,configuration).Wait(); // Wait() -> beacuse async
            app.MapRazorPages();
            app.Run();
        }

    }
}