using UnityEngine;
using VRIF_URP;
using VRIF_URP.Game;
using VRIF_URP.Pipes;
using VRIF_URP.SceneObject;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    [SerializeField] private int maxFrameRate = 72;
    
    public override void InstallBindings()
    {
        OVRPlugin.systemDisplayFrequency = maxFrameRate;
        
        Container
            .Bind<SceneHolder>()
            .AsSingle();
        
        Container
            .Bind<SpawnAndInjectPlayerCommand>()
            .AsSingle()
            .NonLazy();
        
        PlayerInstaller.Install(Container);

        PipeInstaller.Install(Container);

        Container
            .Bind<GameController>()
            .AsSingle()
            .NonLazy();
    }
}
