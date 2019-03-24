using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class KeysCollectionIsReadonlyTest
    {
        [Test]
        public void TagCollections_KeysCollectionIsReadonly()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags[ "tag" ] = "value";

                var keys = scope.Tags.Keys;
                keys.AssertIsCollectionReadonly();

                keys = scope.InheritedTags.Keys;
                keys.AssertIsCollectionReadonly();

                keys = scope.EffectiveTags.Keys;
                keys.AssertIsCollectionReadonly();

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
                var keys = nestedScope.Tags.Keys;
                keys.AssertIsCollectionReadonly();

                keys = nestedScope.InheritedTags.Keys;
                keys.AssertIsCollectionReadonly();

                keys = nestedScope.EffectiveTags.Keys;
                keys.AssertIsCollectionReadonly();
            }
        }
    }
}
