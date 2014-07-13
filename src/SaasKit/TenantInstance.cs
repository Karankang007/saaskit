using System;
using System.Collections.Generic;
using System.Linq;

namespace SaasKit
{
    /// <summary>
    /// Represents a running tenant instance.
    /// </summary>
    public class TenantInstance : IDisposable
    {
        private bool isDisposed;

        public Guid Id { get; private set; }
        public ITenant Tenant { get; private set; }

        public IDictionary<string, object> Properties { get; private set; }

        public TenantInstance(ITenant tenant)
        {
            if (tenant == null)
            {
                throw new ArgumentNullException("tenant");
            }

            Tenant = tenant;
            Id = Guid.NewGuid();
            Properties = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return string.Format("Tenant: {0} Instance: {1}", Tenant.Name, Id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    Console.WriteLine("{0} Disposing.", this.ToString());
                    
                    foreach (var disposable in Properties.Values.OfType<IDisposable>())
                    {
                        try
                        {
                            disposable.Dispose();
                        }
                        catch (ObjectDisposedException) { }
                    }
                }

                Properties = null;
                isDisposed = true;
            }
        }
    }
}
