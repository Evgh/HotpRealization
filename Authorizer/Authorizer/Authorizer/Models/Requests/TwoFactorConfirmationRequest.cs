using System;
using System.Collections.Generic;
using System.Text;

namespace Authorizer.Models.Requests
{
    public class TwoFactorConfirmationRequest
    {
        public string Login { get; set; }
        public string Code { get; set; }
    }
}
