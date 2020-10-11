using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Infrastructure.Http
{
    public class MyHttpContext
    {
        private static IHttpContextAccessor httpContextAccessor;

        public static HttpContext Current => httpContextAccessor?.HttpContext;

        internal static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            MyHttpContext.httpContextAccessor = httpContextAccessor;
        }
    }
}
