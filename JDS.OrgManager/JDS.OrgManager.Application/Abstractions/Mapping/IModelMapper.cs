using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;
using JDS.OrgManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IModelMapper
    {
        // ViewModel -> DB Entity
        TDbEntity MapViewModelToDbEntity<TViewModel, TDbEntity>(TViewModel viewModel) where TViewModel : IViewModel where TDbEntity : IDbEntity;

        void MapViewModelToDbEntity<TViewModel, TDbEntity>(TViewModel viewModel, TDbEntity existing) where TViewModel : IViewModel where TDbEntity : IDbEntity;

        // ViewModel -> Value Object
        TValueObject MapViewModelToValueObject<TViewModel, TValueObject>(TViewModel viewModel) where TValueObject : IValueObject where TViewModel : IViewModel;

        // ViewModel -> Domain Entity
        TDomainEntity MapViewModelToDomainEntity<TViewModel, TDomainEntity>(TViewModel viewModel) where TDomainEntity : IDomainEntity where TViewModel : IViewModel;

        // Domain Entity -> ViewModel
        TViewModel MapDomainEntityToViewModel<TDomainEntity, TViewModel>(TDomainEntity domainEntity) where TDomainEntity : IDomainEntity where TViewModel : IViewModel;

        void MapDomainEntityToViewModel<TDomainEntity, TViewModel>(TDomainEntity domainEntity, TViewModel existing) where TDomainEntity : IDomainEntity where TViewModel : IViewModel;

        // Domain Entity -> DB Entity
        TDbEntity MapDomainEntityToDbEntity<TDomainEntity, TDbEntity>(TDomainEntity domainEntity) where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity;
        
        void MapDomainEntityToDbEntity<TDomainEntity, TDbEntity>(TDomainEntity domainEntity, TDbEntity existing) where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity;

        // DB Entity -> ViewModel
        TViewModel MapDbEntityToViewModel<TDbEntity, TViewModel>(TDbEntity dbEntity) where TDbEntity : IDbEntity where TViewModel : IViewModel;

        void MapDbEntityToViewModel<TDbEntity, TViewModel>(TDbEntity dbEntity, TViewModel existing) where TDbEntity : IDbEntity where TViewModel : IViewModel;

        // DB Entity -> Value Object
        TValueObject MapDbEntityToValueObject<TDbEntity, TValueObject>(TDbEntity dbEntity) where TValueObject : IValueObject where TDbEntity : IDbEntity;

        // DB Entity -> Domain Entity
        TDomainEntity MapDbEntityToDomainEntity<TDbEntity, TDomainEntity>(TDbEntity dbEntity) where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity;
    }
}
