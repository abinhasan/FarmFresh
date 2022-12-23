﻿namespace SuperMarket.Domain.DTO
{
    public class Product
    {
        public string Title { get; set; }

        public string Code { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
