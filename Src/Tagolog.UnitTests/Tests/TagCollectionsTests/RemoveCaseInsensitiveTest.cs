using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class RemoveCaseInsensitiveTest
    {
        [Test]
        public void TagCollections_RemoveCaseInsensitive()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "TaG", "value" );
                scope.AssertOnTagCollectionCounts( 1, 0, 1 );

                scope.Tags.Remove( "tAg" );
                scope.AssertOnTagCollectionCounts( 0, 0, 0 );
            }
        }
    }
}
