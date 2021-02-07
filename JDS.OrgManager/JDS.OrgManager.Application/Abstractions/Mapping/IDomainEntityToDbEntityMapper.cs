using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IDomainEntityToDbEntityMapper<TDomainEntity, TDbEntity> where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity
    {
        #region Public Methods

        TDbEntity Map(TDomainEntity source);

        #endregion
    }
}