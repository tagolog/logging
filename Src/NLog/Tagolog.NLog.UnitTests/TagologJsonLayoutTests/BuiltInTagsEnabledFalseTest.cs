using NUnit.Framework;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    class BuiltInTagsEnabledFalseTest : AbstractTagologJsonLayoutTest
    {
        [Test]
        public void Tagolog_NLog_TagologJsonLayout_BuiltInTagsEnabled_None()
        {
            Run( LoggerName, jsonString =>
            {
                Assert.False( jsonString.Contains( "TagologThreadCorrelationId" ),
                    "Built-in tag was found, but 'BuiltInTagsEnabled' flag was set to FALSE." );
            } );
        }

        const string LoggerName = "BuiltInTagsEnabledFalse";
    }
}
