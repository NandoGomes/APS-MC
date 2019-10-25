using System;
using System.IO;
using System.Reflection;
using APS.MC.Domain.APSContext.Handlers;
using APS.MC.Domain.APSContext.Repositories;
using APS.MC.Domain.APSContext.Services.ArduinoCommunication;
using APS.MC.Infra.CommonContext.DataContext;
using APS.MC.Infra.CommonContext.Repositories;
using APS.MC.Infra.CommonContext.Services;
using APS.MC.Infra.CommonContext.Services.ArduinoCommunicationService;
using APS.MC.Shared.APSShared;
using APS.MC.Shared.APSShared.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace APS.MC.API
{
	public class Startup
	{
		private readonly IHostingEnvironment _hostingEnvironment;

		public static IConfiguration Configuration { get; set; }

		public Startup(IHostingEnvironment hostingEnvironment)
		{
			LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
			_hostingEnvironment = hostingEnvironment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(_hostingEnvironment.IsProduction() ? "appsettings.json" : $"appsettings.{_hostingEnvironment.EnvironmentName}.json", false, false)
				.Build();

			Settings.ConnectionString = $"{Configuration["connectionString"]}";
			Settings.DatabaseName = $"{Configuration["databaseName"]}";

			Settings.ArduinoAddress = $"{Configuration["arduinoAddress"]}";

			Settings.DetailedLog = bool.Parse($"{Configuration["detailedLog"]}");

			services.AddScoped<APSDataContext, APSDataContext>();
			services.AddTransient<IArduinoCommunicationService, ArduinoCommunicationService>();

			services.AddTransient<SensorCommandHandler, SensorCommandHandler>();

			services.AddTransient<ISensorRepository, SensorRepository>();

			services.AddTransient<ILoggingService, LoggingService>();

			services.AddMvc().AddJsonOptions(options =>
			{
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			});

			services.AddResponseCompression();

			services.AddDistributedMemoryCache();

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = "APS API",
					Description = "APS Project",
					TermsOfService = "None"
				});

				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseMvc();

			app.UseResponseCompression();

			app.UseSwagger();

			app.UseSwaggerUI(options =>
			{
				options.DocumentTitle = "APS";

				options.SwaggerEndpoint("/swagger/v1/swagger.json", "APS - V1");
				options.RoutePrefix = string.Empty;

				options.InjectStylesheet(Path.Combine(AppContext.BaseDirectory, "swagger.css"));

				options.DefaultModelsExpandDepth(-1);
				options.DocExpansion(DocExpansion.None);
			});
		}
	}
}
