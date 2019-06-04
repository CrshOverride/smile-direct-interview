using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmileDirect.Web.Models;

namespace SmileDirect.Web.Services.Launchpad
{
    public class SpaceXApiLaunchpadService : ILaunchpadService
    {
        private IConfiguration Configuration { get; set; }
        private IHttpClientService Client { get; set; }
        private ILogger Logger { get; set; }

        public SpaceXApiLaunchpadService(IConfiguration configuration, IHttpClientService client, ILogger<SpaceXApiLaunchpadService> logger)
        {
            Configuration = configuration;
            Client = client;
            Logger = logger;
        }

        public async Task<IEnumerable<LaunchpadModel>> GetAllAsync(List<FilterModel> filters)
        {
            var baseUrl = Configuration.GetValue("SpaceX:BaseUrl", "https://api.spacexdata.com");
            var version = Configuration.GetValue("SpaceX:Version", "v3");
            var builder = new UriBuilder(baseUrl);
            var definition = new[] { new { id = string.Empty, full_name = string.Empty, status = string.Empty } };

            builder.Path = $"{version}/launchpads";

            Logger.LogInformation($"Requesting SpaceX data from: {builder}");

            var response = await Client.GetAsync(builder.ToString());
            var body = await response.Content.ReadAsStringAsync();
            var launchpads = JsonConvert.DeserializeAnonymousType(body, definition);

            Logger.LogInformation($"Found {launchpads.Length} launchpads from SpaceX");

            var mapped = launchpads.Select(l => new LaunchpadModel {
                Id = l.id,
                Name = l.full_name,
                Status = (LaunchpadStatus) Enum.Parse(typeof(LaunchpadStatus), l.status.Replace(" ", ""), true)
            });

            if (filters == null || filters.Count == 0) { return mapped; }

            Logger.LogInformation($"Applying additional filters");

            return ApplyFilters(mapped.AsQueryable(), filters);
        }

        private IEnumerable<LaunchpadModel> ApplyFilters(IQueryable<LaunchpadModel> launchpads, List<FilterModel> filters)
        {
            return filters.Aggregate(launchpads, (ls, filter) => {
                switch(filter.Field.ToLowerInvariant()) {
                    case "id":
                        return ls.Where(l => l.Id.Contains(filter.Value, StringComparison.InvariantCultureIgnoreCase));
                    case "name":
                        return ls.Where(l => l.Name.Contains(filter.Value, StringComparison.InvariantCultureIgnoreCase));
                    case "status":
                        try
                        {
                            var s = (LaunchpadStatus) Enum.Parse(typeof(LaunchpadStatus), filter.Value, true);
                            return ls.Where(l => l.Status == s);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogCritical($"Invalid enum value specified in filter: {filter.Value}");
                            throw ex;
                        }
                    default:
                        Logger.LogCritical($"Invalid filter field specified: {filter.Field}");
                        throw new ArgumentException("Invalid filter field specified");
                }
            });
        }
    }
}