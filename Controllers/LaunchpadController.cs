using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmileDirectInterview.Models;
using SmileDirectInterview.Services.Launchpad;

namespace SmileDirectInterview.Controllers
{
    [Route("api/launchpads")]
    [ApiController]
    public class LaunchpadController : ControllerBase
    {
        private IConfiguration Configuration { get; set; }
        private Func<string, ILaunchpadService> LaunchpadServiceFactory { get; set; }

        public LaunchpadController(
            IConfiguration configuration,
            Func<string, ILaunchpadService> launchpadServiceFactory
        )
        {
            Configuration = configuration;
            LaunchpadServiceFactory = launchpadServiceFactory;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaunchpadModel>>> Get()
        {
            var provider = Configuration["Launchpads:Provider"];
            var launchpadService = LaunchpadServiceFactory(provider);
            var launchpads = await launchpadService.GetAll();

            return launchpads.ToList();
        }
    }
}