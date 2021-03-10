// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;

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