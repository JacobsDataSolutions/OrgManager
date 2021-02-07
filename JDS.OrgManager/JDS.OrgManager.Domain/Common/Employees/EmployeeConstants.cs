using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Domain.Common.Employees
{
    public static class EmployeeConstants
    {
        public static readonly DateTime MinimumValidDateOfBirth = new DateTime(1920, 1, 1);

        public static readonly DateTime MinimumValidDateOfHire = new DateTime(1950, 1, 1);
    }
}
