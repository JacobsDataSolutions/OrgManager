using System;
using System.Collections.Generic;
using System.Text;

namespace JDS.OrgManager.Persistence
{
    public static class PersistenceLayerConstants
    {
        public const string ReadDatabaseConnectionStringName = "ApplicationReadDatabase";

        public const string SqlDateType = "date";

        public const string SqlDecimalMoneyType = "decimal(18,4)";

        public const string WriteDatabaseConnectionStringName = "ApplicationWriteDatabase";
    }
}
