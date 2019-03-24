using NUnit.Framework;

namespace Tagolog.UnitTests.Tests.LogAdapterTests
{
    [TestFixture]
    [NonParallelizable]
    class LogAdapterTest
    {
        [SetUp]
        public void InitTest()
        {
            _adapter = TagScopeManager.LogAdapter;
            TagScopeManager.LogAdapter = new InMemoryCollectionLogAdapter();
        }

        [TearDown]
        public void TearDown()
        {
            TagScopeManager.LogAdapter = _adapter;
        }

        public InMemoryCollectionLogAdapter LogAdapter
        {
            get { return ( InMemoryCollectionLogAdapter ) TagScopeManager.LogAdapter; }
        }

        ITagLogAdapter _adapter;
    }
}
