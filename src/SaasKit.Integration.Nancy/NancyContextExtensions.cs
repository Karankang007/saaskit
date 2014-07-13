using Nancy;
using Nancy.Owin;

namespace SaasKit.Integration.Nancy
{
    public static class NancyContextExtensions
    {
        public static TContainer GetRequestContainer<TContainer>(this NancyContext context) where TContainer : class
        {
            var owinEnvironment = context.GetOwinEnvironment();

            if (owinEnvironment == null)
            {
                return null;
            }
            
            object tenant;
            return owinEnvironment.TryGetValue(Constants.RequestContainerKey, out tenant) ? (TContainer)tenant : null;
        }
    }
}
