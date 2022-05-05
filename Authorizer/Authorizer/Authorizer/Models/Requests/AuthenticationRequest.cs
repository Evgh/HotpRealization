using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.Models.Requests
{
    public class AuthenticationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
