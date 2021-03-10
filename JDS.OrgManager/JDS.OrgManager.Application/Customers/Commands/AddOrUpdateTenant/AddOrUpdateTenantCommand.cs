// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Tenants;
using JDS.OrgManager.Common.Text;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Commands.AddOrUpdateTenant
{
    public class AddOrUpdateTenantCommand : IRequest<TenantViewModel>
    {
        public int AspNetUsersId { get; set; }

        public TenantViewModel Tenant { get; set; } = default!;

        public class AddOrUpdateTenantCommandHandler : IRequestHandler<AddOrUpdateTenantCommand, TenantViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IApplicationWriteDbFacade facade;

            // We still have the flexibility to inject in a specific mapper implementation, if we choose.
            private readonly IViewModelToDbEntityMapper<TenantViewModel, TenantEntity> mapper;

            public AddOrUpdateTenantCommandHandler(
                IApplicationWriteDbContext context,
                IApplicationWriteDbFacade facade,
                IViewModelToDbEntityMapper<TenantViewModel, TenantEntity> mapper)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<TenantViewModel> Handle(AddOrUpdateTenantCommand request, CancellationToken cancellationToken)
            {
                var isNew = false;

                // PRESENTATION/APPLICATION LAYER
                var tenantViewModel = request.Tenant;

                // PERSISTENCE LAYER
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var sqlTransaction = transaction.GetDbTransaction();

                    TenantEntity? tenantEntity = null;

                    // Check for existing slug/key on a different tenant.
                    var slug = tenantViewModel.Slug.Sluggify();
                    var id = await facade.QueryFirstOrDefaultAsync<int?>(
                        @"SELECT TOP 1 Id FROM Tenants WITH(NOLOCK) WHERE Slug = @slug AND (@Id IS NULL OR Id <> @Id)",
                        new { slug, tenantViewModel.Id }, sqlTransaction);

                    if (id != null)
                    {
                        throw new ApplicationLayerException("Tenant slug already in use by another tenant.");
                    }
                    else
                    {
                        tenantViewModel.Slug = slug;
                    }

                    // Update.
                    if (tenantViewModel.Id != 0)
                    {
                        var tenantId = tenantViewModel.Id;
                        tenantEntity = context.Tenants.Find(tenantId);
                        if (tenantEntity == null)
                        {
                            throw new ApplicationLayerException($"Tenant with ID {tenantId} is in the customer's access list, but does not seem to exist.");
                        }
                        mapper.Map(tenantViewModel, tenantEntity);
                    }
                    else
                    {
                        // Add.
                        var customerId = await facade.QueryFirstOrDefaultAsync<int?>("SELECT TOP 1 Id FROM Customers WHERE AspNetUsersId = @AspNetUsersId", request, sqlTransaction);
                        if (customerId == null)
                        {
                            throw new ApplicationLayerException("Customer information doesn't exist for the specified user.");
                        }
                        tenantEntity = mapper.Map(tenantViewModel);
                        tenantEntity.CustomerId = (int)customerId;
                        await context.Tenants.AddAsync(tenantEntity);
                        isNew = true;
                    }
                    await context.SaveChangesAsync();
                    tenantViewModel.Id = tenantEntity.Id;
                    if (isNew)
                    {
                        await context.TenantAspNetUsers.AddAsync(new TenantAspNetUserEntity { AspNetUsersId = request.AspNetUsersId, TenantId = tenantEntity.Id });
                        await context.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                return tenantViewModel;
            }
        }
    }
}