using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Models;
using Mapster;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public abstract class MapperBase<TSource, TDestination> : MapperBase where TSource : notnull where TDestination : notnull
    {
        protected TypeAdapterConfig Config { get; init; } = new TypeAdapterConfig();

        protected MapperBase()
        {
            Configure(
                Config.NewConfig<TSource, TDestination>()
                // Do NOT deep copy object graphs, with the exception of Value Objects (which have value semantics)
                .IgnoreMember((member, side) => side == MemberSide.Destination && !(member.Type.IsValueType || typeof(IValueObject).IsAssignableFrom(member.Type) || member.Type == typeof(string)))
                );
            PerformAdditionalInitialization();
        }

        public virtual TDestination Map(TSource source) => source.Adapt<TDestination>(Config);

        public virtual void Map(TSource source, TDestination destination) => source.Adapt(destination, typeof(TSource), typeof(TDestination), Config);

        protected virtual TypeAdapterSetter<TSource, TDestination> Configure(TypeAdapterSetter<TSource, TDestination> typeAdapterSetter) => typeAdapterSetter;

        protected virtual void PerformAdditionalInitialization() { }
    }
}