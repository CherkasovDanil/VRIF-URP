using System;

namespace VRIF_URP.Command
{
    public class Command : ICommand, IDisposable
    {
        public EventHandler Done { get; set; }
        
        public virtual CommandResult Execute()
        {
            return new CommandResult();
        }

        public virtual void Cancel(){ }

        public void Dispose() { }
    }
}