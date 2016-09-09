using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace Tagolog
{
    /// <summary>
    /// Helper to display tag collections in Visual Studio debugger.
    /// </summary>
    internal sealed class TagCollectionDebugView
    {
        internal TagCollectionDebugView( ITagReadonlyCollection collection )
        {
            if ( collection == null )
                throw new ArgumentNullException( "collection" );

            _collection = collection;
        }

        [DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
        internal KeyValuePair<string, string>[] Items
        {
            get
            {
                return _collection.Keys.Select( _ => new KeyValuePair<string, string>( _, _collection[ _ ] ) ).ToArray();
            }
        }

        readonly ITagReadonlyCollection _collection;
    }
}
