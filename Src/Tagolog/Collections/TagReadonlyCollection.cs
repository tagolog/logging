using System.Diagnostics;
using System.Collections.Generic;
using Tagolog.Helpers;

namespace Tagolog.Collections
{
    [DebuggerDisplay( "Count = {Count}" )]
    [DebuggerTypeProxy( typeof( TagCollectionDebugView ) )]
    class TagReadonlyCollection : ITagReadonlyCollection
    {
        #region Initialization

        public TagReadonlyCollection( ITagReadonlyCollection collection )
        {
            if ( null == collection )
                throw ThrowHelper.ValueCanNotBeNullException( "collection" );

            _collection = collection;
        }

        #endregion // Initialization

        #region Interface implementation

        public string this[ string tagKey ]
        {
            get
            {
                ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
                return _collection[ tagKey ];
            }
        }

        public bool ContainsKey( string tagKey )
        {
            ThrowHelper.ThrowIfTagKeyIsNullEmptyOrWhiteSpace( tagKey );
            return _collection.ContainsKey( tagKey );
        }

        public ICollection<string> Keys
        {
            get
            {
                return _collection.Keys;
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
            return _collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion // "IEnumerable" interface implementation

        readonly ITagReadonlyCollection _collection;
    }
}
