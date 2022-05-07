using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Requests
{
    public class AuthenticationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
