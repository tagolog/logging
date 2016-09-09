using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.Helper
{
    static class CollectionExtender
    {
        public static void AssertIsCollectionReadonly( this ICollection<string> collection )
        {
            Assert.IsTrue( collection.IsReadOnly );

            // Expected result: System.NotSupportedException( "Collection is read-only." )
            Assert.IsTrue( ExceptionTest.IsThrown<NotSupportedException>( () =>
            {
                collection.Add( "newKey" );
            } ) );

            // Expected result: System.NotSupportedException( "Collection is read-only." )
            Assert.IsTrue( ExceptionTest.IsThrown<NotSupportedException>( collection.Clear ) );

            // Expected result: System.NotSupportedException( "Collection is read-only." )
            Assert.IsTrue( ExceptionTest.IsThrown<NotSupportedException>( () =>
            {
                collection.Remove( "anyKey" );
            } ) );
        }
    }
}
