using System;
using Autofac;
using SmileDirect.Web.Services;

namespace SmileDirect.Web.Modules
{
    public class HttpClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClientService>().As<IHttpClientService>();
        }
    }
}