using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsAPI.Repository;
using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore.V1;
using Grpc.Core;
using Grpc.Auth;
using Google.Cloud.Diagnostics.AspNetCore;
using Google.Cloud.Diagnostics.Common;
using Microsoft.Extensions.Logging;

namespace ProductsAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<StackdriverOptions>(
                Configuration.GetSection("Stackdriver"));
            //services.AddGoogleExceptionLogging(options =>
            //{
            //    options.ProjectId = Configuration["Stackdriver:ProjectId"];
            //    options.ServiceName = Configuration["Stackdriver:ServiceName"];
            //    options.Version = Configuration["Stackdriver:Version"];
            //});

            //// Add trace service.
            //services.AddGoogleTrace(options =>
            //{
            //    options.ProjectId = Configuration["Stackdriver:ProjectId"];
            //    options.Options = TraceOptions.Create(
            //        bufferOptions: BufferOptions.NoBuffer());
            //});
            services.AddCors(o => o.AddPolicy("AmCart", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddCustomMVC(Configuration)
               .AddIntegrationServices(Configuration);

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            IApplicationLifetime lifetime, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Adds middleware for using HSTS, which adds the Strict-Transport-Security header.
                app.UseHsts();
            }

            // Configure logging service.
            //loggerFactory.AddGoogle(app.ApplicationServices, Configuration["Stackdriver:ProjectId"]);
            //var logger = loggerFactory.CreateLogger("Product-API-Logger");
            // Write the log entry.
            //logger.LogInformation("Stackdriver logging started. This is a sample log message from Product API.");
            // Configure error reporting service.
            //app.UseGoogleExceptionLogging();
            // Configure trace service.
            //app.UseGoogleTrace();

            app.UseCors("AmCart");
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //GoogleCredential cred = GoogleCredential.FromFile("./Assets/credentials.json");
            //Channel channel = new Channel(FirestoreClient.DefaultEndpoint.Host,
            //              FirestoreClient.DefaultEndpoint.Port,
            //              cred.ToChannelCredentials());
            //FirestoreClient client = FirestoreClient.Create(channel);
            string project = configuration.GetValue<string>("ProductFirebaseDB");
            //FirestoreDb db = FirestoreDb.Create(project);
            //services.AddTransient<IProductRepository>(ab => { return new ProductRepository(db); });
            services.AddTransient<IProductRepository>(ab => { return new ProductRepository(null); });

            return services;
        }

    }
}
