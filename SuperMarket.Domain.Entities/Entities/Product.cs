using SuperMarket.Domain.Entities.Common;

namespace SuperMarket.Domain.Entities.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string Title { get; set; }

        public string Code { get; set; }

        public string Sku { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
