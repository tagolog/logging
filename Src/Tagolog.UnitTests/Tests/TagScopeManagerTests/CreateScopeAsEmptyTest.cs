using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagScopeManagerTests
{
    [TestFixture]
    [NonParallelizable]
    class CreateScopeAsEmptyTest
    {
        [Test]
        public void TagScopeManager_CreateScope_AsEmpty()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.AssertOnTagCollectionCounts( 0, 0, 0 );

                using ( var nestedScope = TagScopeManager.CreateScope() )
                {
                    nestedScope.AssertOnTagCollectionCounts( 0, 0, 0 );
                }

                scope.AssertOnTagCollectionCounts( 0, 0, 0 );
            }
        }
    }
}
