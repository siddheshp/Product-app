using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProductApp
{
    // This has has 2 methods
    // ConfigureServices() : Day before yesterday there was CODA on DI
    //                  This method will create instances of dependent objects or services
    //                  in ASP.NET Core. This method is automatically called by runtime (CLR)
    //                  DI is inbuilt into ASP.NEt Core, so you need not manually do it or 
    //                  use third party tool like Unity
    
    // Configure(): Contains middleware components. This is new concept in .NEt Core
    //  Every HTTP request and response will be acted by these components.
    // This method is again called by runtime.
    // We can use inbuilt middleware or can also create our own
    // lets see which middleware components are present in this method
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
            // lets talk about this method now
            // EF Core DBContext is added as dependency so that I can ibject it in Controller
            // constructor
            // This also allows me for better unit testing or using TDD approach efficiently
            services.AddDbContext<ApplicationDbContext>(options =>
            // specifying that SQL server be used    
            options.UseSqlServer(
                // read conn string from appsettings.json
                    Configuration.GetConnectionString("DefaultConnection")));
            // configure ASP.NEt Identity
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // these 2 are self explnatory
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // will check if this dev environment using ASP.NET runtime environment variable
            // you can see it in project properties
            // you can change it value
            // you can also add new envinment variables there
            if (env.IsDevelopment())
            {
                // displays detailed erros in view
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // if we are in PROD environment, we dont want to display detailed error
                // instead display generic error view
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // redirect all incoming requests to HTTS ptorocol
            app.UseHttpsRedirection();
            // server static files. If commented, static files in wwwroot 
            //will not be served to client/browser
            app.UseStaticFiles();

            // self explanatory
            app.UseRouting();

            // since we selected Individual user account, ASP.NET Core Identity memebership will be used
            // for authentication and authoirization
            // I hope everone is aware of diff between authentication and authoirization :)
            app.UseAuthentication();
            app.UseAuthorization();

            // Here we can configure MVC routes
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                // ASP.NET Core Identity uses (internally) razor pages and not razor views 
                // hence this middleware component.
                // if we had not selected Individual user account while creating project
                // this line would be present here
                endpoints.MapRazorPages();
            });

            // The sequence of some of these middleware components is important
            // for more info refer https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#middleware-order 
        }
    }
}
