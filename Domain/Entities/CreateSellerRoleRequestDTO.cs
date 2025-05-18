using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CreateSellerRoleRequestDTO
    {
        public Guid UserId { get; set; }
        public int PlanId { get; set; }
        public string StoreName { get; set; }
        public string GstNumber { get; set; }
        public string BusinessEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
