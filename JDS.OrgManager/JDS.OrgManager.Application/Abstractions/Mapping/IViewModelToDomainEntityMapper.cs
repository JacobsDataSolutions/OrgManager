using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IViewModelToDomainEntityMapper<TViewModel, TDomainEntity> where TDomainEntity : IDomainEntity where TViewModel : IViewModel
    {
        #region Public Methods

        TDomainEntity Map(TViewModel source);

        #endregion
    }
}