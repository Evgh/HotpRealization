using System.ComponentModel.DataAnnotations;

namespace HotpServer.Contracts.Requests
{
    public class AuthenticationRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
