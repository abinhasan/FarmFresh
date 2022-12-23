using SuperMarket.Domain.Entities.Common;

namespace SuperMarket.Domain.Entities.Entities
{
    public class Category : AuditableBaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
