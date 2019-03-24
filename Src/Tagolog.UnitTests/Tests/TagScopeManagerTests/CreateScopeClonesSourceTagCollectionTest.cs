using System.Collections.Generic;
using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagScopeManagerTests
{
    [TestFixture]
    [NonParallelizable]
    class CreateScopeClonesSourceTagCollectionTest
    {
        /// <summary>
        /// Tests if modification of initial collection of tags
        /// passed to "TagScopeManager.CreateScope" method
        /// will not affect tags within a newly created scope
        /// since "CreateScope" clones original collection.
        /// </summary>
        [Test]
        public void TagScopeManager_CreateScope_ClonesSourceTagCollection()
        {
            var tagsAsDictionary = new Dictionary<string, string>
            {
                { "tag1", "value1" },
                { "tag2", "value2" }
            };

            // At this point tags from dictionary latches in the internal scope collections.
            using ( var scope = TagScopeManager.CreateScope( tagsAsDictionary ) )
            {
                Assert.AreEqual( 2, scope.Tags.Count );

                // Attempt to modify dictionary with original tags...
                tagsAsDictionary.Add( "tag3", "value3" );

                // ... should not affect scope.
                Assert.AreEqual( 2, scope.Tags.Count );
            }
        }
    }
}
