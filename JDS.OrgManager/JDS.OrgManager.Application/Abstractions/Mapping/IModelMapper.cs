// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Application.Abstractions.Models;
using JDS.OrgManager.Domain.Abstractions.Models;

namespace JDS.OrgManager.Application.Abstractions.Mapping
{
    public interface IModelMapper
    {
        // DB Entity -> Domain Entity
        TDomainEntity MapDbEntityToDomainEntity<TDbEntity, TDomainEntity>(TDbEntity dbEntity) where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity;

        // DB Entity -> Value Object
        TValueObject MapDbEntityToValueObject<TDbEntity, TValueObject>(TDbEntity dbEntity) where TValueObject : IValueObject where TDbEntity : IDbEntity;

        // DB Entity -> ViewModel
        TViewModel MapDbEntityToViewModel<TDbEntity, TViewModel>(TDbEntity dbEntity) where TDbEntity : IDbEntity where TViewModel : IViewModel;

        void MapDbEntityToViewModel<TDbEntity, TViewModel>(TDbEntity dbEntity, TViewModel existing) where TDbEntity : IDbEntity where TViewModel : IViewModel;

        // Domain Entity -> DB Entity
        TDbEntity MapDomainEntityToDbEntity<TDomainEntity, TDbEntity>(TDomainEntity domainEntity) where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity;

        void MapDomainEntityToDbEntity<TDomainEntity, TDbEntity>(TDomainEntity domainEntity, TDbEntity existing) where TDomainEntity : IDomainEntity where TDbEntity : IDbEntity;

        // Domain Entity -> ViewModel
        TViewModel MapDomainEntityToViewModel<TDomainEntity, TViewModel>(TDomainEntity domainEntity) where TDomainEntity : IDomainEntity where TViewModel : IViewModel;

        void MapDomainEntityToViewModel<TDomainEntity, TViewModel>(TDomainEntity domainEntity, TViewModel existing) where TDomainEntity : IDomainEntity where TViewModel : IViewModel;

        // ViewModel -> DB Entity
        TDbEntity MapViewModelToDbEntity<TViewModel, TDbEntity>(TViewModel viewModel) where TViewModel : IViewModel where TDbEntity : IDbEntity;

        void MapViewModelToDbEntity<TViewModel, TDbEntity>(TViewModel viewModel, TDbEntity existing) where TViewModel : IViewModel where TDbEntity : IDbEntity;

        // ViewModel -> Domain Entity
        TDomainEntity MapViewModelToDomainEntity<TViewModel, TDomainEntity>(TViewModel viewModel) where TDomainEntity : IDomainEntity where TViewModel : IViewModel;

        // ViewModel -> Value Object
        TValueObject MapViewModelToValueObject<TViewModel, TValueObject>(TViewModel viewModel) where TValueObject : IValueObject where TViewModel : IViewModel;
    }
}