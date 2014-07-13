using Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaasKit.Bootstrapper
{
    public abstract class SaasKitBootstrapper<TContainer> : ISaasKitBootstrapper, IDisposable
        where TContainer : class
    {
        private bool initialized;
        private bool disposed;

        /// <summary>
        /// Gets the global application container.
        /// </summary>
        protected TContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// Initializes SaasKit and registers the SaasKit middleware in the OWIN pipeline
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Initialize(IAppBuilder appBuilder)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException("appBuilder");
            }

            ApplicationContainer = CreateApplicationContainer();

            ConfigureApplicationContainer(ApplicationContainer);

            ApplicationStartup(ApplicationContainer);

            RegisterMiddleware(appBuilder);

            initialized = true;
        }

        /// <summary>
        /// Disposes the bootstrapper
        /// </summary>
        public void Dispose()
        {
            if (!initialized)
            {
                return;
            }

            Dispose(true);
        }

        /// <summary>
        /// Configure the application container with additional registrations
        /// </summary>
        /// <param name="container"></param>
        protected virtual void ConfigureApplicationContainer(TContainer container)
        {

        }

        /// <summary>
        /// Called when the application first starts, after the application container has been configured.
        /// </summary>
        /// <param name="container"></param>
        protected virtual void ApplicationStartup(TContainer container)
        {

        }

        /// <summary>
        /// Configures the request container with additional registrations.
        /// </summary>
        /// <param name="requestContainer"></param>
        protected virtual void ConfigureRequestContainer(TContainer requestContainer)
        {

        }

        /// <summary>
        /// Register middleware into the OWIN pipeline.
        /// </summary>
        /// <param name="appBuilder"></param>
        protected virtual void RegisterMiddleware(IAppBuilder appBuilder)
        {

        }

        /// <summary>
        /// Disposes the application container
        /// </summary>
        protected virtual void DisposeResources()
        {
            var container = this.ApplicationContainer as IDisposable;

            if (container == null)
            {
                return;
            }

            try
            {
                container.Dispose();
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Create the application container
        /// </summary>
        /// <returns></returns>
        protected abstract TContainer CreateApplicationContainer();

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DisposeResources();
                }
                
                disposed = true;
            }
        }

        private void TryDispose(IDisposable disposable)
        {
            if (disposable != null)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (ObjectDisposedException)
                {

                }
            }
        }
    }
}
