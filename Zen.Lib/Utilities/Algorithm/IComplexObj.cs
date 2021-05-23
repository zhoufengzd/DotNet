
namespace Zen.Utilities.Algorithm
{
    /// <summary>
    /// Complex Object = Simple Obj + Advanced Data
    ///   Keep base object simple
    ///   Advanced (Exceptional) data ==> Handled by Advanced / Exceptional Handler
    /// </summary>
    public interface IComplexObj
    {
        // Delayed constructor call. Invoked when actual work has started.
        void LoadControllers();
        IAdvanced Advanced { get; set; }
    }

    public interface IAdvanced
    {
    }

    public interface IAdvancedHandler
    {
        void HandleIt<T>(T advancedInfo);
    }

    public interface IExceptionHandler : IAdvancedHandler
    {
    }
}
