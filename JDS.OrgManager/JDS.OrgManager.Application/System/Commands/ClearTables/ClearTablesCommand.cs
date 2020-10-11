using JDS.OrgManager.Application.Abstractions.DbFacades;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JDS.OrgManager.Application.System.Commands.ClearTables
{
    public class ClearTablesCommand : IRequest
    {
        public class ClearTablesCommandHandler : IRequestHandler<ClearTablesCommand>
        {
            private readonly IApplicationWriteDbFacade facade;

            public ClearTablesCommandHandler(IApplicationWriteDbFacade facade)
            {
                this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            }

            public async Task<Unit> Handle(ClearTablesCommand request, CancellationToken cancellationToken)
            {
                await facade.ClearAllTablesAsync();
                return Unit.Value;
            }
        }
    }
}
