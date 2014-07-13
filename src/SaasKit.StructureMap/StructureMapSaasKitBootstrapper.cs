using SaasKit.Bootstrapper;
using StructureMap;
using System;

namespace SaasKit.StructureMap
{
    public class StructureMapSaasKitBootstrapper : MultiTenantBootstrapper<IContainer>
    {
        public StructureMapSaasKitBootstrapper(SaasKitConfiguration configuration) : base(configuration)
        {

        }
        
        protected override IContainer CreateApplicationContainer()
        {
            return new Container();
        }
        
        protected override IContainer CreateTenantContainer(IContainer applicationContainer)
        {
            return applicationContainer.GetNestedContainer();
        }

        protected override IContainer CreateRequestContainer(IContainer tenantContainer)
        {
            return tenantContainer.GetNestedContainer();
        }

        protected override void RegisterSaasKitEngine(IContainer container, SaasKitEngine engine)
        {
            RegisterInstance(container, engine);
        }

        protected override void RegisterTenantInstance(IContainer tenantContainer, TenantInstance instance)
        {
            RegisterInstance(tenantContainer, instance);
        }

        private void RegisterInstance<T>(IContainer container, T instance)
        {
            container.Configure(cfg => cfg.For<T>().Use(instance));
        }

        protected override void TenantInstanceStartup(IContainer tenantContainer, TenantInstance instance)
        {
            Console.WriteLine("Starting container {0}", tenantContainer.Name);
        }
    }
}
