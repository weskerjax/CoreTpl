using System.Transactions;

namespace CoreTpl.Service.Impl
{
    internal static class Utils
    {
        public static TransactionScope Transaction()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead,
            });
        }
    }
}
