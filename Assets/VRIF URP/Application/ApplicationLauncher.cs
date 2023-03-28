using Zenject;

namespace VRIF_URP
{
    public class ApplicationLauncher
    {
        public ApplicationLauncher(IInstantiator instantiator)
        {
            instantiator.Instantiate<LaunchCommand>().Execute();
        }
    }
}