using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool IsTwoFactorAuthenticationEnabled { get; set; }
    }
}
