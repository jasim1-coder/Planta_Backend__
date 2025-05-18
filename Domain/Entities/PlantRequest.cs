

namespace Domain.Entities
{
    public class PlantRequest
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PricePerMonth { get; set; }
        public string? Description { get; set; }
        public string? CareInstructions { get; set; }
        public int Stock { get; set; }
        public decimal? HeightInches { get; set; }
        public decimal? PotSizeInches { get; set; }
        public string? Benefits { get; set; }
        public DateTime RequestedAt { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime? ProcessedAt { get; set; }

        // Read-only fields populated via JOIN
        public string? SellerName { get; set; }
        public string? CategoryName { get; set; }

        public string? ImageUrl { get; set; } // NEW

    }
}
