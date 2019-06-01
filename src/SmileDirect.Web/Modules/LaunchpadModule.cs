using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using SmileDirect.Web.Services;
using SmileDirect.Web.Services.Launchpad;

namespace SmileDirect.Web.Modules
{
    public class LaunchpadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ILaunchpadService>((c, p) => {
                var configuration = c.Resolve<IConfiguration>();
                var httpClient = c.Resolve<IHttpClientService>();

                var type = p.TypedAs<string>();

                switch(type) {
                    case string s when s.Equals("spacex", StringComparison.InvariantCultureIgnoreCase):
                        return new SpaceXApiLaunchpadService(configuration, httpClient);
                    case string s when s.Equals("database", StringComparison.InvariantCultureIgnoreCase):
                        return new DatabaseLaunchpadService();
                    default:
                        throw new ArgumentException("LaunchpadService implementation not found.");
                }
            });
        }
    }
}