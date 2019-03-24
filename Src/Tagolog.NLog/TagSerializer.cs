using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tagolog.NLog
{
    static class TagSerializer
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

            return SerializeByBinaryFormatter( serializableTagCollection );
        }

        public static void TagsFromString(
            string source,
            out IDictionary<string, string> tags,
            out IDictionary<string, string> builtInTags )
        {
            var serializableTagCollection = DeserializeByBinaryFormatter( source );

            tags = SerializableTagsToDictionary( serializableTagCollection.Tags );
            builtInTags = SerializableTagsToDictionary( serializableTagCollection.BuiltInTags );
        }

        static string SerializeByBinaryFormatter( SerializableTagCollection serializableTagCollection )
        {
            var binaryFormatter = new BinaryFormatter();
            using ( var memoryStream = new MemoryStream() )
            {
                binaryFormatter.Serialize( memoryStream, serializableTagCollection );
                return StringByteArrayConverter.ByteArrayToString( memoryStream.GetBuffer() );
            }
        }

        static SerializableTagCollection DeserializeByBinaryFormatter( string hexadecimalString )
        {
            var bytes = StringByteArrayConverter.StringToByteArray( hexadecimalString );

            var binaryFormatter = new BinaryFormatter();
            using ( var memoryStream = new MemoryStream( bytes ) )
            {
                return ( SerializableTagCollection ) binaryFormatter.Deserialize( memoryStream );
            }
        }

        static List<Tag> ToSerializableList( IEnumerable<KeyValuePair<string, string>> tags )
        {
            return tags.Select( tag => new Tag( tag.Key, tag.Value ) ).ToList();
        }

        static IDictionary<string, string> SerializableTagsToDictionary( IEnumerable<Tag> tags )
        {
            return tags.ToDictionary( _ => _.Key, _ => _.Value );
        }
    }
}
