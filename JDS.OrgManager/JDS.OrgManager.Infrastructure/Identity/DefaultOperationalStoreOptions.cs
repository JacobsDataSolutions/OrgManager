using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Infrastructure.Identity
{
    public class DefaultOperationalStoreOptions : IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value => new OperationalStoreOptions();
    }
}
