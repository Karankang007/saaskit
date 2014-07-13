using System.Net.Http;
using System.Web.Http;

namespace SaasKit.Demos.Nancy.Controllers
{
    public class HomeController : ApiController
    {
        public string Get()
        {
            var currentTenant = "foo";//Request.GetOwinContext().GetTenantInstance();
            return "Current Tenant: " + currentTenant;
        }
    }
}
