using System;

namespace SmileDirectInterview.Models
{
    public class LaunchpadModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public LaunchpadStatus Status { get; set; }
    }

    public enum LaunchpadStatus
    {
        Active,
        Retired,
        UnderConstruction
    }
}