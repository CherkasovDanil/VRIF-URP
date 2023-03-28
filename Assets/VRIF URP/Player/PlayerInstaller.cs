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
                .AsSingle();
        
            Container
                .Bind<PlayerMovementController>()
                .AsSingle()
                .NonLazy();
        }
    }
}