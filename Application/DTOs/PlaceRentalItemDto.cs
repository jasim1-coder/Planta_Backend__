﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PlaceRentalItemDto
    {
        public long PlantId { get; set; }
        public Guid SellerId { get; set; }
        public int Quantity { get; set; }
        public int DurationMonths { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
