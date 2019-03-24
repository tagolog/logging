using System;
using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.LogAdapterTests
{
    [TestFixture]
    [NonParallelizable]
    class UnableToSetNullAsLogAdapterTest
    {
        [Test]
        public void LogAdapter_UnableToSetNullAsLogAdapter()
        {
            Assert.IsTrue( ExceptionTest.IsThrown<ArgumentNullException>( () =>
            {
                TagScopeManager.LogAdapter = null;
            } ) );
        }
    }
}
