using System.Data.Common;
using System.Configuration;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Factory
{
    class DbConnectionFactory : IDbConnectionFactory
    {
        public DbConnectionFactory( ConnectionStringSettings css )
        {
            _css = css;
        }

        /// <summary>
        /// Create connection to SQL database.
        /// </summary>
        /// <returns>Database connection.</returns>
        public DbConnection CreateConnection()
        {
            var factory = DbProviderFactories.GetFactory( _css.ProviderName );
            var dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = _css.ConnectionString;
            return dbConnection;
        }

        readonly ConnectionStringSettings _css;
    }
}
