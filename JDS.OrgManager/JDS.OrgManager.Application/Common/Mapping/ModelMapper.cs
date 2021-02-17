using JDS.OrgManager.Application.Abstractions.Mapping;
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace JDS.OrgManager.Application.Common.Mapping
{
    public class ModelMapper : IModelMapper
    {
        private readonly IServiceProvider serviceProvider;

        private ConcurrentDictionary<Type, object> mapperLookup = new ConcurrentDictionary<Type, dynamic>();

        public ModelMapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public TDomainEntity MapDbEntityToDomainEntity<TDbEntity, TDomainEntity>(TDbEntity dbEntity)
            where TDbEntity : IDbEntity
            where TDomainEntity : IDomainEntity => GetFactoryMethodInstance<TDbEntity, TDomainEntity>().Invoke(dbEntity);

        public TValueObject MapDbEntityToValueObject<TDbEntity, TValueObject>(TDbEntity dbEntity)
            where TDbEntity : IDbEntity
            where TValueObject : IValueObject => GetFactoryMethodInstance<TDbEntity, TValueObject>().Invoke(dbEntity);

        public TViewModel MapDbEntityToViewModel<TDbEntity, TViewModel>(TDbEntity dbEntity)
            where TDbEntity : IDbEntity
            where TViewModel : IViewModel => GetFactoryMethodInstance<TDbEntity, TViewModel>().Invoke(dbEntity);

        public void MapDbEntityToViewModel<TDbEntity, TViewModel>(TDbEntity dbEntity, TViewModel existing)
            where TDbEntity : IDbEntity
            where TViewModel : IViewModel => GetVoidMethodInstance<TDbEntity, TViewModel>().Invoke(dbEntity, existing);

        public TDbEntity MapDomainEntityToDbEntity<TDomainEntity, TDbEntity>(TDomainEntity domainEntity)
            where TDomainEntity : IDomainEntity
            where TDbEntity : IDbEntity => GetFactoryMethodInstance<TDomainEntity, TDbEntity>().Invoke(domainEntity);

        public void MapDomainEntityToDbEntity<TDomainEntity, TDbEntity>(TDomainEntity domainEntity, TDbEntity existing)
            where TDomainEntity : IDomainEntity
            where TDbEntity : IDbEntity => GetVoidMethodInstance<TDomainEntity, TDbEntity>().Invoke(domainEntity, existing);

        public TViewModel MapDomainEntityToViewModel<TDomainEntity, TViewModel>(TDomainEntity domainEntity)
            where TDomainEntity : IDomainEntity
            where TViewModel : IViewModel => GetFactoryMethodInstance<TDomainEntity, TViewModel>().Invoke(domainEntity);

        public void MapDomainEntityToViewModel<TDomainEntity, TViewModel>(TDomainEntity domainEntity, TViewModel existing)
            where TDomainEntity : IDomainEntity
            where TViewModel : IViewModel => GetVoidMethodInstance<TDomainEntity, TViewModel>().Invoke(domainEntity, existing);

        public TDbEntity MapViewModelToDbEntity<TViewModel, TDbEntity>(TViewModel viewModel)
            where TViewModel : IViewModel
            where TDbEntity : IDbEntity => GetFactoryMethodInstance<TViewModel, TDbEntity>().Invoke(viewModel);

        public void MapViewModelToDbEntity<TViewModel, TDbEntity>(TViewModel viewModel, TDbEntity existing)
            where TViewModel : IViewModel
            where TDbEntity : IDbEntity => GetVoidMethodInstance<TViewModel, TDbEntity>().Invoke(viewModel, existing);

        public TDomainEntity MapViewModelToDomainEntity<TViewModel, TDomainEntity>(TViewModel viewModel)
            where TViewModel : IViewModel
            where TDomainEntity : IDomainEntity => GetFactoryMethodInstance<TViewModel, TDomainEntity>().Invoke(viewModel);

        public TValueObject MapViewModelToValueObject<TViewModel, TValueObject>(TViewModel viewModel)
            where TViewModel : IViewModel
            where TValueObject : IValueObject => GetFactoryMethodInstance<TViewModel, TValueObject>().Invoke(viewModel);

        private Func<TSource, TTarget> GetFactoryMethodInstance<TSource, TTarget>()
        {
            var mapper = GetMapper<TSource, TTarget>();
            return (Func<TSource, TTarget>)Delegate.CreateDelegate(typeof(Func<TSource, TTarget>), mapper, "Map");
        }

        private object GetMapper<TSource, TTarget>()
        {
            var mapperType = (typeof(TSource), typeof(TTarget)) switch
            {
                (Type source, Type target) when typeof(IDbEntity).IsAssignableFrom(source) && typeof(IDomainEntity).IsAssignableFrom(target) =>
                    typeof(IDbEntityToDomainEntityMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IDbEntity).IsAssignableFrom(source) && typeof(IValueObject).IsAssignableFrom(target) =>
                    typeof(IDbEntityToValueObjectMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IDbEntity).IsAssignableFrom(source) && typeof(IViewModel).IsAssignableFrom(target) =>
                    typeof(IDbEntityToViewModelMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IDomainEntity).IsAssignableFrom(source) && typeof(IDbEntity).IsAssignableFrom(target) =>
                    typeof(IDomainEntityToDbEntityMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IDomainEntity).IsAssignableFrom(source) && typeof(IViewModel).IsAssignableFrom(target) =>
                    typeof(IDomainEntityToViewModelMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IViewModel).IsAssignableFrom(source) && typeof(IDbEntity).IsAssignableFrom(target) =>
                    typeof(IViewModelToDbEntityMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IViewModel).IsAssignableFrom(source) && typeof(IDomainEntity).IsAssignableFrom(target) =>
                    typeof(IViewModelToDomainEntityMapper<,>).MakeGenericType(source, target),
                (Type source, Type target) when typeof(IViewModel).IsAssignableFrom(source) && typeof(IValueObject).IsAssignableFrom(target) =>
                    typeof(IViewModelToValueObjectMapper<,>).MakeGenericType(source, target),
                { } => throw new ApplicationLayerException($"Could not get instance of mapper class for source type {typeof(TSource).Name} and destination type {typeof(TTarget).Name}.")
            };

            return mapperLookup.GetOrAdd(mapperType, serviceProvider.GetRequiredService(mapperType));
        }

        private Action<TSource, TTarget> GetVoidMethodInstance<TSource, TTarget>()
        {
            var mapper = GetMapper<TSource, TTarget>();
            return (Action<TSource, TTarget>)Delegate.CreateDelegate(typeof(Action<TSource, TTarget>), mapper, "Map");
        }
    }
}