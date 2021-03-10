// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace JDS.OrgManager.Common.Reflection
{
    public static class ReflectionExtensions
    {
        public static PropertyInfo GetPropertyInfoFromLambda<TInterface, TProperty>(this Expression<Func<TInterface, TProperty>> propertyLambda)
        {
            var interfaceType = typeof(TInterface);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a method, not a property.");
            }

            PropertyInfo propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{propertyLambda.ToString()}' refers to a field, not a property.");
            }
            return propertyInfo;
        }
    }
}