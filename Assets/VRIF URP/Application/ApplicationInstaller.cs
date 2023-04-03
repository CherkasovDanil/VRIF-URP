using UnityEngine;
using VRIF_URP;
using VRIF_URP.Pipes;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    [SerializeField] private int maxFrameRate = 72;
    
    public override void InstallBindings()
    {
        OVRPlugin.systemDisplayFrequency = maxFrameRate;
        
        EnvironmentInstaller.Install(Container);
        
        Container
            .Bind<SpawnAndInjectPlayer>()
            .AsSingle()
            .NonLazy();
        
        PlayerInstaller.Install(Container);

        PipeInstaller.Install(Container);
    }
}
