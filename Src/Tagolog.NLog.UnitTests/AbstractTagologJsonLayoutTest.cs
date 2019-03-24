using System;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tagolog.NLog.UnitTests
{
    abstract class AbstractTagologJsonLayoutTest : AbstractNLogInMemoryTargetTest
    {
        protected void Run( Action<string> runAction, bool checkTagValues = true )
        {
            Run( ( logger, memoryTarget ) =>
            {
                TagScopeManager.ConfigureLogAdapter( "Tagolog.NLog.LogAdapter,Tagolog.NLog" );

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
            } );
        }
    }
}
