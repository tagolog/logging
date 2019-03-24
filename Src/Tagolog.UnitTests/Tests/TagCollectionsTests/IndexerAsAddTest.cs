using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class IndexerAsAddTest
    {
        [Test]
        public void TagCollections_IndexerAsAdd()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                Assert.AreEqual( 0, scope.Tags.Count );

                scope.Tags[ "tag" ] = "value";

                Assert.AreEqual( 1, scope.Tags.Count );
                Assert.AreEqual( "value", scope.Tags[ "tag" ] );
            }
        }
    }
}
