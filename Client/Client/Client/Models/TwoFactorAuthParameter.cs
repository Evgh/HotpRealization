using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class TwoFactorAuthParameter
    {
        public event Action<bool> OnAuthExecuted;

        public void SetIfAuthSucceessful(bool isSuccessful)
        {
            OnAuthExecuted?.Invoke(isSuccessful);
        }
    }
}
