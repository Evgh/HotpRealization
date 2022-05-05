namespace HotpServer.Models.Requests
{
    public class TwoFactorConfirmationRequest
    {
        public string Login { get; set; }
        public string Code { get; set; }
    }
}
