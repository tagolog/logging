using System;
using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class RemoveThrowsOnTagKeyThatIsNullTest
    {
        [Test]
        public void TagCollections_RemoveThrowsOnTagKeyThatIsNull()
        {
            // ReSharper disable AccessToDisposedClosure

            using ( var scope = TagScopeManager.CreateScope() )
            {
                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    scope.Tags.Remove( null );
                } ) );

                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    scope.Tags.Remove( null );
                } ) );
            }

            // ReSharper restore AccessToDisposedClosure
        }
    }
}
