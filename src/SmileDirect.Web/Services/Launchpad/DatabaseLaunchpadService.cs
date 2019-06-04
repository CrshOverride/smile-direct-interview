using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmileDirect.Web.Models;

namespace SmileDirect.Web.Services.Launchpad
{
    public class DatabaseLaunchpadService : ILaunchpadService
    {
        public async Task<IEnumerable<LaunchpadModel>> GetAllAsync(List<FilterModel> filters)
        {
            throw new NotImplementedException();
        }
    }
}