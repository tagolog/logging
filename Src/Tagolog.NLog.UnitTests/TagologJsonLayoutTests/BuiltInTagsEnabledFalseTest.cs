using NUnit.Framework;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    [NonParallelizable]
    class BuiltInTagsEnabledFalseTest : AbstractTagologJsonLayoutTest
    {
        protected override string LoggerName { get; } = "BuiltInTagsEnabledFalse";

        [Test]
        public void Tagolog_NLog_TagologJsonLayout_BuiltInTagsEnabled_None()
        {
            Run( jsonString =>
            {
                Assert.False( jsonString.Contains( "TagologThreadCorrelationId" ),
                    "Built-in tag was found, but 'BuiltInTagsEnabled' flag was set to FALSE." );
            } );
        }
    }
}
