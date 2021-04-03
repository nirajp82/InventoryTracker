using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace InventoryTracker.Infrastructure
{
    public class CustomException : Exception
    {
        #region Members
        public HttpStatusCode StatusCode { get; }
        public object Errors { get; }
        #endregion


        #region Constructor
        public CustomException(HttpStatusCode statusCode, object errors = default)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
        #endregion
    }
}
