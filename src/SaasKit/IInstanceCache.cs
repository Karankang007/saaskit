using System;

namespace SaasKit
{
    public interface IInstanceCache
    {
        TenantInstance Get(string requestIdentifier);
        void Add(TenantInstance instance, Action<string, TenantInstance> removedCallback);
        void Remove(string instanceId);
    }
}
