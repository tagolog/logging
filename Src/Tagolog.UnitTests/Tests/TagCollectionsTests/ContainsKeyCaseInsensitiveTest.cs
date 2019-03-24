using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class ContainsKeyCaseInsensitiveTest
    {
        [Test]
        public void TagCollections_ContainsKeyCaseInsensitive()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "TaG", "VaLuE" );

                Assert.IsTrue( scope.Tags.ContainsKey( "tAg" ) );
                Assert.IsTrue( scope.Tags.ContainsKey( "TaG" ) );
                Assert.IsTrue( scope.Tags.ContainsKey( "tag" ) );
                Assert.IsTrue( scope.Tags.ContainsKey( "TAG" ) );
                
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
                Assert.IsTrue( nestedScope.InheritedTags.ContainsKey( "tAg" ) );
                Assert.IsTrue( nestedScope.InheritedTags.ContainsKey( "TaG" ) );
                Assert.IsTrue( nestedScope.InheritedTags.ContainsKey( "tag" ) );
                Assert.IsTrue( nestedScope.InheritedTags.ContainsKey( "TAG" ) );

                // Effective collection.
                Assert.IsTrue( nestedScope.EffectiveTags.ContainsKey( "tAg" ) );
                Assert.IsTrue( nestedScope.EffectiveTags.ContainsKey( "TaG" ) );
                Assert.IsTrue( nestedScope.EffectiveTags.ContainsKey( "tag" ) );
                Assert.IsTrue( nestedScope.EffectiveTags.ContainsKey( "TAG" ) );
            }
        }
    }
}
