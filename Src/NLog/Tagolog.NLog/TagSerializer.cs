using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Tagolog.NLog
{
    public static class TagSerializer
    {
        public static string TagsToString(
            IEnumerable<KeyValuePair<string, string>> tags,
            IEnumerable<KeyValuePair<string, string>> builtInTags )
        {
            // Convert dictionaries to serializable lists.
            var serializableTags = ToSerializableList( tags );
            var serializableBuiltInTags = ToSerializableList( builtInTags );

            // Pack serializable lists into single serializable class instance.
            var serializableTagCollection = new SerializableTagCollection( serializableTags, serializableBuiltInTags );

            var serializer = CreateSerializer();
            using ( var writer = new StringWriter() )
            {
                serializer.Serialize( writer, serializableTagCollection );
                return writer.ToString();
            }
        }

        public static void TagsFromString(
            string source,
            out IDictionary<string, string> tags,
            out IDictionary<string, string> builtInTags )
        {
            var serializer = CreateSerializer();
            using ( var reader = new StringReader( source ) )
            {
                var serializableTagCollection = ( SerializableTagCollection ) serializer.Deserialize( reader );

                tags = SerializableTagsToDictionary( serializableTagCollection.Tags );
                builtInTags = SerializableTagsToDictionary( serializableTagCollection.BuiltInTags );
            }
        }

        static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer( typeof ( SerializableTagCollection ) );
        }

        static List<SerializableTag> ToSerializableList( IEnumerable<KeyValuePair<string, string>> tags )
        {
            return tags.Select( tag => new SerializableTag( tag.Key, tag.Value ) ).ToList();
        }

        static IDictionary<string, string> SerializableTagsToDictionary( IEnumerable<SerializableTag> tags )
        {
            return tags.ToDictionary( _ => _.Key, _ => _.Value );
        }
    }
}
