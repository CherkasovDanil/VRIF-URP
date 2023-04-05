﻿using VRIF_URP.Pipes;
using VRIF_URP.Player;
using Zenject;

namespace VRIF_URP
{
    public class PlayerInstaller : Installer<PlayerInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<VectorDirectionController>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<PlayerInputConfig>()
                .FromScriptableObjectResource("PlayerInputConfig")
                .AsSingle()
                .NonLazy();
        
            Container
                .Bind<PlayerInputController>()
                .AsSingle()
                .NonLazy();
            
            Container
                .Bind<PlayerMovementController>()
                .AsSingle()
                .NonLazy();
        }
    }
}