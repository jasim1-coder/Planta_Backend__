using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RoleRequest
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public int? PlanId { get; set; }

        // Navigation Properties
        public CreateSellerRoleRequestDTO SellerDetail { get; set; }
        public DeliveryPersonDetail DeliveryPersonDetail { get; set; }
        public PlantCaretakerDetail PlantCaretakerDetail { get; set; }
    }
}
