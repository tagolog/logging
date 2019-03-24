using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    [NonParallelizable]
    class BuiltInTagsEnabledTrueTest : AbstractTagologJsonLayoutTest
    {
        protected override string LoggerName { get; } = "BuiltInTagsEnabledTrue";

        [Test]
        public void Tagolog_NLog_TagologJsonLayout_BuiltInTagsEnabled_True()
        {
            Run( jsonString =>
            {
                Assert.True( jsonString.Contains( "TagologThreadCorrelationId" ),
                    "There is no 'TagologThreadCorrelationId' tag in the Json. [Json={0}]", jsonString );
               
                dynamic json = JToken.Parse( jsonString );

                var threadCorrelationId = ( string ) json.TagologThreadCorrelationId;
                Assert.That( threadCorrelationId.Length, Is.GreaterThan( 0 ) );
            } );
        }
    }
}
