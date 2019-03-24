using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.LogAdapterTests
{
    [TestFixture]
    [NonParallelizable]
    class AddInvalidatesLogAdapterTest : LogAdapterTest
    {
        [Test]
        public void LogAdapter_AddInvalidatesLogAdapter()
        {
            Assert.AreEqual( 0, LogAdapter.Tags.Count );

            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );

                Assert.AreEqual( 2, LogAdapter.Tags.Count );
                Assert.AreEqual( "value1", LogAdapter.Tags[ "tag1" ] );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );

                TestWithNestedScope();

                Assert.AreEqual( 2, LogAdapter.Tags.Count );
                Assert.AreEqual( "value1", LogAdapter.Tags[ "tag1" ] );
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
                nestedScope.Tags.Add( "tag3", "value3" );
                nestedScope.Tags.Add( "tag4", "value4" );
                
                Assert.AreEqual( 4, LogAdapter.Tags.Count );
                Assert.AreEqual( "value1", LogAdapter.Tags[ "tag1" ] );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );
                Assert.AreEqual( "value3", LogAdapter.Tags[ "tag3" ] );
                Assert.AreEqual( "value4", LogAdapter.Tags[ "tag4" ] );
            }
        }
    }
}
