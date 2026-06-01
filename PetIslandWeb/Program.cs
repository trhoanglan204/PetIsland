using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.DataAccess.DbInitializer;
using PetIsland.Models;
using PetIsland.Models.Momo;
using PetIsland.Utility;
using PetIslandWeb.Hubs;
using PetIslandWeb.Services.Momo;
using PetIslandWeb.Services.ORS;
using PetIslandWeb.Services.Paypal;
using PetIslandWeb.Services.Vnpay;
using Microsoft.AspNetCore.HttpOverrides;
using System.Security.Cryptography.X509Certificates;

namespace PetIslandWeb
{
    public class Program
    {
        private static readonly int portHTTP    = 8080;
        private static readonly int portHTTPS   = 8081;

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
            builder.Services.AddHttpClient<GeocodingService>(); //scoped

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
                options.AccessDeniedPath = "/Account/AccessDenied";
                // Cookie settings.
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //Use Always for HTTPS | None for dev mode
                options.Cookie.SameSite = SameSiteMode.Lax; //Prevent Cross-Site Request Forgery (CSRF)
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // default challenge = Google
            })
            .AddCookie() // session management
            .AddGoogle(options =>
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
                        builder.WithOrigins($"http://localhost:{portHTTP}")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });

            builder.Services.AddSignalR();

            //Connect VNPay API
            builder.Services.AddScoped<IVnPayService, VnPayService>();

            //Connect Paypal API
            builder.Services.AddScoped<IPaypalService, PaypalService>();

            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = portHTTPS;
            });

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            builder.WebHost.ConfigureKestrel(options => 
            {
                var certPath = builder.Configuration["KestrelCertificates:CertPath"];
                var keyPath = builder.Configuration["KestrelCertificates:KeyPath"];

                options.ListenAnyIP(portHTTPS, listenOption => 
                {
                    if (!string.IsNullOrEmpty(certPath) && !string.IsNullOrEmpty(keyPath))
                    {
                        // 1. Load the ephemeral certificate from the PEM files
                        using var ephemeralCert = X509Certificate2.CreateFromPemFile(certPath, keyPath);

                        // 2. Export it to a PFX format in memory (this makes it compatible with Windows)
                        var pfxBytes = ephemeralCert.Export(X509ContentType.Pfx);

                        // 3. Re-import it as a standard certificate that Windows Schannel/Kestrel can use
                        var windowsCompatibleCert = new X509Certificate2(pfxBytes);

                        // 4. Use the compatible certificate
                        listenOption.UseHttps(windowsCompatibleCert);
                    }
                });
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseForwardedHeaders();
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

            app.MapControllerRoute(
            name: "Areas",
            pattern: "{areas:exists}/{controller=Product}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.MapHub<ChatHub>("/Realtime/Index");

            app.MapControllers();

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
