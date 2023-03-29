using VRIF_URP.Player;
using Zenject;

namespace VRIF_URP
{
    public class PlayerInstaller : Installer<PlayerInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<PlayerInputConfig>()
                .FromScriptableObjectResource("PlayerInputConfig")
                .AsSingle()
                .NonLazy();
        
            Container
                .Bind<PlayerMovementController>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<RayVisualizer>()
                .AsSingle()
                .NonLazy();
        }
    }
}