using System.Linq;
using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class GetOriginalCaseSensitiveTagKeyTest
    {
        /// <summary>
        /// Tests if tag keys are case insesitive for all collections
        /// exposed by "ITagScope" interface (Tags, InheritedTags, EffectiveTags).
        /// </summary>
        [Test]
        public void TagCollections_GetOriginalCaseSensitiveTagKey()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "TaG", "VaLuE" );

                var pair = scope.Tags.First();
                Assert.AreEqual( "TaG", pair.Key );

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
                var pair = nestedScope.InheritedTags.First();
                Assert.AreEqual( "TaG", pair.Key );

                // Effective collection.
                pair = nestedScope.EffectiveTags.First();
                Assert.AreEqual( "TaG", pair.Key );
            }
        }
    }
}
