using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(aes.Areas.Identity.IdentityHostingStartup))]
namespace aes.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            _ = builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}