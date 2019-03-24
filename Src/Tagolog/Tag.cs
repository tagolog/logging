using System;
using System.Diagnostics;

namespace Tagolog
{
    [Serializable]
    [DebuggerDisplay( "[{Key}={Value}]" )]
    public class Tag
    {
        public Tag( string key, string value )
        {
            Key = key;
            Value = value;
        }

        public string Key;

        public string Value;
    }
}
