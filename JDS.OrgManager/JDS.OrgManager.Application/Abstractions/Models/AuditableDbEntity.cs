using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Abstractions.Models
{
    public abstract class AuditableDbEntity : IDbEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedUtc { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedUtc { get; set; }

    }
}
