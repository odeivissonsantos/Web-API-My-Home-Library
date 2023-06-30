using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Globalization;
using WebAPI_My_Home_Library.Context;
using WebAPI_My_Home_Library.Services;
using WebAPI_My_Home_Library.Utils;

namespace WebAPI_My_Home_Library
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
            Settings.IsDesenv = Configuration["Ambiente"] == "2"; // 1 - Produção; 2 - Desenvolvimento;

            services.AddEntityFrameworkSqlServer()
                     .AddDbContext<MyHomeLibraryContext>(options => options.UseSqlServer(
                         Settings.IsDesenv ? Configuration.GetConnectionString("ConnectionStringDesenvolvimento") :
                                                        Configuration.GetConnectionString("ConnectionStringProducao")
                                                    ));
            services.AddScoped<LoginBusiness>();
            services.AddScoped<UsuarioBusiness>();
            services.AddScoped<LivroBusiness>();
            services.AddScoped<Utilies>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddSwaggerDocumentation();

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Web API - My Home Library",
            //        Version = "v1",
            //        Description = "Versão 1.0.0.1",
            //    });
            //});

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseSwaggerDocumentation();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI - My Home Library v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
