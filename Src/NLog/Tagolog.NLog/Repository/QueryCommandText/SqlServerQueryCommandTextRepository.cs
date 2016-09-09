using System;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Repository.QueryCommandText
{
    class SqlServerQueryCommandTextRepository : QueryCommandTextRepository
    {
        protected override string DoGetCommandText( QueryCommandType commandType )
        {
            switch ( commandType )
            {
                case QueryCommandType.MessageInsert:
                    return "MessageInsert_SqlServer.sql.txt";

                case QueryCommandType.MessageAndTagInsert:
                    return "MessageAndTagInsert_SqlServer.sql.txt";

                default:
                    throw new ArgumentException( ErrorCommandTypeIsNotSupported( commandType ), "commandType" );
            }
        }
    }
}
