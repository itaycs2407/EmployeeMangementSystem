using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using EmployeeMangement.Models;

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
               
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddXmlSerializerFormatters();
            // dependcy Injection 
            services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env
            )
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
