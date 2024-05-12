using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Core.Infrastructure.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}