using System;

namespace Tagolog.NLog.Factory
{
    static class SqlProviderSelector
    {
        public static SqlProviderSupported GetProvider( string providerName )
        {
            switch ( providerName )
            {
                case "System.Data.SqlClient":
                    return SqlProviderSupported.SqlClient;

                case "Npgsql":
                    return SqlProviderSupported.Npgsql;

                default:
                    throw new ArgumentException(
                        string.Format( "SQL provider is not supported. [providerName={0}]", providerName ), "providerName" );
            }
        }
    }
}
