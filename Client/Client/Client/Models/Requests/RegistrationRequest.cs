using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Requests
{
    public class RegistrationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
