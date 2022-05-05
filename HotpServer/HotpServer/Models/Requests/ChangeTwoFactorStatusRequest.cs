namespace HotpServer.Models.Requests
{
    public class ChangeTwoFactorStatusRequest
    {
        public string Login { get; set; }
        public string? KeyBase64 { get; set; } 
        public bool IsEnabled { get; set; }
    }
}
