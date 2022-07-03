// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using ExpressionDebugger;
using JDS.OrgManager.Domain.Abstractions.Models;
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

        //This overload of the Map() method respects polymorphic mapping...
        public virtual TDestination Map(TSource source) => source.Adapt<TSource, TDestination>(Config);

        //This one does not.
        //public virtual TDestination Map(TSource source) => source.Adapt<TDestination>(Config);

        public virtual void Map(TSource source, TDestination destination) => source.Adapt(destination, typeof(TSource), typeof(TDestination), Config);

        public virtual string ToScript(TSource source) => source.BuildAdapter().CreateMapExpression<TDestination>().ToScript();

        protected virtual TypeAdapterSetter<TSource, TDestination> Configure(TypeAdapterSetter<TSource, TDestination> typeAdapterSetter) => typeAdapterSetter;

        protected virtual void PerformAdditionalInitialization()
        {
        }
    }
}