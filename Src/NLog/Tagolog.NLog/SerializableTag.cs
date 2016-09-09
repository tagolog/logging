using System;
using System.Xml.Serialization;

namespace Tagolog.NLog
{
    [Serializable]
    [XmlRoot( "Tag" )]
    public class SerializableTag
    {
        public SerializableTag()
        {
        }

        public SerializableTag( string key, string value )
        {
            Key = key;
            Value = value;
        }

        // [XmlAttribute( "Key" )]
        public string Key { get; set; }


        // [XmlAttribute( "Value" )]
        public string Value { get; set; }
    }
}
