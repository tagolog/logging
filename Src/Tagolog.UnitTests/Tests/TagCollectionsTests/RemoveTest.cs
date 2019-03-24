using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class RemoveTest
    {
        [Test]
        public void TagCollections_Remove()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );
                scope.AssertOnTagCollectionCounts( 2, 0, 2 );

                scope.Tags.Remove( "tag2" );

                scope.AssertOnTagCollectionCounts( 1, 0, 1 );
                Assert.IsTrue( scope.Tags.ContainsKey( "tag1" ) );

                scope.Tags.Remove( "tag1" );
                scope.AssertOnTagCollectionCounts( 0, 0, 0 );
            }
        }
    }
}
