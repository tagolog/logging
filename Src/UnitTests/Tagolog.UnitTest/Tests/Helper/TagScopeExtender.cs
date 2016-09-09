using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.Helper
{
    static class TagScopeExtender
    {
        public static void AssertOnTagCollectionCounts(
            this ITagScope scope,
            int tagsCountExpected,
            int inheritedTagsCountExpected,
            int effectiveTagsCountExpected )
        {
            Assert.AreEqual( tagsCountExpected, scope.Tags.Count );
            Assert.AreEqual( inheritedTagsCountExpected, scope.InheritedTags.Count );
            Assert.AreEqual( effectiveTagsCountExpected, scope.EffectiveTags.Count );
        }
    }
}
