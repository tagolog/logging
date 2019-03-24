using System;
using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
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
