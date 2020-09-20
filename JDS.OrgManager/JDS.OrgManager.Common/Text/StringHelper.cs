using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Common.Text
{
    public static class StringHelper
    {
        public static string GetFullName(string firstName, string middleName, string lastName) =>
            (string.IsNullOrWhiteSpace(middleName) ? $"{lastName}, {firstName}" : $"{lastName}, {middleName} {firstName}").Trim(',', ' ');
    }
}
