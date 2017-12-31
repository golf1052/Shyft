using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Shyft
{
    public class ShyftException : Exception
    {
        public HttpResponseMessage ResponseMessage { get; private set; }

        public ShyftException(HttpResponseMessage responseMessage)
        {
            ResponseMessage = responseMessage;
        }
    }
}
