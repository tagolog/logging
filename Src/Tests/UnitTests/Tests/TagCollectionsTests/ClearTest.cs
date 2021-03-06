﻿using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class ClearTest
    {
        [Test]
        public void TagCollections_Clear()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.Tags.Add( "tag1", "value1" );
                scope.Tags.Add( "tag2", "value2" );

                scope.AssertOnTagCollectionCounts( 2, 0, 2 );

                scope.Tags.Clear();

                scope.AssertOnTagCollectionCounts( 0, 0, 0 );
            }
        }
    }
}
