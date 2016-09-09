using System;
using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class IndexerGetThrowsOnTagKeyThatDoesNotExistTest
    {
        [Test]
        public void TagCollections_IndexerGet_ThrowsOnTagKeyThatDoesNotExistTest()
        {
            // ReSharper disable AccessToDisposedClosure

            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "tag", "value" );

                // Access to existing tag. Expected result: success.
                Assert.AreEqual( "value", scope.Tags[ "tag" ] );

                // Access to tag that does not exist.
                // Expected result: ArgumentException( "The given tag key was not present in the tag collection." )
                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    Assert.IsNull( scope.Tags[ "not_a_tag" ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    Assert.IsNull( scope.InheritedTags[ "not_a_tag" ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    Assert.IsNull( scope.EffectiveTags[ "not_a_tag" ] );
                } ) );

                TestWithNestedScope();
            }

            // ReSharper restore AccessToDisposedClosure
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
            // ReSharper disable AccessToDisposedClosure

            using ( var nestedScope = TagScopeManager.CreateScope() )
            {
                // Access to tag that does not exist.
                // Expected result: ArgumentException( "The given tag key was not present in the tag collection." )
                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    Assert.IsNull( nestedScope.Tags[ "not_a_tag" ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    Assert.IsNull( nestedScope.InheritedTags[ "not_a_tag" ] );
                } ) );

                Assert.IsTrue( ExceptionTest.IsThrown<ArgumentException>( () =>
                {
                    Assert.IsNull( nestedScope.EffectiveTags[ "not_a_tag" ] );
                } ) );
            }

            // ReSharper restore AccessToDisposedClosure
        }
    }
}
