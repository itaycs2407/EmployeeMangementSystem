using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using EmployeeMangement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace EmployeeMangement
{
    public class Startup
    {
        private IConfiguration config;

        public Startup(IConfiguration i_Config)
        {
            this.config = i_Config;
            
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // define the sql server to work with entity framework
            //services.AddDbContextPool<AppDBContext>(options => options.UseSqlServer(this.config.GetConnectionString("EmployeeDBConnection"),null));
            services.AddDbContextPool<AppDBContext>(options => options.UseSqlServer("server = (localdb)\\MSSQLLocalDB; database = EmployeeDB; Trusted_Connection = true", null));

            // add the built-in identity system of asp.net and define the password rules
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<AppDBContext>();


            services.AddMvc(config => {
                config.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters(); 
            
            // dependcy Injection 
            //services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepo>();
            services.AddAuthorization(option =>
            {
                option.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) // there are isProd/envirmonet/staging
            {
                /* DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
                 {
                     SourceCodeLineCount = 2
                 };
                 app.UseDeveloperExceptionPage(developerExceptionPageOptions);
                  */
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            /*
                        FileServerOptions fileServerOptions = new FileServerOptions();
                        fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
                        fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("htmlpage.html");
                        // do the work of the two middleware in comment above
                        app.UseFileServer(fileServerOptions);

                        //app.UseDefaultFiles(dfo);
                        //app.UseStaticFiles();
            */
            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();

            
                        // conventional routing
                        app.UseMvc(routes =>
                        {
                            routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        });
                       
          //  app.UseMvc();

        }
    }
}
