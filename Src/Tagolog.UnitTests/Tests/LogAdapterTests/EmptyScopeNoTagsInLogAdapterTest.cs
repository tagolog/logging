using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.LogAdapterTests
{
    [TestFixture]
    [NonParallelizable]
    class EmptyScopeNoTagsInLogAdapterTestBase : LogAdapterTest
    {
        [Test]
        public void LogAdapter_EmptyScopeNoTagsInLogAdapter()
        {
            using ( TagScopeManager.CreateScope() )
            {
                Assert.AreEqual( 0, LogAdapter.Tags.Count );
            }
        }
    }
}
