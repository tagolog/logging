namespace Tagolog.NLog.Interface
{
    /// <summary>
    /// Interface of a repository that holds SQL query command text for appropriates DBMS.
    /// </summary>
    interface IQueryCommandTextRepository
    {
        /// <summary>
        /// Gets SQL query command text for requested command type.
        /// </summary>
        /// <param name="commandType">SQL query command type to get command text for.</param>
        /// <returns>Text of the SQL command.</returns>
        string GetCommandText( QueryCommandType commandType );
    }
}
