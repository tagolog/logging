using System;
using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests.LogAdapterTests
{
    [TestFixture]
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
