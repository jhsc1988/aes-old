using aes.CommonDependecies;
using aes.Data;
using aes.Models.Datatables;
using aes.Repository;
using aes.Repository.IRepository;
using aes.Repository.UnitOfWork;
using aes.Services;
using aes.Services.BillsServices;
using aes.Services.BillsServices.BillsElektraServices.BillsElektra;
using aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraServices;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraServices.Is;
using aes.Services.BillsServices.BillsHoldingService;
using aes.Services.BillsServices.BillsHoldingService.IService;
using aes.Services.BillsServices.IBillsService;
using aes.Services.IServices;
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
using System.Threading.Tasks;

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
            _ = services.AddScoped<IBillsInlineEditorService, BillsInlineEditorService>();



            // bills services
            _ = services.AddScoped<IBillsHoldingService, BillsHoldingService>();

            // bills elektra services
            _ = services.AddScoped<IBillsElektraService, BillsElektraService>();
            _ = services.AddScoped<IBillsElektraAdvancesService, BillsElektraAdvancesService>();
            _ = services.AddScoped<IBillsElektraServicesService, BillsElektraServicesService>();

            // bills temp create
            _ = services.AddScoped<IBillsHoldingTempCreateService, BillsHoldingTempCreateService>();
            _ = services.AddScoped<IBillsElektraAdvancesTempCreateService, BillsElektraAdvancesTempCreateService>();
            _ = services.AddScoped<IBillsElektraTempCreateService, BillsElektraTempCreateService>();
            _ = services.AddScoped<IBillsTempEditorService, BillsTempEditorService>();
            _ = services.AddScoped<IBillsElektraServicesTempCreateService, BillsElektraServicesTempCreateService>();

            // bills upload services
            _ = services.AddScoped<IBillsElektraUploadService, BillsElektraUploadService>();
            _ = services.AddScoped<IBillsElektraAdvancesUploadService, BillsElektraAdvancesUploadService>();
            _ = services.AddScoped<IBillsHoldingUploadService, BillsHoldingUploadService>();

            // bills common services
            _ = services.AddScoped<IBillsValidationService, BillsValidationService>();
            _ = services.AddScoped<IBillsCheckService, BillsCheckService>();



            // other services
            _ = services.AddScoped<ICaseFileService, CaseFileService>();
            _ = services.AddScoped<IService, Service>();
            _ = services.AddScoped<ILetterService, LetterService>();
            _ = services.AddScoped<IOdsService, OdsService>();
            _ = services.AddScoped<IApartmentUploadService, ApartmentUploadService>();

            _ = services.AddScoped<IApartmentUpdateRepository, ApartmentUpdateRepository>();



            // serilog logger
            _ = services.AddSingleton(Log.Logger);



            // common dependecies
            _ = services.AddScoped<ICommonDependencies, CommonDependencies>();
            _ = services.AddScoped<IBillsCommonDependecies, BillsCommonDependecies>();




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
                      pattern: "{controller=Apartments}/{action=Index}/{id?}");
                  _ = endpoints.MapRazorPages();
              });
        }
    }
}
