using System;
using System.IO;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;

namespace Tagolog.NLog.UnitTests
{
    abstract class AbstractNLogInMemoryTargetTest
    {
        protected abstract string LoggerName { get; }

        protected void Run( Action<Logger,MemoryTarget> actionOnMemoryTarget )
        {
            InitializeNLogConfiguration();

            var loggingRule = LogManager.Configuration.LoggingRules
                .SingleOrDefault( _ => _.LoggerNamePattern == LoggerName );
            Assert.IsNotNull( loggingRule, "NLog logging rule was not found. [LoggingRule.Name={0}]", LoggerName );

            Assert.That( loggingRule.Targets.Count, Is.EqualTo( 1 ),
                "Logging rule should have one and the only target. [LoggingRule.Name={0}]", LoggerName );

            var target = loggingRule.Targets[ 0 ];
            Assert.True( null != target, "NLog MemoryTarget was not found" );
            Assert.True( target is MemoryTarget, "NLog target is not of type MemoryTarget." );

            var logger = LogManager.GetLogger( LoggerName );
            actionOnMemoryTarget.Invoke( logger, ( MemoryTarget ) target );
        }

        protected static void InitializeNLogConfiguration()
        {
            var nlogConfigFile = Path.Combine( TestContext.CurrentContext.TestDirectory, "nlog.config" );

            LogManager.Configuration = new XmlLoggingConfiguration( nlogConfigFile );
            LogManager.ThrowExceptions = true;
            LogManager.Configuration.Reload();

            Assert.That( LogManager.Configuration.AllTargets.Count, Is.EqualTo( 7 ),
                "NLog targets count is not expected." );
            Assert.That( LogManager.Configuration.LoggingRules.Count, Is.EqualTo( 7 ),
                "NLog logging rules count is not expected." );
        }
    }
}
