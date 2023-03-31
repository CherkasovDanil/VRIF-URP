using Zenject;

namespace VRIF_URP
{
    public class SpawnAndInjectPlayer
    {
        public SpawnAndInjectPlayer(IInstantiator instantiator)
        {
            instantiator.Instantiate<InjectPlayer>().Execute();
        }
    }
}