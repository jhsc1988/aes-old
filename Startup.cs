using aes.CommonDependecies;
using aes.CommonDependecies.ICommonDependencies;
using aes.Data;
using aes.Models.Datatables;
using aes.Repository.IRepository;
using aes.Repository.Stan;
using aes.Services;
using aes.Services.IServices;
using aes.Services.RacuniServices;
using aes.Services.RacuniServices.Elektra.RacuniElektra;
using aes.Services.RacuniServices.Elektra.RacuniElektra.Is;
using aes.Services.RacuniServices.Elektra.RacuniElektraIzvrsenjeUsluge;
using aes.Services.RacuniServices.Elektra.RacuniElektraIzvrsenjeUsluge.Is;
using aes.Services.RacuniServices.Elektra.RacuniElektraRate;
using aes.Services.RacuniServices.Elektra.RacuniElektraRate.Is;
using aes.Services.RacuniServices.IRacuniService;
using aes.Services.RacuniServices.IServices;
using aes.Services.RacuniServices.RacuniHoldingService;
using aes.Services.RacuniServices.RacuniHoldingService.IService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Globalization;
using aes.UnitOfWork;

namespace aes
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
            RegisterServices(services);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection") ?? string.Empty));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddApplicationInsightsTelemetry();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // datatables
            services.AddScoped<IDatatablesSearch, DatatablesSearch>();
            services.AddScoped<IDatatablesGenerator, DatatablesGenerator>();
            services.AddScoped<IRacuniInlineEditorService, RacuniInlineEditorService>();

            // Racuni services
            services.AddScoped<IRacuniHoldingService, RacuniHoldingService>();

            // Racuni elektra services
            services.AddScoped<IRacuniElektraService, RacuniElektraService>();
            services.AddScoped<IRacuniElektraRateService, RacuniElektraRateService>();
            services.AddScoped<IRacuniElektraIzvrsenjeUslugeService, RacuniElektraIzvrsenjeUslugeService>();

            // Racuni temp create
            services.AddScoped<IRacuniHoldingTempCreateService, RacuniHoldingTempCreateService>();
            services.AddScoped<IRacuniElektraRateTempCreateService, RacuniElektraRateTempCreateService>();
            services.AddScoped<IRacuniElektraTempCreateService, RacuniElektraTempCreateService>();
            services.AddScoped<IRacuniTempEditorService, RacuniTempEditorService>();
            services
                .AddScoped<IRacuniElektraIzvrsenjeUslugeTempCreateService,
                    RacuniElektraIzvrsenjeUslugeTempCreateService>();

            // Racuni upload services
            services.AddScoped<IRacuniElektraUploadService, RacuniElektraUploadService>();
            services.AddScoped<IRacuniElektraRateUploadService, RacuniElektraRateUploadService>();
            services.AddScoped<IRacuniHoldingUploadService, RacuniHoldingUploadService>();

            // Racuni common services
            services.AddScoped<IRacuniCheckService, RacuniCheckService>();

            // other services
            services.AddScoped<IPredmetiervice, PredmetiService>();
            services.AddScoped<IService, Service>();
            services.AddScoped<IDopisiervice, DopisiService>();
            services.AddScoped<IOdsService, OdsService>();
            services.AddScoped<IStanUploadService, StanUploadService>();
            services.AddScoped<IStanUpdateRepository, StanUpdateRepository>();

            // serilog logger
            services.AddSingleton(Log.Logger);

            // common dependecies
            services.AddScoped<ICommonDependencies, CommonDependencies>();
            services.AddScoped<IRacuniCommonDependecies, RacuniCommonDependecies>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CultureInfo cultureInfo = new("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // disable registration
                endpoints.MapGet("/Identity/Account/Register",
                    context => Task.Factory.StartNew(() =>
                        context.Response.Redirect("/Identity/Account/Login", true, true)));
                endpoints.MapPost("/Identity/Account/Register",
                    context => Task.Factory.StartNew(() =>
                        context.Response.Redirect("/Identity/Account/Login", true, true)));

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Stanovi}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}