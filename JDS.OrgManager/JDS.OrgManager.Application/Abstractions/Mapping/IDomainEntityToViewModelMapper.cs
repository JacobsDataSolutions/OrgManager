using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IDomainEntityToViewModelMapper<TDomainEntity, TViewModel> where TDomainEntity : IDomainEntity where TViewModel : IViewModel
    {
        TViewModel Map(TDomainEntity source);

        void Map(TDomainEntity source, TViewModel destination);
    }
}