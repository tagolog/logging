using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Tagolog.NLog
{
    [Serializable]
    [XmlRoot( "TagCollection" )]
    public class SerializableTagCollection
    {
        public SerializableTagCollection()
        {
        }

        public SerializableTagCollection( List<SerializableTag> tags, List<SerializableTag> builtInTags )
        {
            Tags = tags;
            BuiltInTags = builtInTags;
        }

        [XmlArray( "Tags" ), XmlArrayItem( "Tag" )]
        public List<SerializableTag> Tags { get; set; }

        [XmlArray( "BuiltInTags" ), XmlArrayItem( "Tag" )]
        public List<SerializableTag> BuiltInTags { get; set; }
    }
}
