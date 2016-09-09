using System.Collections.Generic;

namespace Tagolog
{
    public interface ITagCollection : IEnumerable<KeyValuePair<string, string>>
    {
        bool ContainsKey( string tagKey );
        ICollection<string> Keys { get; }
        int Count { get; }

        /// <summary>
        /// Adds a tag with the provided key and value to the collection.
        /// </summary>
        /// <param name="tagKey">Key of the tag to add. Tag keys are case insensitive.</param>
        /// <param name="tagValue">Value of the tag to add.</param>
        /// <exception cref="System.ArgumentNullException">Key is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// Tag with the same key already exists in the collection. Tag keys are case insensitive.
        /// </exception>
        void Add( string tagKey, string tagValue );

        string this[ string tagKey ] { get; set; }
        bool Remove( string tagKey );
        void Clear();
    }
}
