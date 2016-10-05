using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    class TagsKeyPrefixTest : AbstractTagologJsonLayoutTest
    {
        [Test]
        public void Tagolog_NLog_TagologJsonLayout_TagsKeyPrefix()
        {
            Run( LoggerName, jsonString =>
            {
                dynamic json = JToken.Parse( jsonString );

                var tag1Value = ( string ) json.PrefixTag1;
                var tag2Value = ( string ) json.PrefixTag2;

                Assert.That( tag1Value, Is.EqualTo( "Value1" ) );
                Assert.That( tag2Value, Is.EqualTo( "Value2" ) );
            }, checkTagValues:false );
        }

        const string LoggerName = "TagsKeyPrefix";
    }
}
