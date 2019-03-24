using System;
using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class RemoveThrowsOnTagKeyThatDoesNotExistTest
    {
        [Test]
        public void TagCollections_RemoveThrowsOnTagKeyThatDoesNotExist()
        {
            // ReSharper disable AccessToDisposedClosure

            using ( var scope = TagScopeManager.CreateScope() )
            {
                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    scope.Tags.Remove( "tag_that_does_not_exist" );
                } ) );

                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    scope.Tags.Remove( "tag_that_does_not_exist" );
                } ) );
            }

            // ReSharper restore AccessToDisposedClosure
        }
    }
}
