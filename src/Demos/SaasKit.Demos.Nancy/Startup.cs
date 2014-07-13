using Owin;
using SaasKit.Bootstrapper;
using SaasKit.StructureMap;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace SaasKit.Demos.Nancy
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();

            app.UseSaasKit(BootSaasKit(container))
               //.UseWebApi(ConfigureWebApi())
               .UseNancy(cfg => cfg.Bootstrapper = new MyNancyBootstrapper(container));
        }

        private ISaasKitBootstrapper BootSaasKit(IContainer container)
        {
            var config = new SaasKitConfiguration
            {
                TenantResolver = new MyResolver(),
                Logger = msg => Console.WriteLine(msg)
            };

            var instanceStore = new DefaultInstanceStore(
                new InstanceLifetimeOptions { 
                    Lifetime =  TimeSpan.FromSeconds(30),
                    UseSlidingExpiration = true
                }
            );

            return new StructureMapSaasKitBootstrapper(config);
        }

        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute("Default", "api/{controller}", new { controller = "home" });

            return config;
        }
    }

    public class MyResolver : ITenantResolver
    {
        public Task<ITenant> Resolve(string tenantIdentifier)
        {
            var tenants = new Dictionary<string, Tenant>
            {
                { "localhost", new Tenant { Name = "Tenant1", RequestIdentifiers = new[] { "localhost" }}},
                { "dev.local", new Tenant { Name = "Tenant2", RequestIdentifiers = new[] { "dev.local" }}},

            };
            
            return Task.FromResult<ITenant>(tenants[tenantIdentifier]);
        }
    }
}