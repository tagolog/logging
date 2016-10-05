using System;
using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.TagCollectionsTests
{
    [TestFixture]
    class ApplicationLevelTagsTest
    {
        [Test]
        [Ignore( "Need to be refactored" )]
        public void TagCollections_ApplicationLevelTags()
        {
            TagScopeManager.Tags.Add( "qwe", "rty"  );
            TagScopeManager.Tags.Clear();
            throw new NotImplementedException();
        }
    }
}
