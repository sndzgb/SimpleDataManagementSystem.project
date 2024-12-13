using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;
using System.Runtime.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions
{
    public class WebApiCallException : Exception
    {
        public ErrorViewModel? Error { get; set; }


        public WebApiCallException()
        {
        }
        
        public WebApiCallException(ErrorViewModel errorViewModel)
        {
            this.Error = errorViewModel;
        }

        public WebApiCallException(string? message) : base(message)
        {
        }

        public WebApiCallException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WebApiCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
