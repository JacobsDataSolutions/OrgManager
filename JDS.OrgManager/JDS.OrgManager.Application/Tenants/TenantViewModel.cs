using JDS.OrgManager.Application.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantViewModel : IViewModel
    {
        public Guid AssignmentKey { get; set; }

        public int? Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
