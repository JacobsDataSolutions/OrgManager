using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Application.Common.Employees;
using JDS.OrgManager.Application.Customers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantEntity : AuditableDbEntity
    {
        public ICollection<TenantAspNetUserEntity> AspNetUsers { get; set; }

        public Guid AssignmentKey { get; set; }

        public CustomerEntity Customer { get; set; }

        public int CustomerId { get; set; }

        public ICollection<EmployeeEntity> Employees { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public TenantEntity()
        {
            Employees = new HashSet<EmployeeEntity>();
            AspNetUsers = new HashSet<TenantAspNetUserEntity>();
        }
    }
}
