namespace Tagolog.NLog.Interface
{
    /// <summary>
    /// Possible SQL query command types.
    /// </summary>
    enum QueryCommandType
    {
        ApplicationSelect,
        TagInsertAndSelect,
        MessageInsert,
        MessageAndTagInsert
    }
}
