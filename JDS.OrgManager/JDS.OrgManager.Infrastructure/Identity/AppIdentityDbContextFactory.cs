using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Infrastructure.Identity
{
    public class AppIdentityDbContextFactory : DesignAppTimeIdentityDbContextFactoryBase<AppIdentityDbContext>
    {
        protected override AppIdentityDbContext CreateNewInstance(DbContextOptions<AppIdentityDbContext> options) => new AppIdentityDbContext(options, new DefaultOperationalStoreOptions());
    }
}
