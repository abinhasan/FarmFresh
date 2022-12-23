using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarket.Domain.Entities.Common
{
    public abstract class AuditableBaseEntity
    {
        public virtual int Id { get; set; }

        public string? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Created { get; set; }

        public string? LastModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LastModified { get; set; }
    }
}
