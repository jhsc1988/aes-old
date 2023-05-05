using aes.CommonDependecies;
using aes.CommonDependecies.ICommonDependencies;
using aes.Data;
using aes.Models.Datatables;
using aes.Repositories.IRepository;
using aes.Repositories.Stan;
using aes.Repositories.UnitOfWork;
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

            /******************************** Dependency Injection ********************************/

            // unit of work
            _ = services.AddScoped<IUnitOfWork, UnitOfWork>();

            // datatables
            _ = services.AddScoped<IDatatablesSearch, DatatablesSearch>();
            _ = services.AddScoped<IDatatablesGenerator, DatatablesGenerator>();
            _ = services.AddScoped<IRacuniInlineEditorService, RacuniInlineEditorService>();



            // Racuni services
            _ = services.AddScoped<IRacuniHoldingService, RacuniHoldingService>();

            // Racuni elektra services
            _ = services.AddScoped<IRacuniElektraService, RacuniElektraService>();
            _ = services.AddScoped<IRacuniElektraRateService, RacuniElektraRateService>();
            _ = services.AddScoped<IRacuniElektraIzvrsenjeUslugeService, RacuniElektraIzvrsenjeUslugeService>();

            // Racuni temp create
            _ = services.AddScoped<IRacuniHoldingTempCreateService, RacuniHoldingTempCreateService>();
            _ = services.AddScoped<IRacuniElektraRateTempCreateService, RacuniElektraRateTempCreateService>();
            _ = services.AddScoped<IRacuniElektraTempCreateService, RacuniElektraTempCreateService>();
            _ = services.AddScoped<IRacuniTempEditorService, RacuniTempEditorService>();
            _ = services.AddScoped<IRacuniElektraIzvrsenjeUslugeTempCreateService, RacuniElektraIzvrsenjeUslugeTempCreateService>();

            // Racuni upload services
            _ = services.AddScoped<IRacuniElektraUploadService, RacuniElektraUploadService>();
            _ = services.AddScoped<IRacuniElektraRateUploadService, RacuniElektraRateUploadService>();
            _ = services.AddScoped<IRacuniHoldingUploadService, RacuniHoldingUploadService>();

            // Racuni common services
            _ = services.AddScoped<IRacuniCheckService, RacuniCheckService>();



            // other services
            _ = services.AddScoped<IPredmetiervice, Predmetiervice>();
            _ = services.AddScoped<IService, Service>();
            _ = services.AddScoped<IDopisiervice, Dopisiervice>();
            _ = services.AddScoped<IOdsService, OdsService>();
            _ = services.AddScoped<IStanUploadService, StanUploadService>();

            _ = services.AddScoped<IStanUpdateRepository, StanUpdateRepository>();



            // serilog logger
            _ = services.AddSingleton(Log.Logger);



            // common dependecies
            _ = services.AddScoped<ICommonDependencies, CommonDependencies>();
            _ = services.AddScoped<IRacuniCommonDependecies, RacuniCommonDependecies>();




            _ = services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(
                      Configuration.GetConnectionString("DefaultConnection")));

            _ = services.AddDatabaseDeveloperPageExceptionFilter();

            _ = services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            _ = services.AddControllersWithViews();
            _ = services.AddApplicationInsightsTelemetry();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CultureInfo cultureInfo = new("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
                _ = app.UseMigrationsEndPoint();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                _ = app.UseHsts();
            }
            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles();

            _ = app.UseRouting();

            _ = app.UseAuthentication();
            _ = app.UseAuthorization();

            _ = app.UseEndpoints(endpoints =>
              {
                  // disable registration
                  _ = endpoints.MapGet("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
                  _ = endpoints.MapPost("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));

                  _ = endpoints.MapControllerRoute(
                      name: "default",
                      pattern: "{controller=Stanovi}/{action=Index}/{id?}");
                  _ = endpoints.MapRazorPages();
              });
        }
    }
}
