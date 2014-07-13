using System;

namespace SaasKit
{
    public class TenantInstanceShutdownEventArgs : EventArgs
    {
        public TenantInstance Instance { get; private set; }

        public TenantInstanceShutdownEventArgs(TenantInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Instance = instance;
        }
    }
}
