using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmileDirect.Web.Services;
using SmileDirect.Web.Services.Launchpad;

namespace SmileDirect.Web.Modules
{
    public class LaunchpadModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SpaceXApiLaunchpadService>().AsSelf();

            builder.RegisterType<DatabaseLaunchpadService>().AsSelf();

            builder.Register<ILaunchpadService>((c, p) => {
                var type = p.TypedAs<string>();

                switch(type) {
                    case string s when s.Equals("spacex", StringComparison.InvariantCultureIgnoreCase):
                        return c.Resolve<SpaceXApiLaunchpadService>();
                    case string s when s.Equals("database", StringComparison.InvariantCultureIgnoreCase):
                        return c.Resolve<DatabaseLaunchpadService>();
                    default:
                        throw new ArgumentException("LaunchpadService implementation not found.");
                }
            });
        }
    }
}