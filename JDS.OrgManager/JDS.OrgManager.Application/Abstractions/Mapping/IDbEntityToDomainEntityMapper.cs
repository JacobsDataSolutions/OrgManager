using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IDbEntityToDomainEntityMapper<TDbEntity, TDomainEntity> where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity
    {
        TDomainEntity Map(TDbEntity source);
    }
}