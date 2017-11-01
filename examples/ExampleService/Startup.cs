namespace ExampleService
{
    using System;
    using System.Buffers;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using ExampleData;
    using ExampleData.Entities;
    using ExampleService.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Vanguard.Framework.Http.Filters;
    using Vanguard.Framework.Http.Formatters;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Mapper.Initialize(config =>
                {
                    config.CreateMap<Car, CarModel>();
                    config.CreateMap<CarModel, Car>();
                });

            ////var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            ////jsonFormatter.SerializerSettings.ContractResolver = new SelectFieldContractResolver()
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc(options =>
                {
                    options.Filters.Add(typeof(ExceptionFilter));
                    options.Filters.Add(typeof(ValidateModelAttribute));
                    options.OutputFormatters.Clear();
                    options.OutputFormatters.Add(new SelectFieldJsonOutputFormatter(JsonSerializerSettingsProvider.CreateSerializerSettings(), ArrayPool<char>.Shared));
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddDbContext<ExampleContext>(options => options.UseInMemoryDatabase("Test"));

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseDeveloperExceptionPage();
        }
    }
}
