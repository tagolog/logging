using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Tagolog.Helpers;
using Tagolog.Private.CustomDictionary;

namespace Tagolog.Collections
{
    [DebuggerDisplay( "Count = {Count}" )]
    [DebuggerTypeProxy( typeof( TagCollectionDebugView ) )]
    internal class TagCollection : ITagCollection, ITagReadonlyCollection
    {
        #region Interface implementation

        /// <summary>
        /// Adds a tag with the provided key and value to the collection.
        /// </summary>
        /// <param name="tagKey">Key of the tag to add.</param>
        /// <param name="tagValue">Value of the tag to add.</param>
        /// <exception cref="System.ArgumentNullException">Key is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// Tag with the same key already exists in the collection. Tag keys are case insensitive.
        /// </exception>
        public void Add( string tagKey, string tagValue )
        {
            ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
            _tags.Add( NormalizeKey( tagKey ), new Tag( tagKey, tagValue ) );
        }

        public string this[ string tagKey ]
        {
            get
            {
                ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
                ThrowIfNoKeyInCollection( tagKey );
                return _tags[ NormalizeKey( tagKey ) ].Value;
            }
            set
            {
                ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
                _tags[ NormalizeKey( tagKey ) ] = new Tag( tagKey, value );
            }
        }

        public int Count
        {
            get { return _tags.Count; }
        }

        public void Clear()
        {
            _tags.Clear();
        }

        public bool Remove( string tagKey )
        {
            ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
            ThrowIfNoKeyInCollection( tagKey );
            return _tags.Remove( NormalizeKey( tagKey ) );
        }

        public bool ContainsKey( string tagKey )
        {
            ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
            return _tags.ContainsKey( NormalizeKey( tagKey ) );
        }

        public ICollection<string> Keys
        {
            get
            {
                return _tags.Keys
                    .Select( _ => _tags[ _ ].Key )
                    .ToList()
                    .AsReadOnly();
            }
        }

        #endregion // Interface implementation

        #region "IEnumerable" interface implementation

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach ( var tag in _tags.Values )
                yield return new KeyValuePair<string, string>( tag.Key, tag.Value );
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion // "IEnumerable" interface implementation

        public event Action<object, DictionaryChangedEventArgs<string, Tag>> Changed
        {
            add { _tags.Changed += value; }
            remove { _tags.Changed -= value; }
        }

        void ThrowIfNoKeyInCollection( string key )
        {
            var normalizedKey = NormalizeKey( key );
            if ( ! _tags.ContainsKey( normalizedKey ) )
                throw ThrowHelper.TagKeyWasNotPresentInTagCollectionException( key );
        }

        static string NormalizeKey( string key )
        {
            return key.ToLower();
        }

        /// <summary>
        /// Dictionary of tags. Key is normalized to be lower case.
        /// </summary>
        readonly DictionaryWithChangedEvent<string, Tag> _tags = new DictionaryWithChangedEvent<string, Tag>();
    }
}
