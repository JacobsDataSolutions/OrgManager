// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.Abstractions.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Commands.AddOrUpdateCustomer
{
    public class AddOrUpdateCustomerCommand : IRequest<CustomerViewModel>
    {
        public int AspNetUsersId { get; init; }

        public CustomerViewModel Customer { get; init; } = default!;

        public class AddOrUpdateCustomerCommandHandler : IRequestHandler<AddOrUpdateCustomerCommand, CustomerViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IViewModelToDbEntityMapper<CustomerViewModel, CustomerEntity> customerMapper;

            private readonly IApplicationWriteDbFacade facade;

            public AddOrUpdateCustomerCommandHandler(
                IApplicationWriteDbContext context,
                IApplicationWriteDbFacade facade,
                IViewModelToDbEntityMapper<CustomerViewModel, CustomerEntity> customerMapper
                )
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.customerMapper = customerMapper ?? throw new ArgumentNullException(nameof(customerMapper));
            }

            public async Task<CustomerViewModel> Handle(AddOrUpdateCustomerCommand request, CancellationToken cancellationToken)
            {
                // PRESENTATION/APPLICATION LAYER
                var customerViewModel = request.Customer;

                // PERSISTENCE LAYER
                var customerAdded = false;
                using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
                {
                    var sqlTransaction = transaction.GetDbTransaction();
                    var customerEntity = await context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.AspNetUsersId == request.AspNetUsersId, cancellationToken: cancellationToken);
                    if (customerEntity != null)
                    {
                        // Update.
                        customerMapper.Map(customerViewModel, customerEntity);
                    }
                    else
                    {
                        // Add.
                        customerEntity = customerMapper.Map(customerViewModel);
                        customerAdded = true;
                    }
                    customerEntity.AspNetUsersId = request.AspNetUsersId;
                    await context.Customers.AddAsync(customerEntity, cancellationToken);
                    context.Entry(customerEntity).State = customerAdded ? EntityState.Added : EntityState.Modified;

                    await context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                return customerViewModel;
            }
        }
    }
}