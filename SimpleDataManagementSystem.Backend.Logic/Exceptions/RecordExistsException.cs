using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Exceptions
{
    public class RecordExistsException : Exception
    {
        public RecordExistsException()
        {
        }

        public RecordExistsException(string? message) : base(message)
        {
        }

        public RecordExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RecordExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
