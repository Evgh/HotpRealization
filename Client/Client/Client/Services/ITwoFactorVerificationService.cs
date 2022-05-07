using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Services
{
    public interface ITwoFactorVerificationService
    {
        bool IsTwoFactorAuthInitialized(string login);
        string InitializeTwoFactorAuth(string login);
        string GenerareCode(string login);
    }
}
