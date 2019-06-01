using System;
using System.Net.Http;
using Autofac;
using Microsoft.Extensions.Configuration;
using SmileDirectInterview.Services.Launchpad;

namespace SmileDirectInterview.Modules
{
    public class LaunchpadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ILaunchpadService>((c, p) => {
                var configuration = c.Resolve<IConfiguration>();
                var httpClient = c.Resolve<HttpClient>();

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