using System.Collections.Generic;
using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.LogAdapterTests
{
    [TestFixture]
    [NonParallelizable]
    class CreateScopeInvalidatesLogAdapterTest : LogAdapterTest
    {
        [Test]
        public void LogAdapter_CreateScopeInvalidatesLogAdapter()
        {
            Assert.AreEqual( 0, LogAdapter.Tags.Count );

            using ( TagScopeManager.CreateScope( new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            }) )
            {
                Assert.AreEqual( 2, LogAdapter.Tags.Count );
                Assert.AreEqual( "value1", LogAdapter.Tags[ "tag1" ] );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );

                CreateScopeInvalidatesLogAdapter_TestWithNestedScope();

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
        void CreateScopeInvalidatesLogAdapter_TestWithNestedScope()
        {
            using ( TagScopeManager.CreateScope( new Dictionary<string, string>
                {
                    { "tag3", "value3" },
                    { "tag4", "value4" }
                } ) )
            {
                Assert.AreEqual( 4, LogAdapter.Tags.Count );
                Assert.AreEqual( "value1", LogAdapter.Tags[ "tag1" ] );
                Assert.AreEqual( "value2", LogAdapter.Tags[ "tag2" ] );
                Assert.AreEqual( "value3", LogAdapter.Tags[ "tag3" ] );
                Assert.AreEqual( "value4", LogAdapter.Tags[ "tag4" ] );
            }
        }
    }
}
