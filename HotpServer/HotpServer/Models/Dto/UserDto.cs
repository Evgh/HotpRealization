namespace HotpServer.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool IsTwoFactorAuthenticationEnabled { get; set; }
    }
}
