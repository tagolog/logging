using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.LogAdapterTests
{
    [TestFixture]
    [NonParallelizable]
    class ClearInvalidatesLogAdapterTest : LogAdapterTest
    {
        [Test]
        public void LogAdapter_ClearInvalidatesLogAdapter()
        {
            Assert.AreEqual( 0, LogAdapter.Tags.Count );

            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags[ "tag1" ] = "value1";
                scope.Tags[ "tag2" ] = "value2";

                TestWithNestedScope();

                scope.Tags.Clear();

                Assert.AreEqual( 0, LogAdapter.Tags.Count );
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
                Assert.AreEqual( 4, LogAdapter.Tags.Count );

                nestedScope.Tags.Clear();

                Assert.AreEqual( 2, LogAdapter.Tags.Count );
                Assert.AreEqual( "value1", LogAdapter.Tags[ "tag1" ] );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );
            }
        }
    }
}
