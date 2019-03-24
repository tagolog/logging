using NUnit.Framework;

namespace Tagolog.NLog.UnitTests.TagologJsonLayoutTests
{
    [TestFixture]
    [Category( "NLog")]
    [NonParallelizable]
    class BuiltInTagsEnabledNoneTest : AbstractTagologJsonLayoutTest
    {
        protected override string LoggerName { get; } = "BuiltInTagsEnabledNone";

        [Test]
        public void Tagolog_NLog_TagologJsonLayout_BuiltInTagsEnabled_None()
        {
            Run( jsonString =>
            {
                Assert.False( jsonString.Contains( "TagologThreadCorrelationId" ),
                    "Built-in tag was found, but 'BuiltInTagsEnabled' flag was not set." );
            } );
        }
    }
}
