namespace Tagolog
{
    public interface ITagLogAdapter
    {
        void InvalidateTags( ITagReadonlyCollection tags, ITagReadonlyCollection builtInTags );
    }
}
