using System;

namespace VRIF_URP.Command
{
    public interface ICommand
    {
        EventHandler Done { get; set; }
            
        CommandResult Execute();
        
        void Cancel();
    }
    public class CommandResult
    {
        public CommandStatus CommandStatus = CommandStatus.Success;     
    }

    public enum CommandStatus
    {
        Success,
        InProgress,
        Failed
    }
}