using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateSubscriptionDTO
    {
        public Guid SellerId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? TrialEndDate { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public string PaymentGateway { get; set; } = "razorpay";
        public string? GatewaySubscriptionId { get; set; }
        public string? CouponCode { get; set; }
    }
}
