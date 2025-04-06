using PetIsland.DataAccess.Data;
using PetIsland.Utility;
using PetIsland.DataAccess.DbInitializer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using PetIsland.Models;
using PetIsland.Models.Momo;
using PetIslandWeb.Services.Momo;
using PetIslandWeb.Services.Vnpay;

namespace PetIslandWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Connect MomoAPI
            //builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
            //builder.Services.AddScoped<IMomoService, MomoService>();

            var connectionPetIslandDbString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionPetIslandDB' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionPetIslandDbString);
            });

            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddIdentity<AppUserModel, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                //options.SignIn.RequireConfirmedEmail = false;
                //options.SignIn.RequireConfirmedPhoneNumber = false;
                //options.Password.RequireDigit = true;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredLength = 8;
                //options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value!;
                options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value!;
            });

            builder.Services.AddRazorPages();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // User settings.
                options.User.RequireUniqueEmail = false;
            });



            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            //Connect VNPay API
            //builder.Services.AddScoped<IVnPayService, VnPayService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("~/Views/Shared/NotFoundPage");
            //app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();
            app.UseHttpsRedirection();

            SeedDatabase();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
            name: "Areas",
            pattern: "{areas:exists}/{controller=Product}/{action=Index}/{id?}");

            app.MapRazorPages();

            await app.RunAsync();

            void SeedDatabase()
            {
                using var scope = app.Services.CreateScope();
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.Initialize();
            }
        }
    }
}
