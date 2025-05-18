using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public int PlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "active";
        public DateTime? TrialEndDate { get; set; }
        public DateTime? NextBillingDate { get; set; }
        public string PaymentGateway { get; set; } = "razorpay";
        public string? GatewaySubscriptionId { get; set; }
        public string? CouponCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
