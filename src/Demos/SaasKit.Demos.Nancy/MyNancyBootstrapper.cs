using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.StructureMap;
using SaasKit.Integration.Nancy;
using StructureMap;

namespace SaasKit.Demos.Nancy
{
    public class MyNancyBootstrapper : StructureMapNancyBootstrapper
    {

        private IContainer container;

        public MyNancyBootstrapper(IContainer container)
        {
            this.container = container;
        }

        protected override IContainer GetApplicationContainer()
        {
            return container;
        }

        protected override void ApplicationStartup(IContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            this.Conventions.ViewLocationConventions.Add((viewName, model, context) =>
            {
                var requestContainer = context.Context.GetRequestContainer<IContainer>();
                var instance = requestContainer.GetInstance<TenantInstance>();
                return string.Concat("views/", instance.Tenant.Name, "/", viewName);
            });
        }

        //protected override void ConfigureRequestContainer(IContainer container, NancyContext context)
        //{
        //    var requestContainer = context.GetRequestContainer<IContainer>();
        //    context.Items["StructureMap.IContainerBootstrapperChildContainer"] = requestContainer;           
            
        //    base.ConfigureRequestContainer(requestContainer, context);
        //}

        protected override void ConfigureRequestContainer(IContainer container, NancyContext context)
        {
            var requestContainer = context.GetRequestContainer<IContainer>();

            if (requestContainer != null)
            {
                context.Items["StructureMap.IContainerBootstrapperChildContainer"] = requestContainer;
                base.ConfigureRequestContainer(requestContainer, context);
            }
            else
            {
                base.ConfigureRequestContainer(container, context);
            }
        }
    }
}
