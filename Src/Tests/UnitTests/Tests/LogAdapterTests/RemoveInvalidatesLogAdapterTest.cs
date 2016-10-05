using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.LogAdapterTests
{
    [TestFixture]
    class RemoveInvalidatesLogAdapterTest : LogAdapterTest
    {
        [Test]
        public void LogAdapter_RemoveInvalidatesLogAdapter()
        {
            Assert.AreEqual( 0, LogAdapter.Tags.Count );

            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags[ "tag1" ] = "value1";
                scope.Tags[ "tag2" ] = "value2";
                Assert.AreEqual( 2, LogAdapter.Tags.Count );

                scope.Tags.Remove( "tag1" );

                Assert.AreEqual( 1, LogAdapter.Tags.Count );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );

                TestWithNestedScope();

                Assert.AreEqual( 1, LogAdapter.Tags.Count );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );
            }

            Assert.AreEqual( 0, LogAdapter.Tags.Count );
        }

        /// <summary>
        /// This fragment implemented as a separate method
        /// to isolate nested scope from parent scope
        /// and prevent some really annoying problems with
        /// mess with acident "scope" and "nestedScope" calls
        /// while they are in one method.
        /// </summary>
        void TestWithNestedScope()
        {
            using ( var nestedScope = TagScopeManager.CreateScope() )
            {
                nestedScope.Tags[ "tag3" ] = "value3";
                nestedScope.Tags[ "tag4" ] = "value4";
                Assert.AreEqual( 3, LogAdapter.Tags.Count );

                nestedScope.Tags.Remove( "tag3" );

                Assert.AreEqual( 2, LogAdapter.Tags.Count );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );
                Assert.AreEqual( "value4", LogAdapter.Tags[ "tag4" ] );
            }
        }
    }
}
