using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmileDirect.Web.Models;

namespace SmileDirect.Web.Services.Launchpad
{
    public class SpaceXApiLaunchpadService : ILaunchpadService
    {
        private IConfiguration Configuration { get; set; }
        private IHttpClientService Client { get; set; }

        public SpaceXApiLaunchpadService(IConfiguration configuration, IHttpClientService client)
        {
            Configuration = configuration;
            Client = client;
        }

        public async Task<IEnumerable<LaunchpadModel>> GetAllAsync()
        {
            var baseUrl = Configuration.GetValue("SpaceX:BaseUrl", "https://api.spacexdata.com");
            var version = Configuration.GetValue("SpaceX:Version", "v3");
            var builder = new UriBuilder(baseUrl);
            var definition = new[] { new { id = string.Empty, full_name = string.Empty, status = string.Empty } };

            builder.Path = $"{version}/launchpads";

            var response = await Client.GetAsync(builder.ToString());
            var body = await response.Content.ReadAsStringAsync();
            var launchpads = JsonConvert.DeserializeAnonymousType(body, definition);
            var result = launchpads.Select(l => new LaunchpadModel {
                Id = l.id,
                Name = l.full_name,
                Status = (LaunchpadStatus) Enum.Parse(typeof(LaunchpadStatus), l.status.Replace(" ", ""), true)
            });

            return result;
        }
    }
}