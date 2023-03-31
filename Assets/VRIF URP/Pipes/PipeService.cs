using System.Collections.Generic;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeService
    {
        private readonly PipeConfig _config;
        private readonly IInstantiator _instantiator;
        
        private Dictionary<int, IPipeController> _enemyStorage = new Dictionary<int, IPipeController>();
    }
}