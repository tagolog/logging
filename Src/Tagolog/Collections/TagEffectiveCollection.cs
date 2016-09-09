using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Tagolog.Helpers;

namespace Tagolog.Collections
{
    [DebuggerDisplay( "Count = {Count}" )]
    [DebuggerTypeProxy( typeof( TagCollectionDebugView ) )]
    class TagEffectiveCollection : ITagReadonlyCollection
    {
        #region Initialization

        public TagEffectiveCollection( ITagReadonlyCollection parent, ITagReadonlyCollection current )
        {
            if ( null == parent )
                throw new ArgumentNullException( "parent", Properties.Resources.ValueCanNotBeNull_ErrorMessage );

            if ( null == current )
                throw new ArgumentNullException( "current", Properties.Resources.ValueCanNotBeNull_ErrorMessage );

            _parent = parent;
            _current = current;
        }

        #endregion // Initialization

        #region Interface implementation

        public string this[ string tagKey ]
        {
            get
            {
                ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );

                if ( _current.ContainsKey( tagKey ) )
                    return _current[ tagKey ];

                if ( _parent.ContainsKey( tagKey ) )
                    return _parent[ tagKey ];

                throw new ArgumentException(
                    string.Format( Properties.Resources.TagKeyWasNotPresentInTagCollection_ErrorMessageTemplate1, tagKey ) );
            }
        }

        public bool ContainsKey( string tagKey )
        {
            ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
            return _current.ContainsKey( tagKey ) || _parent.ContainsKey( tagKey );
        }

        public ICollection<string> Keys
        {
            get
            {
                // Select new keys from current collection that absent in parent one.
                var newKeys = _current.Keys.Where( _ => !_parent.ContainsKey( _ ) );

                // Merge keys from parent collection and new keys from current.
                var keys = new List<string>();
                keys.AddRange( _parent.Keys );
                keys.AddRange( newKeys );

                return keys.AsReadOnly();
            }
        }

        public int Count
        {
            get { return Keys.Count; }
        }

        #endregion // Interface implementation

        #region "IEnumerable" interface implementation

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach ( var key in Keys )
                yield return new KeyValuePair<string, string>( key, this[ key ] );
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion // "IEnumerable" interface implementation

        readonly ITagReadonlyCollection _parent;
        readonly ITagReadonlyCollection _current;
    }
}
