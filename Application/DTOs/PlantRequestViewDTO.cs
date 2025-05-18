using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PlantRequestViewDTO
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public decimal PricePerMonth { get; set; }
        public string Description { get; set; }
        public string CareInstructions { get; set; }
        public int Stock { get; set; }
        public decimal? HeightInches { get; set; }
        public decimal? PotSizeInches { get; set; }
        public string Benefits { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }

        public string? ImageUrl { get; set; } // NEW
    }
}
