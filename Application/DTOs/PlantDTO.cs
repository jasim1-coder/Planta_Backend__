using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PlantDTO
    {
        public int Id { get; set; }
        public Guid SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal PricePerMonth { get; set; }
        public string Description { get; set; }
        public string CareInstructions { get; set; }
        public int Stock { get; set; }
        public decimal HeightInches { get; set; }
        public decimal PotSizeInches { get; set; }
        public string Benefits { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
