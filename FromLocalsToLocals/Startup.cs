using System;
using System.Collections.Generic;
using System.Globalization;
using FromLocalsToLocals.Contracts.Entities;
using FromLocalsToLocals.Database;
using FromLocalsToLocals.Services.Ado;
using FromLocalsToLocals.Services.EF;
using FromLocalsToLocals.Utilities;
using FromLocalsToLocals.Web.Utilities;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NToastNotify;

namespace FromLocalsToLocals.Web
{
    public class Startup
    {
        public readonly AppDbContext _context;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Startup(AppDbContext context)
        {
            _context = context;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                //options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("lt")
                };
                opts.DefaultRequestCulture = new RequestCulture("en");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
                opts.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.Configure<SendGridAccount>(Configuration.GetSection("SendGridAccount"));

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("AppDbContext"),
                    options => options.SetPostgresVersion(new Version(9, 6))));

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


            services.AddMvc().AddNToastNotifyToastr(new ToastrOptions
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

            services.AddSignalR();

            services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMemoryStorage());


            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            app.UseHangfireDashboard();
            var sendAll = new SendAllSubscribers(_context);
            //RecurringJob.AddOrUpdate(() => sendAll.SendingAll(), Cron.MinuteInterval(1));


            app.UseCookiePolicy();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseNToastNotify();

            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<NotiHub>("/notiHub");
                endpoints.MapHub<MessageHub>("/msgHub");
            });
        }
    }
}