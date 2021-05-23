
namespace Zen.Utilities.Threading
{
    /// <summary>
    /// Process State.
    /// </summary>
    public enum State
    {
        Stopped = 1,
        Paused = 2,
        Running = 3,

        /// <summary>
        /// Optional. Grey area. In-between state. 
        /// Like stopping but not stopped, starting but not started yet.
        /// </summary>
        Pending = 4,
    }

    public enum ActionRequest
    {
        Unspecified = 0,
        Start = 1,
        Pause = 2,
        Stop = 3,

        /// <summary>
        /// Superior action to handle hanging state.
        /// Disabled, since it's not a normal state action.
        /// </summary>
        //Kill = 4,
    }

    public interface IRunable
    {
        bool Run();
    }

    public interface IControllable
    {
        bool Start();
        bool Stop();
        bool Pause();
        State State { get; }
    }

    /// Statistics
    /// Error
    public interface IProgress
    {
        int Count { get; }
        int Percentage { get; }
    }
 
}
