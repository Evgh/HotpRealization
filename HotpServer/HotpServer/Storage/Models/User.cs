namespace HotpServer.Storage.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsTwoFactorAuthenticationEnabled { get; set; }
        public bool IsTwoFactorConfirmed { get; set; }
        public int HotpCounter { get; set; }
        public string SecretKeyBase64 { get; set; }
    }
}
