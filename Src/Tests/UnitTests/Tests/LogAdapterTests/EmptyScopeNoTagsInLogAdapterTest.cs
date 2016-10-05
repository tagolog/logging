using NUnit.Framework;

namespace Tagolog.UnitTest.Tests.LogAdapterTests
{
    [TestFixture]
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
