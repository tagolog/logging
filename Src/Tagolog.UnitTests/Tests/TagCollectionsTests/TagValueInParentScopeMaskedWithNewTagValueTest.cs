using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    class TagValueInParentScopeMaskedWithNewTagValueTest
    {
        [Test]
        public void TagCollections_TagValueInParentScopeMaskedWithNewTagValue()
        {
            const string tagKey = "tag";
            using ( var scope1 = TagScopeManager.CreateScope() )
            {
                scope1.Tags.Add( tagKey, "1" );
                Assert.AreEqual( "1", scope1.Tags[ tagKey ] );
                Assert.AreEqual( "1", scope1.EffectiveTags[ tagKey ] );

                using ( var scope2 = TagScopeManager.CreateScope() )
                {
                    scope2.Tags.Add( tagKey, "2" );
                    Assert.AreEqual( "2", scope2.Tags[ tagKey ] );
                    Assert.AreEqual( "1", scope2.InheritedTags[ tagKey ] );
                    Assert.AreEqual( "2", scope2.EffectiveTags[ tagKey ] );

                    using ( var scope3 = TagScopeManager.CreateScope() )
                    {
                        scope3.Tags.Add( tagKey, "3" );
                        Assert.AreEqual( "3", scope3.Tags[ tagKey ] );
                        Assert.AreEqual( "2", scope3.InheritedTags[ tagKey ] );
                        Assert.AreEqual( "3", scope3.EffectiveTags[ tagKey ] );
                    }
                }
            }
        }
    }
}
