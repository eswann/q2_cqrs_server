using System;
using System.Reflection;

using Autofac;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Q2.Cqrs.Server.Api.Configuration;
using Q2.Cqrs.Server.Api.Mapping;
using Q2.Library.Api;
using Q2.Library.Common.Configuration;
using Q2.Oao.Library.Api.Configuration;
using Q2.Oao.Library.Api.Context;
using Q2.Oao.Library.Api.Exceptions;
using Q2.Oao.Library.Api.Ioc;
using Q2.Oao.Library.EventStore.Subscriptions;
using Q2.Oao.Library.Logging;
using Q2.Oao.Library.Logging.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;

using ILogger = Serilog.ILogger;

namespace Q2.Cqrs.Server.Api
{
	public class Startup
	{
		private static readonly LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);
		private const string AureliaAppPolicyName = "AureliaApp";
		private readonly AppSettings _appSettings;
		private readonly IConfigurationRoot _configuration;

		// ReSharper disable once UnusedParameter.Local
		public Startup(IApplicationEnvironment appEnv)
		{
			_configuration = new ConfigurationBuilder()
				.SetBasePath(appEnv.ApplicationBasePath)
				.WithDefaultConfiguration()
				.Build();
			_appSettings = _configuration.Get<AppSettings>("AppSettings");
		}

		public void Configure(IApplicationBuilder app, ILoggerFactory loggerfactory, ILogger logger)
		{
			logger.Verbose("Configuring the Application");
			loggerfactory.MinimumLevel = LogLevel.Debug;
			loggerfactory.AddSerilog(logger);

			//app.UseAppBuilder(builder => { builder.Use<ScopeRequirementMiddleware>(new List<string> { _appSettings.IdentityServerSettings.ClientId, "write" }); }, _appSettings.ServiceSettings.ServiceName);

			app.UseCors(AureliaAppPolicyName);
			app.UseMvc();

			LoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			ILogger logger = new LogConfigurationFactory().Create()
				.WriteTo.Seq(_appSettings.LogSettings.EndpointUri)
				.WriteTo.ColoredConsole()
				.MinimumLevel.ControlledBy(LoggingLevelSwitch)
				.CreateLogger();


			services.AddCorsPolicy(AureliaAppPolicyName, _appSettings.CorsOrigins);

			AuthorizationPolicy authenticatedUserPolicy = AuthorizationPolicyFactory.GetFactoryInstance().Create(AuthorizationPolicyType.AuthenticatedUser);

			services.AddMvcCore(
				options =>
				{
					options.Filters.Add(typeof(ExceptionFilter));
				})
				.AddOaoJsonFormatters()

#warning Related to: https://github.com/aspnet/Mvc/issues/3094, remove after rc2 released.
				.AddViews();

			services.AddInstance(_configuration);
			services.AddInstance(_appSettings);

			IContainer container = AutofacBootstrapper.CreateContainer(
				logger,
				services,
				Assembly.GetExecutingAssembly(),
				typeof(SubscriptionFactory).Assembly,
				typeof(ContextMiddleware).Assembly,
				typeof(ILogSettings).Assembly);

			MappingRoot.Register();

			return container.Resolve<IServiceProvider>();
		}
	}
}