using Autofac;
using MongoDB.Driver;
using Q2.Cqrs.Server.Crud.Repositories;

//using Q2.Oao.Registration.ApplicationServices.RegisterFinancialInstitution;

namespace Q2.Cqrs.Server.Api.Configuration
{
    public class IocModule : Module
    {
	    protected override void Load(ContainerBuilder builder)
	    {
		    builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces().SingleInstance();
			builder.RegisterAssemblyTypes(typeof(CartRepository).Assembly).AsImplementedInterfaces().SingleInstance();
			//builder.RegisterAssemblyTypes(typeof(RegisterFinancialInstitutionCommand).Assembly).AsImplementedInterfaces().SingleInstance();


			builder.Register(ctx => ctx.Resolve<AppSettings>().EventStoreSettings)
				.AsSelf().AsImplementedInterfaces().SingleInstance();


			//Mongo registration stuff
			builder.RegisterInstance(new MongoClient()).AsImplementedInterfaces().SingleInstance();
		    builder.Register(ctx =>
		    {
			    var appsettings = ctx.Resolve<AppSettings>();
			    var mongoClient = ctx.Resolve<IMongoClient>();

			    return mongoClient.GetDatabase(appsettings.MongoDatabase);

		    }).AsImplementedInterfaces().SingleInstance();
	    }
    }
}
