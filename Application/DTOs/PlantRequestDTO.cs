
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class PlantRequestDTO
    {
        public Guid SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal PricePerMonth { get; set; }
        public string Description { get; set; }
        public string CareInstructions { get; set; }
        public int Stock { get; set; }
        public decimal? HeightInches { get; set; }
        public decimal? PotSizeInches { get; set; }
        public string Benefits { get; set; }
        public IFormFile? Image { get; set; } // NEW

    }
}
