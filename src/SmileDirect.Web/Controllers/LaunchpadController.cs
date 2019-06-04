using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmileDirect.Web.Models;
using SmileDirect.Web.Services.Launchpad;

namespace SmileDirect.Web.Controllers
{
    [Route("api/launchpads")]
    [ApiController]
    public class LaunchpadController : ControllerBase
    {
        private IConfiguration Configuration { get; set; }
        private Func<string, ILaunchpadService> LaunchpadServiceFactory { get; set; }
        private ILogger Logger { get; set; }

        public LaunchpadController(
            IConfiguration configuration,
            Func<string, ILaunchpadService> launchpadServiceFactory,
            ILogger<LaunchpadController> logger
        )
        {
            Configuration = configuration;
            LaunchpadServiceFactory = launchpadServiceFactory;
            Logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaunchpadModel>>> Get([FromQuery] List<FilterModel> filters)
        {
            var provider = Configuration["Launchpads:Provider"];

            Logger.LogInformation($"Fetching launchpads using the following provider: {provider}");

            var launchpadService = LaunchpadServiceFactory(provider);
            var launchpads = await launchpadService.GetAllAsync(filters);

            return launchpads.ToList();
        }
    }
}