using UnityEngine;
using VRIF_URP;
using Zenject;

public class ApplicationInstaller : MonoInstaller
{
    [SerializeField] private int maxFrameRate = 72;
    
    public override void InstallBindings()
    {
        OVRPlugin.systemDisplayFrequency = maxFrameRate;

        EnvironmentInstaller.Install(Container);
        
        Container
            .Bind<ApplicationLauncher>()
            .AsSingle()
            .NonLazy();
    }
}
