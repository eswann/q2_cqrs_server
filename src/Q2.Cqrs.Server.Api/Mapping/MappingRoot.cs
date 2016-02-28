using System.Reflection;
using Mapster;

namespace Q2.Cqrs.Server.Api.Mapping
{
    public static class MappingRoot
    {
	    public static void Register()
	    {
			//TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
			//TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;

		    var registers = TypeAdapterConfig.GlobalSettings.Scan(
			    Assembly.GetExecutingAssembly());

		    TypeAdapterConfig.GlobalSettings.Compile();
	    }
    }
}
