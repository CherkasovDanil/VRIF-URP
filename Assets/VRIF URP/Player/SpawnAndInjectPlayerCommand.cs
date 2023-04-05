using Zenject;

namespace VRIF_URP
{
    public class SpawnAndInjectPlayerCommand
    {
        public SpawnAndInjectPlayerCommand(IInstantiator instantiator)
        {
            instantiator.Instantiate<InjectPlayerCommand>().Execute();
        }
    }
}