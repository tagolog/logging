using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class KeysCollectionCaseSensitiveTest
    {
        [Test]
        public void TagCollections_KeysCollectionCaseSensitive()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "TaG", "value" );

                // Tags collection.
                var keys = scope.Tags.Keys;
                Assert.IsTrue( keys.Contains( "TaG" ) );
                Assert.IsFalse( keys.Contains( "tAg" ) );

                // Effective collection.
                keys = scope.EffectiveTags.Keys;
                Assert.IsTrue( keys.Contains( "TaG" ) );
                Assert.IsFalse( keys.Contains( "tAg" ) );

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
                // Inherited collection.
                var keys = nestedScope.InheritedTags.Keys;
                Assert.IsTrue( keys.Contains( "TaG" ) );
                Assert.IsFalse( keys.Contains( "tAg" ) );

                // Effective collection.
                keys = nestedScope.EffectiveTags.Keys;
                Assert.IsTrue( keys.Contains( "TaG" ) );
                Assert.IsFalse( keys.Contains( "tAg" ) );
            }
        }
    }
}
