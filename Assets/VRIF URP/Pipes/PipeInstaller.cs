﻿using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeInstaller : Installer<PipeInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<PipeService>()
                .AsSingle();
            
            Container
                .BindFactory<PipeView, PipeView.Factory>()
                .FromComponentInNewPrefabResource("Pipe")
                .UnderTransformGroup("PipeObj");

            Container
                .Bind<PipeConfig>()
                .FromScriptableObjectResource("PipeConfig")
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<PipeMovementController>()
                .AsSingle()
                .NonLazy();
        }
    }
}