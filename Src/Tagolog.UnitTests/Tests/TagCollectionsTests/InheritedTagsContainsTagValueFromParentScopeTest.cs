using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class InheritedTagsContainsTagValueFromParentScopeTest
    {
        [Test]
        public void TagCollections_InheritedTagsContainsTagValueFromParentScope()
        {
            const string tagKey = "tag";
            using ( var scope1 = TagScopeManager.CreateScope() )
            {
                scope1.Tags.Add( tagKey, "1" );
                // No tags inherited yet.
                Assert.AreEqual( 0, scope1.InheritedTags.Count );

                using ( var scope2 = TagScopeManager.CreateScope() )
                {
                    scope2.Tags.Add( tagKey, "2" );
                    Assert.AreEqual( 1, scope2.InheritedTags.Count );

                    using ( var scope3 = TagScopeManager.CreateScope() )
                    {
                        scope3.Tags.Add( tagKey, "3" );
                        Assert.AreEqual( 1, scope3.InheritedTags.Count );

                        using ( var scope4 = TagScopeManager.CreateScope() )
                        {
                            scope4.Tags.Add( tagKey, "4" );
                            Assert.AreEqual( 1, scope4.InheritedTags.Count );

                            // Inherited collection contains value from parent scope.
                            Assert.AreEqual( "1", scope2.InheritedTags[ tagKey ] );
                            Assert.AreEqual( "2", scope3.InheritedTags[ tagKey ] );
                            Assert.AreEqual( "3", scope4.InheritedTags[ tagKey ] );
                        }
                    }
                }
            }
        }
    }
}
