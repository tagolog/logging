using NUnit.Framework;
using Tagolog.Adapters;

namespace Tagolog.UnitTest.Tests.LogAdapterTests
{
    [TestFixture]
    class DefaultAdapterIsNoOpAdapterTest
    {
        [Test]
        public void LogAdapter_DefaultAdapterIsNoOpAdapter()
        {
            var adapter = TagScopeManager.LogAdapter;

            Assert.IsNotNull( adapter, "Log adapter cannot be null" );

            Assert.IsTrue( adapter is NoOpLogAdapter,
                "Default log adapter should be of {0} type", typeof( NoOpLogAdapter ).Name  );
        }
    }
}
