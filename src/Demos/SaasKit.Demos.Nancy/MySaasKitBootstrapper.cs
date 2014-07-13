using SaasKit.StructureMap;
using StructureMap;
using System;

namespace SaasKit.Demos.Nancy
{
    public class MySaasKitBootstrapper : StructureMapSaasKitBootstrapper
    {
        private IContainer container;
        
        public MySaasKitBootstrapper(IContainer container, SaasKitConfiguration config) : base(config)
        {
            this.container = container;
        }
        
        protected override IContainer CreateApplicationContainer()
        {
            return container;
        }
    }
}
