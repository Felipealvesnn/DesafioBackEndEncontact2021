using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using TesteBackendEnContaact_Infra_IOC;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact
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
            //ativar e validar a autentificação do token
            services.AddSegurancaJWT(Configuration);
            services.ConfiguracoesSweeger();

            services.AddFluentMigratorCore()
                    .ConfigureRunner(rb => rb
                        .AddSQLite()
                        .WithGlobalConnectionString(Configuration.GetConnectionString("DefaultConnection"))
                        .ScanIn(typeof(ADDFK).Assembly).For.Migrations())
                    .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddSingleton(new DatabaseConfig { ConnectionString = Configuration.GetConnectionString("DefaultConnection") });
            services.AddScoped<IContactBookRepository, ContactBookRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
                options.AllowSynchronousIO = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TesteBackendEnContact v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }
}