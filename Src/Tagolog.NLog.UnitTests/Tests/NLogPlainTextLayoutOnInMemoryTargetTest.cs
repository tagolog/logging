using System;
using NUnit.Framework;

namespace Tagolog.NLog.UnitTests.Tests
{
    [TestFixture]
    [Category( "NLog" )]
    [NonParallelizable]
    class NLogPlainTextLayoutOnInMemoryTargetTest : AbstractNLogInMemoryTargetTest
    {
        protected override string LoggerName { get; } = "PlainTextLayoutOnInMemoryTarget";

        [Test]
        public void Tagolog_NLog_PlainTextLayout_On_InMemoryTarget()
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
                Assert.True( loggedText.Contains( uniqueId ), "Logged message should contains unique id." );
            } );
        }
    }
}
