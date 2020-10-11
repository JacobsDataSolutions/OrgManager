using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace JDS.OrgManager.Application
{
    public class NotFoundException : ApplicationLayerException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
