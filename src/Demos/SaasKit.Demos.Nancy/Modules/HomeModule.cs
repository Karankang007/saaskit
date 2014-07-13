using Nancy;
using SaasKit.Integration.Nancy;
using StructureMap;

namespace SaasKit.Demos.Nancy.Modules
{  
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                var instance = Context.GetRequestContainer<IContainer>().GetInstance<TenantInstance>();
                
                var model = new
                {
                    Tenant = instance
                };

                //return View["home", model];

                return instance.ToString();
            };
        }
    }
}