using System.Collections.Generic;
using System.Linq;

namespace Tagolog.UnitTest.Model
{
    public class TagCollection : Dictionary<string, string>
    {
        /// <summary>
        /// Helper to create collection instance from IDictionary or IEnumerable interfaces.
        /// </summary>
        /// <param name="src">Source collection presented by "IEnumerable" interface.</param>
        /// <returns>Collection of type "TagCollection".</returns>
        public static TagCollection FromIEnumerable( IEnumerable<KeyValuePair<string, string>> src )
        {
            var tags = new TagCollection();
            src.ToList().ForEach( _ => tags.Add( _.Key, _.Value ) );
            return tags;
        }

        /// <summary>
        /// Push collections together.
        /// </summary>
        /// <param name="tags1">First collection.</param>
        /// <param name="tags2">Second collection.</param>
        /// <returns>Collection that contains tags from first and second collection.</returns>
        public static TagCollection Add( TagCollection tags1, TagCollection tags2 )
        {
            var tags = new TagCollection();
            tags1.ToList().ForEach( _ => tags.Add( _.Key, _.Value ) );
            tags2.ToList().ForEach( _ => tags.Add( _.Key, _.Value ) );
            return tags;
        }
    }
}
