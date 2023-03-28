using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP
{
    public class EnvironmentInstaller : Installer<EnvironmentInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<SceneHolder>()
                .AsSingle();
        }
    }
}