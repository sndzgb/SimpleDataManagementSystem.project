using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Exceptions
{
    public class NotAllowedException : Exception
    {
        public NotAllowedException()
        {
        }

        public NotAllowedException(string? message) : base(message)
        {
        }

        public NotAllowedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
