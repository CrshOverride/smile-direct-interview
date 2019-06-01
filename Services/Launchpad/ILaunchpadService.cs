using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmileDirectInterview.Models;

namespace SmileDirectInterview.Services.Launchpad
{
    public interface ILaunchpadService
    {
        Task<IEnumerable<LaunchpadModel>> GetAll();
    }
}