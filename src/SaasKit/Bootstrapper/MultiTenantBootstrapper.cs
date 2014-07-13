using Microsoft.Owin;
using Owin;
using System;

namespace SaasKit.Bootstrapper
{
    public abstract class MultiTenantBootstrapper<TContainer> : SaasKitBootstrapper<TContainer>
        where TContainer : class
    {
        private SaasKitConfiguration configuration; // Can we make saaskit dependencies injectable rather than making part of saaskit configuration?
        protected SaasKitEngine engine;
        
        public MultiTenantBootstrapper(SaasKitConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        protected override void ApplicationStartup(TContainer container)
        {
            engine = new SaasKitEngine(configuration);
            engine.TenantInstanceStarting += OnTenantInstanceStarting;
            engine.TenantInstanceShutdown += OnTenantInstanceShutdown;
            
            // Register the engine with the container so it is available throughout application
            RegisterSaasKitEngine(container, engine);
        }

        protected override void RegisterMiddleware(IAppBuilder appBuilder)
        {
            appBuilder.UseHooksAsync(
                before: async env =>
                {
                    var instance = await engine.BeginRequest(new OwinContext(env));
                    TContainer requestContainer = null;

                    if (instance != null)
                    {
                        // set up the per request container
                        requestContainer = CreateRequestContainer(instance.GetContainer<TContainer>());
                        ConfigureRequestContainer(requestContainer);

                        // make the request container available to the rest of the OWIN pipeline
                        // TODO - this needs to be IServiceProvider or just wrapped in one
                        env.Add(Constants.RequestContainerKey, requestContainer);
                    }

                    return requestContainer;
                },
                after: async (container, env) =>
                {
                    await engine.EndRequest(new OwinContext(env));
                    
                    var disposable = container as IDisposable;
                    if (disposable != null)
                    {
                        try
                        {
                            Console.WriteLine("Disposing container {0}", container.GetType().GetProperty("Name").GetValue(container));
                            disposable.Dispose();
                        }
                        catch (ObjectDisposedException) { }
                    }
                }
            );
        }

        /// <summary>
        /// Configure the tenant container.
        /// </summary>
        /// <param name="tenantContainer"></param>
        protected virtual void ConfigureTenantContainer(TContainer tenantContainer)
        {

        }

        /// <summary>
        /// Called when a tenant instance starts
        /// </summary>
        /// <param name="tenantContainer"></param>
        /// <param name="instance"></param>
        protected virtual void TenantInstanceStartup(TContainer tenantContainer, TenantInstance instance)
        {
            // Here we should store the container in the tenant instance properties bag
            // It would also allow the consumer to load additional information into the tenant
        }

        /// <summary>
        /// Called when a tenant instance shuts down
        /// </summary>
        /// <param name="instance"></param>
        protected virtual void TenantInstanceShutdown(TenantInstance instance)
        {

        }

        /// <summary>
        /// Creates the tenant container
        /// </summary>
        /// <returns></returns>
        protected abstract TContainer CreateTenantContainer(TContainer applicationContainer);
        
        /// <summary>
        /// Creates the request container
        /// </summary>
        /// <returns></returns>
        protected abstract TContainer CreateRequestContainer(TContainer tenantContainer);

        /// <summary>
        /// Register the <see cref="SaasKitEngine"/> instance with the container.
        /// </summary>
        /// <param name="container"></param>
        /// <param name="engine"></param>
        protected abstract void RegisterSaasKitEngine(TContainer container, SaasKitEngine engine);

        /// <summary>
        /// Register the tenant instance with the tenant/request container;
        /// </summary>
        /// <param name="tenantContainer"></param>
        /// <param name="instance"></param>
        protected abstract void RegisterTenantInstance(TContainer tenantContainer, TenantInstance instance);

        private void OnTenantInstanceStarting(object sender, TenantInstanceStartingEventArgs args)
        {
            // First we need to create and configure nested container for the tenant
            var tenantContainer = CreateTenantContainer(ApplicationContainer);
            ConfigureTenantContainer(tenantContainer);

            // Register the tenant instance with the container
            RegisterTenantInstance(tenantContainer, args.Instance);

            // Store the container inside the instance
            args.Instance.SetContainer(tenantContainer);

            // Allow hooking into tenant starting event
            TenantInstanceStartup(tenantContainer, args.Instance);
        }

        private void OnTenantInstanceShutdown(object sender, TenantInstanceShutdownEventArgs args)
        {           
            // Allow hooking into tenant shutdown event (before instance is disposed)
            TenantInstanceShutdown(args.Instance);
        }
    }
}
