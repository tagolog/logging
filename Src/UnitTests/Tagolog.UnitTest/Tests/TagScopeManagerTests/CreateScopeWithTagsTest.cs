using System.Collections.Generic;
using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.TagScopeManagerTests
{
    [TestFixture]
    class CreateScopeWithTagsTest
    {
        [Test]
        public void TagScopeManager_CreateScope_WithTags()
        {
            using ( var scope = TagScopeManager.CreateScope( new Dictionary<string,string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            } ) )
            {
                scope.AssertOnTagCollectionCounts( 2, 0, 2 );

                using ( var nestedScope = TagScopeManager.CreateScope( new Dictionary<string, string>
                {
                    { "tag3", "value3" },
                    { "tag4", "value4" }
                } ) )
                {
                    nestedScope.AssertOnTagCollectionCounts( 2, 2, 4 );
                }

                scope.AssertOnTagCollectionCounts( 2, 0, 2 );
            }
        }
    }
}
