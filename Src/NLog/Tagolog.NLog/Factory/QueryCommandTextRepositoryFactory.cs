using System;
using Tagolog.NLog.Interface;
using Tagolog.NLog.Repository.QueryCommandText;

namespace Tagolog.NLog.Factory
{
    static class QueryCommandTextRepositoryFactory
    {
        public static IQueryCommandTextRepository GetQueryCommandTextRepository( SqlProviderSupported provider )
        {
            switch ( provider )
            {
                case SqlProviderSupported.SqlClient:
                    return new SqlServerQueryCommandTextRepository();

                case SqlProviderSupported.Npgsql:
                    return new PostgreSqlQueryCommandTextRepository();

                default:
                    throw new ArgumentException(
                        string.Format( "SQL provider is not supported. [provider={0}]", provider ), "provider" );
            }
        }
    }
}
