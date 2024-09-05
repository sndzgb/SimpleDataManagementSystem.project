using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Exceptions
{
    public class RequiredRecordNotFoundException : Exception
    {
        public RequiredRecordNotFoundException()
        {
        }

        public RequiredRecordNotFoundException(string? message) : base(message)
        {
        }

        public RequiredRecordNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RequiredRecordNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
