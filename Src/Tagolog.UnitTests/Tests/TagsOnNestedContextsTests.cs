using System.Collections.Generic;
using NUnit.Framework;
using Tagolog.UnitTests.Model;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests
{
    [TestFixture]
    [NonParallelizable]
    public class TagsOnNestedContextsTests
    {
        [Test]
        [Ignore( "Obsolete - will be removed" )]
        public void TagologTagsOnNestedContexts()
        {
            new NestedContextsBasedOnLinkedList().Invoke( _useCases );
        }

        readonly List<TagTestCase> _useCases = new List<TagTestCase>
        {
            new TagTestCase
            {
                Name = "New scope from scratch",
                TagsForCreateContext = new TagCollection { { "firstTag", "firstValue" } },
                ExpectedTags = new TagCollection { { "firstTag", "firstValue" } },
            },

            new TagTestCase
            {
                Name = "Nested scope",
                TagsForCreateContext = new TagCollection { { "secondTag", "secondValue" } },
                ExpectedTags = new TagCollection
                {
                    { "firstTag", "firstValue" },
                    { "secondTag", "secondValue" }
                }
            },

            new TagTestCase
            {
                Name = "Nested scope with tag that replaces value of the tag from parent scope",
                TagsForCreateContext = new TagCollection
                {
                    { "secondTag", "NEW_SecondValue" },
                    { "thirdTag", "thirdValue" }
                },
                ExpectedTags = new TagCollection
                {
                    { "firstTag", "firstValue" },
                    { "secondTag", "NEW_SecondValue" },
                    { "thirdTag", "thirdValue" }
                }
            },

            new TagTestCase
            {
                Name = "Nested scope add tag directly to scope tags",
                TagsForCreateContext = new TagCollection
                {
                    { "fourthTag", "fourthValue" }
                },
                ExpectedTags = new TagCollection
                {
                    { "firstTag", "firstValue" },
                    { "secondTag", "NEW_SecondValue" },
                    { "thirdTag", "thirdValue" },
                    { "fourthTag", "fourthValue" },
                    { "directTag2Context", "directTag2ContextValue" }
                },
                Action = ( useCase, context ) => context.Tags[ "directTag2Context" ] = "directTag2ContextValue"
            }
        };
    }
}
