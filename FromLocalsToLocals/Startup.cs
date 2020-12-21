using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NToastNotify;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Localization;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Web.Utilities;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Services.Ado;
using FromLocalsToLocals.Utilities.Helpers;
using FromLocalsToLocals.Web.Utilities.Jwt;

namespace FromLocalsToLocals.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                //options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddLocalization(opts =>
            {
                opts.ResourcesPath = "Resources";
            });

            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo> {
                new CultureInfo("en"),
                new CultureInfo("lt"),
                };
                opts.DefaultRequestCulture = new RequestCulture("en");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
                opts.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                };
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
           
            services.Configure<SendGridAccount>(Configuration.GetSection("SendGridAccount"));

            services.AddDbContext<AppDbContext>(options =>
                options.UseLazyLoadingProxies()
                .UseNpgsql(Configuration.GetConnectionString("AppDbContext")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Account/Login";
            });

            services.AddHttpClient();


            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = false,
                PositionClass = ToastPositions.BottomCenter
            });

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<IReviewsService, ReviewsService>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IVendorServiceADO, VendorServiceADO>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IFollowerService, FollowerService>();


            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<IWebApiClient, WebApiClient>();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCookiePolicy();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseNToastNotify();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotiHub>("/notiHub");
                endpoints.MapHub<MessageHub>("/msgHub");

            });
        }
    }
}
