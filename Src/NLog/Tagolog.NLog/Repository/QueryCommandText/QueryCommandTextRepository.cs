using System;
using Tagolog.NLog.Helper;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Repository.QueryCommandText
{
    abstract class QueryCommandTextRepository : IQueryCommandTextRepository
    {
        /// <summary>
        /// Gets SQL query command text for requested command type.
        /// </summary>
        /// <param name="commandType">SQL query command type to get command text for.</param>
        /// <returns>Text of the SQL command.</returns>
        public string GetCommandText( QueryCommandType commandType )
        {
            switch ( commandType )
            {
                case QueryCommandType.ApplicationSelect:
                    return LoadFileContent( "ApplicationSelect.sql.txt" );

                case QueryCommandType.TagInsertAndSelect:
                    return LoadFileContent( "TagInsertAndSelect.sql.txt" );

                case QueryCommandType.MessageInsert:
                    return LoadFileContent( DoGetCommandText( commandType ) );

                case QueryCommandType.MessageAndTagInsert:
                    return LoadFileContent( DoGetCommandText( commandType ) );

                default:
                    throw new ArgumentException( ErrorCommandTypeIsNotSupported( commandType ), "commandType" );
            }
        }

        protected abstract string DoGetCommandText( QueryCommandType commandType );

        protected string ErrorCommandTypeIsNotSupported( QueryCommandType commandType )
        {
            return string.Format( "Command type is not supported. [commandType={0}]", commandType );
        }

        string LoadFileContent( string fileName )
        {
            return EmbeddedResourceHelper.LoadFileContent(
                string.Format( "{0}.{1}", SqlQueryDirectoryUri, fileName ) );
        }

        const string SqlQueryDirectoryUri = "Tagolog.NLog.SqlQuery";
    }
}
