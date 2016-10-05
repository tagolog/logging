using NUnit.Framework;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    class BuiltInTagsEnabledNoneTest : AbstractTagologJsonLayoutTest
    {
        [Test]
        public void Tagolog_NLog_TagologJsonLayout_BuiltInTagsEnabled_None()
        {
            Run( LoggerName, jsonString =>
            {
                Assert.False( jsonString.Contains( "TagologThreadCorrelationId" ),
                    "Built-in tag was found, but 'BuiltInTagsEnabled' flag was not set." );
            } );
        }

        const string LoggerName = "BuiltInTagsEnabledNone";
    }
}
