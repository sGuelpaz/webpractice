using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPractice.Data;

[assembly: HostingStartup(typeof(WebPractice.Areas.Identity.IdentityHostingStartup))]
namespace WebPractice.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                //services.AddDbContext<WebPracticeContext>(options =>
                //    options.UseSqlServer(
                //        context.Configuration.GetConnectionString("WebPracticeContextConnection")));

                //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //    .AddEntityFrameworkStores<WebPracticeContext>();
            });
        }
    }
}