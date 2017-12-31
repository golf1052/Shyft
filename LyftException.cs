using System;
using System.Collections.Generic;
using System.Text;
using Shyft.Models;

namespace Shyft
{
    public class LyftException<T> : Exception where T : ApiError
    {
        public T Error { get; private set; }

        public LyftException(T error) : base(error.Error)
        {
            Error = error;
        }

        public LyftException(T error, Exception innerException) : base(error.Error, innerException)
        {
            Error = error;
        }
    }
}
