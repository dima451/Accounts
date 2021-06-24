using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Core.Entity.AccountingManagement;
using Accounting.Core.Repository;
using Accounts.DataAcess.Data;
using Accounts.DataAcess.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Accounting.WebHost
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

            services.AddControllers();


            services.AddDbContext<ApplicationContext>((x) =>
            {
                x.UseSqlite("FileName=AccountsDb.sqlite", x => x.MigrationsAssembly(typeof(Startup).Assembly.FullName));
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddMaps(typeof(Startup).Assembly);
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddScoped<IRepository<Account>, EFRepository<Account>>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounting.WebHost", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounting.WebHost v1"));
            }

            Migrate(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void Migrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.Database.Migrate();
        }
    }
}
