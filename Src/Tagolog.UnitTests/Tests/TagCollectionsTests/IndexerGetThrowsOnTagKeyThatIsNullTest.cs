using System;
using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class IndexerGetThrowsOnTagKeyThatIsNullTest
    {
        [Test]
        public void TagCollections_IndexerGet_ThrowsOnTagKeyThatIsNull()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "tag", "value" );

                // Access to existing tag. Expected result: success.
                Assert.AreEqual( "value", scope.Tags[ "tag" ] );

                // Access to tag that does not exist.
                // Expected result: ArgumentNullException( "Tag key cannot be null." )
                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
                {
                    Assert.IsNull( scope.Tags[ null ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
                {
                    Assert.IsNull( scope.InheritedTags[ null ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
                {
                    Assert.IsNull( scope.EffectiveTags[ null ] );
                } ) );

                TestWithNestedScope();
            }
        }

        /// <summary>
        /// This fragment implemented as a separate method
        /// to isolate nested scope from parent scope
        /// and prevent some really annoying problems with
        /// mess with acident "scope" and "nestedScope" calls
        /// while they are in one method.
        /// </summary>
        static void TestWithNestedScope()
        {
            using ( var nestedScope = TagScopeManager.CreateScope() )
            {
                // Expected result: ArgumentNullException( "Tag key cannot be null." )
                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
                {
                    Assert.IsNull( nestedScope.Tags[ null ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
                {
                    Assert.IsNull( nestedScope.InheritedTags[ null ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
                {
                    Assert.IsNull( nestedScope.EffectiveTags[ null ] );
                } ) );
            }
        }
    }
}
