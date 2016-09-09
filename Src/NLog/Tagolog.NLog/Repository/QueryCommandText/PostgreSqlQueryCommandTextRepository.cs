using System;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Repository.QueryCommandText
{
    class PostgreSqlQueryCommandTextRepository : QueryCommandTextRepository
    {
        protected override string DoGetCommandText( QueryCommandType commandType )
        {
            switch ( commandType )
            {
                case QueryCommandType.MessageAndTagInsert:
                    return "MessageInsert_PostgreSql.sql.txt";

                default:
                    throw new ArgumentException( ErrorCommandTypeIsNotSupported( commandType ), "commandType" );
            }
        }
    }
}
