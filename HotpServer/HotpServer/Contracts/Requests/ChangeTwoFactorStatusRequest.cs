using System.ComponentModel.DataAnnotations;

namespace HotpServer.Contracts.Requests
{
    public class ChangeTwoFactorStatusRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string? KeyBase64 { get; set; }
        [Required]
        public bool IsEnabled { get; set; }
    }
}
