using System;
using NLog;
using NLog.Targets;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    class AbstractTagologJsonLayoutTest
    {
        protected static void Run( string loggerName, Action<string> runAction, bool checkTagValues = true )
        {
            var logger = LogManager.GetLogger( loggerName );
            Assert.That( LogManager.Configuration.AllTargets.Count, Is.GreaterThan( 0 ),
                "NLog targets were NOT found in the configuration." );

            var target = LogManager.Configuration.FindTargetByName( loggerName );
            Assert.True( null != target, "NLog MemoryTarget was not found by name '{0}'.", loggerName );
            Assert.True( target is MemoryTarget, "NLog target is not of type MemoryTarget." );

            var memoryTarget = ( MemoryTarget ) target;

            using ( var tagScope = TagScopeManager.CreateScope() )
            {
                tagScope.Tags[ "Tag1" ] = "Value1";
                tagScope.Tags[ "Tag2" ] = "Value2";

                logger.Debug( "Log message." );

                Assert.That( memoryTarget.Logs.Count, Is.EqualTo( 1 ),
                    "The only row should be logged in the MemoryTarget." );

                var jsonString = memoryTarget.Logs[ 0 ];

                try
                {
                    runAction.Invoke( jsonString );

                    dynamic json = JToken.Parse( jsonString );

                    if ( checkTagValues )
                    {
                        var value1 = ( string ) json.Tag1;
                        var value2 = ( string ) json.Tag2;

                        Assert.That( value1, Is.EqualTo( "Value1" ), "Tag1 value was not found." );
                        Assert.That( value2, Is.EqualTo( "Value2" ), "Tag2 value was not found." );
                    }
                }
                catch ( JsonReaderException ex )
                {
                    var message = string.Format( "Wrong Json string: {0}", jsonString );
                    throw new ArgumentException( message, ex );
                }
            }
        }
    }
}
