namespace VRIF_URP.Pipes
{
    public class IPipeController
    {
        public PipeView View { get; }
        public IPipeFillingController MoveController { get; }
        public IPipeConnectionController ConnectionController { get; }
    }
}