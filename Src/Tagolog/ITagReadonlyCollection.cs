using System.Collections.Generic;

namespace Tagolog
{
    public interface ITagReadonlyCollection : IEnumerable<KeyValuePair<string, string>>
    {
        string this[ string tagKey ] { get; }
        bool ContainsKey( string tagKey );
        ICollection<string> Keys { get; }
        int Count { get; }
    }
}
