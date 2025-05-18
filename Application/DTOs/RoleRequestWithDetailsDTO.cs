namespace Application.DTOs
{
    public class RoleRequestWithDetailsDTO
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string RequestedRole { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public int? PlanId { get; set; }

        // Role-specific details properties:
        public SellerDetailDTO SellerDetails { get; set; }
        public DeliveryPersonDetailDTO DeliveryPersonDetails { get; set; }
        public PlantCaretakerDetailDTO PlantCaretakerDetails { get; set; }
    }
}
