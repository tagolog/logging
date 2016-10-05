using System;
using System.Collections.Generic;

namespace Tagolog.NLog
{
    [Serializable]
    class SerializableTagCollection
    {
        public SerializableTagCollection()
        {
        }

        public SerializableTagCollection( List<Tag> tags, List<Tag> builtInTags )
        {
            Tags = tags;
            BuiltInTags = builtInTags;
        }

        public List<Tag> Tags { get; set; }
        public List<Tag> BuiltInTags { get; set; }
    }
}
