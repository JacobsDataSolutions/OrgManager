using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JDS.OrgManager.Persistence
{
    public class PersistenceLayerException : Exception
    {
        public PersistenceLayerException()
        {
        }

        public PersistenceLayerException(string? message) : base(message)
        {
        }

        public PersistenceLayerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PersistenceLayerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
