using System;

namespace Tagolog
{
    [Serializable]
    public class Tag
    {
        public Tag()
        {
        }

        public Tag( string key, string value )
        {
            Key = key;
            Value = value;
        }

        public string Key;

        public string Value;
    }
}
