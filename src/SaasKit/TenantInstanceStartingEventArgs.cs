using System;

namespace SaasKit
{
    public class TenantInstanceStartingEventArgs : EventArgs
    {
        public TenantInstance Instance { get; private set; }
        
        public TenantInstanceStartingEventArgs(TenantInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this.Instance = instance;
        }
    }
}
