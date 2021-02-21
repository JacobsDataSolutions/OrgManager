// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using JDS.OrgManager.Common.Reflection;
using JDS.OrgManager.Domain.Abstractions.Models;
using Mapster;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace JDS.OrgManager.Domain.Models
{
    public abstract class DomainEntity<TEntity> : DomainEntity where TEntity : IDomainEntity
    {
        // Note that this technique is not future-proof and may break with future versions of the framework.
        public TEntity CloneWith2<TProperty>(Expression<Func<TEntity, TProperty>> expr, TProperty value)
        {
            var prop = expr.GetPropertyInfoFromLambda();

            var e = CreateShallowCopy();
            prop.SetValue(e, value);
            return e;
        }

        public TEntity CloneWith(Action<TEntity> action)
        {
            var e = CreateShallowCopy();
            action(e);
            return e;
        }

        public virtual void ValidateAggregate()
        {
        }

        protected TEntity CreateShallowCopy() => (TEntity)MemberwiseClone();

        protected void ValidateNotNull(params DomainEntity[] domainEntities)
        {
            _ = domainEntities ?? throw new ArgumentNullException(nameof(domainEntities));
            if (domainEntities.Any(e => e is null))
            {
                throw new NullReferenceException($"One or more child entities for aggregate of type '{typeof(TEntity).Name}' was null.");
            }
        }
    }
}