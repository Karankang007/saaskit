using SaasKit.Bootstrapper;
using System;

namespace Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseSaasKit(this IAppBuilder appBuilder, ISaasKitBootstrapper bootstrapper)
        {
            if (appBuilder == null)
            {
                throw new ArgumentNullException("appBuilder");
            }

            if (bootstrapper == null)
            {
                throw new ArgumentNullException("bootstrapper");
            }

            bootstrapper.Initialize(appBuilder);
            return appBuilder;
        }
    }
}