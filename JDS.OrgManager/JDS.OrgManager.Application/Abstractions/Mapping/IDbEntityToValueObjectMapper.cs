using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IDbEntityToValueObjectMapper<TDbEntity, TValueObject> where TValueObject : IValueObject where TDbEntity : IDbEntity
    {
        TValueObject Map(TDbEntity source);
    }
}