using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Authorizer.Models.Responces
{
    public class BaseResponce<T>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Content { get; set; }
    }
}
