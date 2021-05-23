
namespace Zen.UIControls
{
    /// <summary>
    /// Define control interface to take object 
    ///    Then controls will be built and positioned automatically.
    /// </summary>
    interface IAutoControl<T>
    {
        int SetObject(T obj);
    }
}
