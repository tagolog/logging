namespace Tagolog.Adapters
{
    internal class NoOpLogAdapter : ITagLogAdapter
    {
        public void InvalidateTags( ITagReadonlyCollection tags, ITagReadonlyCollection builtInTags )
        {
            // Do nothing.
        }
    }
}
