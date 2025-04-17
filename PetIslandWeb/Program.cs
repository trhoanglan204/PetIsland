using PetIsland.DataAccess.Data;
using PetIsland.Utility;
using PetIsland.DataAccess.DbInitializer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PetIsland.Models;
using PetIslandWeb.Hubs;
using PetIsland.Models.Momo;
using PetIslandWeb.Services.Momo;
using PetIslandWeb.Services.Vnpay;
using Microsoft.AspNetCore.Authentication.Google;

namespace PetIslandWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Connect MomoAPI
            builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
            builder.Services.AddScoped<IMomoService, MomoService>();

            var connectionPetIslandDbString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnectionPetIslandDB' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionPetIslandDbString);
            });

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IDbInitializer,DbInitializer>();

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
                // SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                // User settings.
                options.User.RequireUniqueEmail = true;
                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Redirects settings.
                options.LoginPath = "/Account/Login"; 
                // Cookie settings.
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //Use Always for HTTPS | None for dev mode
                options.Cookie.SameSite = SameSiteMode.None; //Prevent Cross-Site Request Forgery (CSRF)
            });

            builder.Services.AddAuthentication()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value!;
                options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value!;
            });

            builder.Services.AddRazorPages();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5140")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });

            builder.Services.AddSignalR();

            //Connect VNPay API
            builder.Services.AddScoped<IVnPayService, VnPayService>();

            //builder.Services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 7021;
            //});

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();
            app.UseCors("CorsPolicy");

            app.UseStatusCodePagesWithReExecute("/Home/Error/", "?statuscode={0}");


            SeedDatabase();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.MapControllerRoute(
            //name: "Areas",
            //pattern: "{areas:exists}/{controller=Product}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.MapHub<ChatHub>("/Realtime/Index");
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
