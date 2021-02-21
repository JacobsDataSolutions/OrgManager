using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
