using Owin;

namespace SaasKit.Bootstrapper
{
    public interface ISaasKitBootstrapper
    {
        void Initialize(IAppBuilder appBuilder);
    }
}
