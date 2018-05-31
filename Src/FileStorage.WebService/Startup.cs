using Common.Repository;
using FileStorage.BL;
using FileStorage.Core;
using FileStorage.Repository.Core;
using FileStorage.Repository.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileStorage.WebService
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
            services.AddTransient<IDbConfig, FileStorageConfig>();
            services.AddTransient<IFileStorageUnitOfWork, FileStorageUnitOfWork>();
            services.AddTransient<IFileStorageConfig, FileStorageConfig>();
            services.AddTransient<IFileStorageService, FileStorageService>();

            services.AddMvc();

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();
        }
    }
}
