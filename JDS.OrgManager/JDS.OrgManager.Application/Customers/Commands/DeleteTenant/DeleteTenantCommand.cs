using JDS.OrgManager.Application.Abstractions.DbContexts;
using JDS.OrgManager.Application.Abstractions.DbFacades;
using JDS.OrgManager.Application.System;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.Customers.Commands.DeleteTenant
{
    public class DeleteTenantCommand : IRequest<DeleteTenantViewModel>
    {
        public DeleteTenantViewModel DeleteTenant { get; set; }

        public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, DeleteTenantViewModel>
        {
            private readonly IApplicationWriteDbContext context;

            private readonly IApplicationWriteDbFacade facade;

            public DeleteTenantCommandHandler(IApplicationWriteDbFacade facade, IApplicationWriteDbContext context)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
                this.context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<DeleteTenantViewModel> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
            {
                var tenantViewModel = request.DeleteTenant;
                var tenantName = await facade.QueryFirstOrDefaultAsync<string>(@"SELECT TOP 1 Name From Tenants WITH(NOLOCK) WHERE Id = @TenantId", tenantViewModel);
                if (tenantName.ToUpper() != tenantViewModel.ConfirmationCode.ToUpper())
                {
                    throw new ApplicationLayerException("Invalid confirmation provided for delete tenant command.");
                }

                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    var sqlTransaction = transaction.GetDbTransaction();
                    await facade.ExecuteAsync(
                        string.Join("\r\n", from t in facade.GetTenantTables() select $"DELETE FROM {t} WHERE TenantId = @TenantId")
                        + "\r\nDELETE FROM Tenants WHERE Id = @TenantId", tenantViewModel, sqlTransaction);
                    await transaction.CommitAsync();
                }
                return tenantViewModel;
            }
        }
    }
}
