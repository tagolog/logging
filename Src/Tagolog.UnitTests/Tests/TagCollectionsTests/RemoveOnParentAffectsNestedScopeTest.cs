using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class RemoveOnParentAffectsNestedScopeTest
    {
        [Test]
        public void TagCollections_RemoveOnParentAffectsNestedScope()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );

                using ( var nestedScope = TagScopeManager.CreateScope() )
                {
                    nestedScope.Tags.Add( "tag3", "value3" );
                    nestedScope.Tags.Add( "tag4", "value4" );
                    nestedScope.AssertOnTagCollectionCounts( 2, 2, 4 );

                    // Remove tags from parent scope.
                    scope.Tags.Remove( "tag2" );
                    nestedScope.AssertOnTagCollectionCounts( 2, 1, 3 );
                    scope.Tags.Remove( "tag1" );
                    nestedScope.AssertOnTagCollectionCounts( 2, 0, 2 );
                }

                scope.AssertOnTagCollectionCounts( 0, 0, 0 );
            }
        }
    }
}
