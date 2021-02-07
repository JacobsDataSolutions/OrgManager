using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Common.Enums
{
    public static class EnumExtensions
    {
        public static TEnum[] EnumerateFlagValues<TEnum>(this TEnum myEnum) where TEnum : struct, Enum
        {
            var result = new List<TEnum>();
            foreach (var flag in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                if (Equals(flag, default(TEnum)))
                {
                    continue;
                }
                if (myEnum.HasFlag(flag))
                {
                    result.Add(flag);
                }
            }
            return result.ToArray();
        }

        public static TEnum[] EnumerateFlagValues2<TEnum>(this TEnum myEnum) where TEnum : struct, Enum
            => Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Skip(1).Where(flag => myEnum.HasFlag(flag)).Select(flag => flag).ToArray();
    }
}
