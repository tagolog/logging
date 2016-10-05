using System;
using NUnit.Framework;
using Tagolog.UnitTest.Tests.Helper;

namespace Tagolog.UnitTest.Tests
{
    [TestFixture]
    public class ThreadCorrelationIdExplicitOperationTests
    {
        /// <summary>
        /// Check if explicit operations are disabled on built-in "TagologThreadCorrelationId" tag.
        /// </summary>
        [Test]
        [Ignore( "Need to be refactored" )]
        public void TagologExplicitOperationOnThreadCorrelationId()
        {
            using ( var context = TagScopeManager.CreateScope() )
            {
                ExplicitOperations( context );
            }
        }

        void ExplicitOperations( ITagScope scope )
        {
            Assert.IsTrue( scope.Tags.ContainsKey( Constants.ThreadCorrelationIdTagKey ) );

            ExplicitAdd( scope );
            ExplicitUpdate( scope );
            ExplicitRemove( scope );
        }

        void ExplicitAdd( ITagScope scope )
        {
            Assert.IsTrue( ExceptionTest.IsThrown<Exception>(
                () => scope.Tags[ Constants.ThreadCorrelationIdTagKey ] = ThreadCorrelationIdValue ) );
        }

        void ExplicitUpdate( ITagScope scope )
        {
            Assert.IsTrue( ExceptionTest.IsThrown<Exception>(
                () => scope.Tags[ ThreadCorrelationIdValue ] = Constants.ThreadCorrelationIdTagKey ) );
        }

        void ExplicitRemove( ITagScope scope )
        {
            Assert.IsTrue( ExceptionTest.IsThrown<Exception>(
                () => scope.Tags.Remove( Constants.ThreadCorrelationIdTagKey ) ) );
        }

        const string ThreadCorrelationIdValue = "ThreadCorrelationIdValue";
    }
}
