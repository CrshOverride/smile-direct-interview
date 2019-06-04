using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmileDirect.Web.Models;

namespace SmileDirect.Web.Services.Launchpad
{
    public interface ILaunchpadService
    {
        Task<IEnumerable<LaunchpadModel>> GetAllAsync(List<FilterModel> filters);
    }
}