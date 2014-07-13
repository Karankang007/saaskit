using System;

namespace SaasKit
{
    public static class TenantInstanceExtensions
    {
        private const string TenantContainerKey = "container";
        
        public static TContainer GetContainer<TContainer>(this TenantInstance instance) where TContainer : class
        {
            return instance.GetProperty<TContainer>(TenantContainerKey);
        }

        public static void SetContainer<TContainer>(this TenantInstance instance, TContainer container) where TContainer : class
        {
            instance.Properties.Add(TenantContainerKey, container);
        }

        public static T GetProperty<T>(this TenantInstance instance, string propertyKey)
        {
            if (string.IsNullOrEmpty(propertyKey))
            {
                throw new ArgumentNullException("propertyKey");
            }

            object val;
            if (instance.Properties.TryGetValue(propertyKey, out val))
            {
                return (T)val;
            }

            return default(T);
        }
    }
}
