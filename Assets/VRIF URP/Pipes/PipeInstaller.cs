using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeInstaller : Installer<PipeInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<PipeSpawner>()
                .AsSingle();

            Container
                .BindFactory<Pipe, Pipe.Factory>()
                .FromComponentInNewPrefabResource("PipeView")
                .UnderTransformGroup("PipeObj");
        }
    }
}