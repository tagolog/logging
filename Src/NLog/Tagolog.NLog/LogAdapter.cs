namespace Tagolog.NLog
{
    public class LogAdapter : ITagLogAdapter
    {
        public void InvalidateTags( ITagReadonlyCollection tags, ITagReadonlyCollection builtInTags )
        {
            MdcStorage.Save( TagSerializer.TagsToString( tags, builtInTags ) );
        }
    }
}
