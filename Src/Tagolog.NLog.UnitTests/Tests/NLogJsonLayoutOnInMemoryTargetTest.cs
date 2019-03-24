using System;
using NUnit.Framework;

namespace Tagolog.NLog.UnitTests.Tests
{
    [TestFixture]
    [Category( "NLog" )]
    [NonParallelizable]
    class NLogJsonLayoutOnInMemoryTargetTest : AbstractNLogInMemoryTargetTest
    {
        protected override string LoggerName { get; } = "JsonLayoutOnInMemoryTarget";

        [Test]
        public void Tagolog_NLog_JsonLayout_On_InMemoryTarget()
        {
            Run( ( logger, memoryTarget ) =>
            {
                var uniqueId = Guid.NewGuid().ToString( "D" );
                var message = $"Log message. [UniqueId={uniqueId}]";
                logger.Debug( message );

                Assert.That( memoryTarget.Logs.Count, Is.EqualTo( 1 ),
                    "The only row should be logged in the MemoryTarget." );

                var loggedText = memoryTarget.Logs[ 0 ];
                Assert.IsNotNull( loggedText, "Logged message should not be null." );
                Assert.IsNotEmpty( loggedText, "Logged message should not be empty." );
                Assert.True( loggedText.StartsWith( "{" ), "Logged message should look like Json." );
                Assert.True( loggedText.Contains( uniqueId ), "Logged message should contains unique id." );
            } );
        }
    }
}
