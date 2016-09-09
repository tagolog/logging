using System;

namespace Tagolog.Private.CustomDictionary
{
    class DictionaryChangedEventArgs<TKey, TValue> : EventArgs
    {
        public DictionaryChangedEventArgs( TKey key )
        {
            Key = key;
        }

        public DictionaryChangedEventArgs( TKey key, TValue value )
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; protected set; }

        public TValue Value { get; protected set; }
    }
}
