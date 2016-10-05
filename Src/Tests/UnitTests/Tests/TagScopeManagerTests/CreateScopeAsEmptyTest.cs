using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.TagScopeManagerTests
{
    [TestFixture]
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
