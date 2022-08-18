using System.ComponentModel.DataAnnotations;

namespace HotpServer.Contracts.Requests
{
    public class TwoFactorConfirmationRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
