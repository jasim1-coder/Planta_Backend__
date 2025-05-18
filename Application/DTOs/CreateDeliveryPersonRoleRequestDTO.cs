using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateDeliveryPersonRoleRequestDTO
    {
        public Guid UserId { get; set; }
        public int PlanId { get; set; }
        public string VehicleType { get; set; }
        public string LicenseNumber { get; set; }
        public int YearsOfExperience { get; set; }
        public string PhoneNumber { get; set; }
    }

}
