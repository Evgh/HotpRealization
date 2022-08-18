namespace HotpServer.Contracts.Responces
{
    public class UserResponce
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool IsTwoFactorAuthenticationEnabled { get; set; }
    }
}
