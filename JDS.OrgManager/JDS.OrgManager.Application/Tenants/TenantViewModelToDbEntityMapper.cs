using JDS.OrgManager.Application.Abstractions.Mapping;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Tenants
{
    public class TenantViewModelToDbEntityMapper : IViewModelToDbEntityMapper<TenantViewModel, TenantEntity>
    {
        public TenantEntity Map(TenantViewModel source) => source.Adapt<TenantEntity>();
        public void Map(TenantViewModel source, TenantEntity destination) => source.Adapt(destination);
    }
}
