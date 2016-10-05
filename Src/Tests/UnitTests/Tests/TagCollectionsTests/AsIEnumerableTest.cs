using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class AsIEnumerableTest
    {
        [Test]
        public void TagCollections_AsIEnumerable()
        {
            using ( var scope = TagScopeManager.CreateScope( new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            } ) )
            {
                scope.Tags.Add( "tag3", "value3" );
                scope.Tags.Add( "tag4", "value4" );

                // Use IEnumerable on Tags collection.
                var dictionary = scope.Tags.ToDictionary( _ => _.Key, _ => _.Value );
                Assert.AreEqual( 4, dictionary.Count );

                // Check if all tags were converted to dictionary through IEnumerable.
                Assert.AreEqual( "value1", dictionary[ "tag1" ] );
                Assert.AreEqual( "value2", dictionary[ "tag2" ] );
                Assert.AreEqual( "value3", dictionary[ "tag3" ] );
                Assert.AreEqual( "value4", dictionary[ "tag4" ] );

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
            using ( var nestedScope = TagScopeManager.CreateScope( new Dictionary<string, string>
                {
                    { "tag5", "value5" },
                    { "tag6", "value6" }
                } ) )
            {
                // Use IEnumerable on InheritedTags collection.
                var dictionary = nestedScope.InheritedTags.ToDictionary( _ => _.Key, _ => _.Value );
                Assert.AreEqual( 4, dictionary.Count );

                // Check if all tags were converted to dictionary through IEnumerable.
                Assert.AreEqual( "value1", dictionary[ "tag1" ] );
                Assert.AreEqual( "value2", dictionary[ "tag2" ] );
                Assert.AreEqual( "value3", dictionary[ "tag3" ] );
                Assert.AreEqual( "value4", dictionary[ "tag4" ] );

                // Use IEnumerable on EffectiveTags collection.
                dictionary = nestedScope.EffectiveTags.ToDictionary( _ => _.Key, _ => _.Value );
                Assert.AreEqual( 6, dictionary.Count );

                // Check if all tags were converted to dictionary through IEnumerable.
                Assert.AreEqual( "value1", dictionary[ "tag1" ] );
                Assert.AreEqual( "value2", dictionary[ "tag2" ] );
                Assert.AreEqual( "value3", dictionary[ "tag3" ] );
                Assert.AreEqual( "value4", dictionary[ "tag4" ] );
                Assert.AreEqual( "value5", dictionary[ "tag5" ] );
                Assert.AreEqual( "value6", dictionary[ "tag6" ] );
            }
        }
    }
}
