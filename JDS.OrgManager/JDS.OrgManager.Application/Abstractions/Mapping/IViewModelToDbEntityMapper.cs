using JDS.OrgManager.Application.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IViewModelToDbEntityMapper<TViewModel, TDbEntity> where TViewModel : IViewModel where TDbEntity : IDbEntity
    {
        TDbEntity Map(TViewModel source);

        void Map(TViewModel source, TDbEntity destination);
    }
}