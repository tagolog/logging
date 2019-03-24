using System.Collections.Generic;
using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class CountTest
    {
        [Test]
        public void TagCollections_Count()
        {
            CreateEmptyScope();
            CreateScopeWithTags();
        }

        static void CreateEmptyScope()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                Assert.AreEqual( 0, scope.Tags.Count );

                scope.Tags.Add( "tag1", "value1" );
                Assert.AreEqual( 1, scope.Tags.Count );

                scope.Tags.Add( "tag2", "value2" );
                Assert.AreEqual( 2, scope.Tags.Count );
            }
        }

        static void CreateScopeWithTags()
        {
            using ( var scope = TagScopeManager.CreateScope( new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            } ) )
            {
                Assert.AreEqual( 2, scope.Tags.Count );

                scope.Tags.Add( "tag3", "value3" );
                Assert.AreEqual( 3, scope.Tags.Count );

                scope.Tags.Add( "tag4", "value4" );
                Assert.AreEqual( 4, scope.Tags.Count );
            }
        }
    }
}
