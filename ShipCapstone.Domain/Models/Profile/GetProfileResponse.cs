using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipCapstone.Domain.Models.Profile
{
    public class GetProfileResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }= string.Empty;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
