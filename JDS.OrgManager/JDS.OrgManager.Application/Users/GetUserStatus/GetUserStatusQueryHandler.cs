using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Users.GetUserStatus
{
    public class GetUserStatusQuery : IRequest<UserStatusViewModel>, ICacheableQuery
    {
        public int AspNetUsersId { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(GetUserStatusQuery)}-{AspNetUsersId}";

        public bool RefreshCachedEntry { get; set; }

        public bool ReplaceCachedEntry { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }

        public int? TenantId { get; set; }

        public class GetUserStatusQueryHandler : IRequestHandler<GetUserStatusQuery, UserStatusViewModel>
        {
            private readonly IApplicationReadDbFacade facade;

            public GetUserStatusQueryHandler(IApplicationReadDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<UserStatusViewModel> Handle(GetUserStatusQuery request, CancellationToken cancellationToken)
            {
                var userStatus = await facade.QueryFirstOrDefaultAsync<UserStatusViewModel>(
                    @"
						SELECT TOP 1
                            u.IsCustomer,
                            c.Id CustomerId,
	                        CASE WHEN c.Id IS NULL THEN 0 ELSE 1 END HasProvidedCustomerInformation,
	                        CASE WHEN e.Id IS NULL THEN 0 ELSE 1 END IsApprovedEmployee
                        FROM AspNetUsers u WITH(NOLOCK)
                        LEFT JOIN Customers c WITH(NOLOCK) ON c.AspNetUsersId = u.Id
                        LEFT JOIN Employees e WITH(NOLOCK) ON e.AspNetUsersId = u.Id AND (@TenantId IS NULL OR e.TenantId = @TenantId)
                        WHERE u.Id = @AspNetUsersId
                    ", request);
                userStatus.AuthorizedTenants = (await facade.QueryAsync<int>("SELECT TenantId FROM TenantAspNetUsers t WITH(NOLOCK) WHERE AspNetUsersId = @AspNetUsersId", request)).ToArray();
                return userStatus;
            }
        }
    }
}
