
using VRIF_URP.Command;
using VRIF_URP.Player;
using VRIF_URP.Room;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP
{
    public class InjectPlayer: Command.Command
    {
        private readonly IInstantiator _instantiator;
        private readonly SceneHolder _sceneHolder;

        public InjectPlayer(
            IInstantiator instantiator,
            SceneHolder sceneHolder)
        {
            _instantiator = instantiator;
            _sceneHolder = sceneHolder;
        }
        
        public override CommandResult Execute()
        {
            var player = _instantiator.InstantiatePrefabResourceForComponent<PlayerView>("PlayerView");
            var room = _instantiator.InstantiatePrefabResourceForComponent<RoomView>("RoomView");
            _sceneHolder.Add<PlayerView>(player);
            _sceneHolder.Add<RoomView>(room);
            
            return base.Execute();
        }
    }
}