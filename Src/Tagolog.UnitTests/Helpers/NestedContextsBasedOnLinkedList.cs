using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Tagolog.UnitTests.Model;

namespace Tagolog.UnitTests.Helpers
{
    class NestedContextsBasedOnLinkedList
    {
        public void Invoke( IEnumerable<TagTestCase> testCasesAsList )
        {
            var testCasesAsLinkedList = LinkedListConverter.ToLinkedList( testCasesAsList );
            Invoke( testCasesAsLinkedList.First );
        }

        void Invoke( LinkedListNode<TagTestCase> linkedListNode )
        {
            var testCase = linkedListNode.Value;
            using ( var context = TagScopeManager.CreateScope( testCase.TagsForCreateContext ) )
            {
                testCase.Scope = context;

                if ( null != testCase.Action )
                    testCase.Action( testCase, context );

                ValidateTestCase( testCase );

                if ( null != linkedListNode.Next )
                    Invoke( linkedListNode.Next );
            }

            // Validate parent scope.
            ValidateParentTestCase( linkedListNode );
        }

        void ValidateParentTestCase( LinkedListNode<TagTestCase> linkedListNode )
        {
            // Get parent test case.
            if ( null == linkedListNode.Previous )
                return;

            // Validate parent scope.
            ValidateTestCase( linkedListNode.Previous.Value );
        }

        void ValidateTestCase( TagTestCase testCase )
        {
            throw new NotImplementedException();

            //// Compare actually logged tags with expected.
            //var actuallyLoggedTags = InMemoryTagStack.Peek();
            //AssertTagsAreEqual( testCase.ExpectedTags, actuallyLoggedTags );

            //// Compare tags in scope with expected.
            //AssertTagsAreEqual( testCase.ExpectedTags, TagCollection.FromIEnumerable( testCase.Scope.EffectiveTags ) );
        }

        static void AssertTagsAreEqual( TagCollection expected, TagCollection actual )
        {
            Assert.AreEqual( expected.Count, actual.Count );

            foreach ( var expectedTag in expected )
            {
                Assert.IsTrue( actual.ContainsKey( expectedTag.Key ) );

                var expectedValue = expectedTag.Value;
                var actualValue = actual[ expectedTag.Key ];
                Assert.AreEqual( expectedValue, actualValue );
            }
        }

        static TagCollection CutOutCorrelationId( TagCollection tags )
        {
            // tags.Count > 0 
            Assert.Greater( tags.Count, 0 );
            Assert.IsTrue( tags.ContainsKey( CorrelationIdName ) );
            return TagCollection.FromIEnumerable( tags.Where( _ => _.Key != CorrelationIdName ) );
        }

        const string CorrelationIdName = Constants.ThreadCorrelationIdTagKey;
    }
}
