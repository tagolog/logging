using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class ClearOnParentAffectsNestedScopeTest
    {
        [Test]
        public void TagCollections_ClearOnParentAffectsNestedScope()
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

                    // Clear parent scope tags.
                    scope.Tags.Clear();

                    nestedScope.AssertOnTagCollectionCounts( 2, 0, 2 );
                }

                scope.AssertOnTagCollectionCounts( 0, 0, 0 );
            }
        }
    }
}
