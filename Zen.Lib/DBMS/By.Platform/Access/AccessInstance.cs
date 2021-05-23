
namespace Zen.DBMS.Access
{
    /// <summary>
    ///  3/18/2010: May not need this instance. Keep it here for now.
    ///    Evaluate import / schema builder code
    /// </summary>
    public sealed class AccessInstance : DataSourceInstance
    {
        public AccessInstance(OleDBContext context, string connString)
            : base(context, connString)
        {
        }
    }
}
