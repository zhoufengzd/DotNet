
namespace Zen.Utilities.Service
{
    /// <summary>
    /// Atomic operation per work item
    /// </summary>
    public interface ISvcItem
    {
        void RunWorkItem();
    }
}
