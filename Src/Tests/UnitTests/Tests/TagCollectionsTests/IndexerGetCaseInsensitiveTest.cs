using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class IndexerGetCaseInsensitiveTest
    {
        [Test]
        public void TagCollections_IndexerGet_CaseInsensitive()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "TaG", "VaLuE" );

                Assert.AreEqual( "VaLuE", scope.Tags[ "tAg" ] );
                Assert.AreEqual( "VaLuE", scope.Tags[ "TaG" ] );
                Assert.AreEqual( "VaLuE", scope.Tags[ "tag" ] );
                Assert.AreEqual( "VaLuE", scope.Tags[ "TAG" ] );

                TestWithNestedScope();

                scope.Tags.Clear();
                scope.AssertOnTagCollectionCounts( 0, 0, 0 );

                scope.Tags[ "TaG" ] = "VaLuE";

                Assert.AreEqual( "VaLuE", scope.Tags[ "tAg" ] );
                Assert.AreEqual( "VaLuE", scope.Tags[ "TaG" ] );
                Assert.AreEqual( "VaLuE", scope.Tags[ "tag" ] );
                Assert.AreEqual( "VaLuE", scope.Tags[ "TAG" ] );

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
                // Test on Inherited and Effective collections also.

                // Inherited collection.
                Assert.AreEqual( "VaLuE", nestedScope.InheritedTags[ "tAg" ] );
                Assert.AreEqual( "VaLuE", nestedScope.InheritedTags[ "TaG" ] );
                Assert.AreEqual( "VaLuE", nestedScope.InheritedTags[ "tag" ] );
                Assert.AreEqual( "VaLuE", nestedScope.InheritedTags[ "TAG" ] );

                // Effective collection.
                Assert.AreEqual( "VaLuE", nestedScope.EffectiveTags[ "tAg" ] );
                Assert.AreEqual( "VaLuE", nestedScope.EffectiveTags[ "TaG" ] );
                Assert.AreEqual( "VaLuE", nestedScope.EffectiveTags[ "tag" ] );
                Assert.AreEqual( "VaLuE", nestedScope.EffectiveTags[ "TAG" ] );
            }
        }
    }
}
