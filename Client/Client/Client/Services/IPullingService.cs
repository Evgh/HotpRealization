using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Services
{
    public interface IPullingService
    {
        void StartPulling();
        void StopPulling();
    }
}
