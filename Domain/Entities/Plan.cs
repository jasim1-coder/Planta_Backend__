using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string BillingCycle { get; set; } = null!;     // “Monthly” or “Annual”
        public decimal Price { get; set; }
    }
}
