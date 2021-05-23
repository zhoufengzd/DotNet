
namespace Zen.DBMS
{
    public interface ILoadEventListener
    {
        void OnBulkCopyCompleted();
        void OnBulkUpdateCompleted();
        void OnRowUpdateError(System.Data.DataRow row, string errorMsg);
    }
}
