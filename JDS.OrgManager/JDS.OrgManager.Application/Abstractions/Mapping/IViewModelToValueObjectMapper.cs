using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IViewModelToValueObjectMapper<TViewModel, TValueObject> where TValueObject : IValueObject where TViewModel : IViewModel
    {
        TValueObject Map(TViewModel source);
    }
}