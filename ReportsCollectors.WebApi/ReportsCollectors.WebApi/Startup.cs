using FastReport.ApiClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReportsCollectors.WebApi.Implementions;
using ReportsCollectors.WebApi.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using Serilog.Enrichers.Standard.Http;

namespace ReportsCollectors.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddStandardHttp();
            services.AddOptions();
            services.Configure<AppSettings>(Configuration);
            var configuration = Configuration.Get<AppSettings>();

            services.AddScoped<IDataBase, DataBase>();
            services.AddScoped<IFastReport, FastReportRepos>();
            services.AddTransient<ISmtp, Smtp>(); 

            services.AddHttpClient<IFastReport_ApiClient, FastReport_ApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.UrlFastReport);
                client.Timeout = TimeSpan.FromSeconds(configuration.TimeOutHTTPConnection);
            })
                .AddPolicyHandler(GetRetryPolicy());

            services.AddDbContext<DbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")), ServiceLifetime.Singleton);

            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportsCollectors.WebApi", Version = "v1" });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportsCollectors.WebApi v1"));
            }

            app.UseRouting();
            app.UseStandardHttpMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
