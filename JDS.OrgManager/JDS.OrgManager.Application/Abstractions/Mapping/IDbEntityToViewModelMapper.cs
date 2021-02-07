using JDS.OrgManager.Application.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IDbEntityToViewModelMapper<TDbEntity, TViewModel> where TDbEntity : IDbEntity where TViewModel : IViewModel
    {
        TViewModel Map(TDbEntity source);
    }
}