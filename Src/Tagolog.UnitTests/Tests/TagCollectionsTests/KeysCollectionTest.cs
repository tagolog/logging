using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class KeysCollectionTest
    {
        [Test]
        public void TagCollections_KeysCollection()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                var keys = scope.Tags.Keys;
                Assert.AreEqual( 0, keys.Count );

                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );

                // Tags collection.
                keys = scope.Tags.Keys;
                Assert.AreEqual( 2, keys.Count );
                Assert.IsTrue( keys.Contains( "tag1" ) );
                Assert.IsTrue( keys.Contains( "tag2" ) );

                // Inherited collection.
                keys = scope.InheritedTags.Keys;
                Assert.AreEqual( 0, keys.Count );

                // Effective collection.
                keys = scope.EffectiveTags.Keys;
                Assert.AreEqual( 2, keys.Count );
                Assert.IsTrue( keys.Contains( "tag1" ) );
                Assert.IsTrue( keys.Contains( "tag2" ) );

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
                nestedScope.Tags.Add( "tag3", "value3" );
                nestedScope.Tags.Add( "tag4", "value4" );

                // Tags collection.
                var keys = nestedScope.Tags.Keys;
                Assert.AreEqual( 2, keys.Count );
                Assert.IsTrue( keys.Contains( "tag3" ) );
                Assert.IsTrue( keys.Contains( "tag4" ) );

                // Inherited collection.
                keys = nestedScope.InheritedTags.Keys;
                Assert.AreEqual( 2, keys.Count );
                Assert.IsTrue( keys.Contains( "tag1" ) );
                Assert.IsTrue( keys.Contains( "tag2" ) );

                // Effective collection.
                keys = nestedScope.EffectiveTags.Keys;
                Assert.AreEqual( 4, keys.Count );
                Assert.IsTrue( keys.Contains( "tag1" ) );
                Assert.IsTrue( keys.Contains( "tag2" ) );
                Assert.IsTrue( keys.Contains( "tag3" ) );
                Assert.IsTrue( keys.Contains( "tag4" ) );
            }
        }
    }
}
