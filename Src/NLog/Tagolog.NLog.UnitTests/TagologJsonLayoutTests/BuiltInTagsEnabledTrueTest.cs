using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    class BuiltInTagsEnabledTrueTest : AbstractTagologJsonLayoutTest
    {
        [Test]
        public void Tagolog_NLog_TagologJsonLayout_BuiltInTagsEnabled_True()
        {
            Run( LoggerName, jsonString =>
            {
                Assert.True( jsonString.Contains( "TagologThreadCorrelationId" ) );
               
                dynamic json = JToken.Parse( jsonString );

                var threadCorrelationId = ( string ) json.TagologThreadCorrelationId;
                Assert.That( threadCorrelationId.Length, Is.GreaterThan( 0 ) );
            } );
        }

        const string LoggerName = "BuiltInTagsEnabledTrue";
    }
}
