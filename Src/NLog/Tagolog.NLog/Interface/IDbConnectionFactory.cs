using System.Data.Common;

namespace Tagolog.NLog.Interface
{
    interface IDbConnectionFactory
    {
        /// <summary>
        /// Create connection to SQL database.
        /// </summary>
        /// <returns>Database connection.</returns>
        DbConnection CreateConnection();
    }
}
