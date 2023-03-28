using UnityEngine;
using VRIF_URP.Command;
using VRIF_URP.Player;
using VRIF_URP.Room;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP
{
    public class LaunchCommand: Command.Command
    {
        private readonly IInstantiator _instantiator;
        private readonly SceneHolder _sceneHolder;

        public LaunchCommand(
            IInstantiator instantiator,
            SceneHolder sceneHolder)
        {
            _instantiator = instantiator;
            _sceneHolder = sceneHolder;
        }
        
        public override CommandResult Execute()
        {
            var player = _instantiator.InstantiatePrefabResourceForComponent<PlayerView>("PlayerView");
            _sceneHolder.Add<PlayerView>(player);

            var room = Resources.Load<RoomView>("RoomView");
            _sceneHolder.Add<RoomView>(room);

            return base.Execute();
        }
    }
}