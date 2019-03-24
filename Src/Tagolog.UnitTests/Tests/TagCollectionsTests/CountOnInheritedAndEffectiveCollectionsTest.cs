using System.Collections.Generic;
using NUnit.Framework;
using Tagolog.UnitTests.Helpers;

namespace Tagolog.UnitTests.Tests.TagCollectionsTests
{
    [TestFixture]
    [NonParallelizable]
    internal class CountOnInheritedAndEffectiveCollectionsTest
    {
        [Test]
        public void TagCollections_CountOnInheritedAndEffectiveCollections()
        {
            CreateEmptyScope();
            NestedScopesWithTags();
        }

        static void CreateEmptyScope()
        {
            using ( var scope = TagScopeManager.CreateScope() )
            {
                scope.AssertOnTagCollectionCounts( 0, 0, 0 );

                using ( var nestedScope = TagScopeManager.CreateScope() )
                {
                    nestedScope.AssertOnTagCollectionCounts( 0, 0, 0 );
                }
            }
        }

        static void NestedScopesWithTags()
        {
            using ( var scope1 = TagScopeManager.CreateScope( new Dictionary<string, string>
            {
                { "scope1_tag1", "scope1_tag1_value" },
                { "scope1_tag2", "scope1_tag2_value" }
            } ) )
            {
                // Expected counts: tags, inherited, effective.
                scope1.AssertOnTagCollectionCounts( 2, 0, 2 );

                using ( var scope2 = TagScopeManager.CreateScope( new Dictionary<string, string>
                {
                    { "scope1_scope2_tag1", "scope1_scope2_tag1_value" },
                    { "scope1_scope2_tag2", "scope1_scope2_tag2_value" }
                } ) )
                {
                    // Expected counts: tags, inherited, effective.
                    scope2.AssertOnTagCollectionCounts( 2, 2, 4 );

                    using ( var scope3 = TagScopeManager.CreateScope( new Dictionary<string, string>
                    {
                        { "scope1_scope2_scope3_tag1", "scope1_scope2_scope3_tag1_value" },
                        { "scope1_scope2_scope3_tag2", "scope1_scope2_scope3_tag1_value" }
                    } ) )
                    {
                        // Expected counts: tags, inherited, effective.
                        scope3.AssertOnTagCollectionCounts( 2, 4, 6 );

                        scope3.Tags.Add( "scope1_scope2_scope3_tag3", "scope1_scope2_scope3_tag3_value" );
                        scope3.AssertOnTagCollectionCounts( 3, 4, 7 );

                        scope3.Tags.Add( "scope1_scope2_scope3_tag4", "scope1_scope2_scope3_tag4_value" );
                        scope3.AssertOnTagCollectionCounts( 4, 4, 8 );

                        // Modify tags in upper scopes.
                        scope2.Tags.Add( "scope1_scope2_tag3", "scope1_scope2_tag3_value" );
                        scope3.AssertOnTagCollectionCounts( 4, 5, 9 );

                        scope1.Tags.Add( "scope1_tag3", "scope1_tag3_value" );
                        scope3.AssertOnTagCollectionCounts( 4, 6, 10 );
                    }

                    // Expected counts: tags, inherited, effective.
                    scope2.AssertOnTagCollectionCounts( 3, 3, 6 );
                }
            }
        }
    }
}
