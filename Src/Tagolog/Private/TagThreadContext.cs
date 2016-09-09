using System;
using System.Collections.Generic;
using System.Linq;
using Tagolog.Collections;

namespace Tagolog.Private
{
    class TagThreadContext : ITagThreadContext
    {
        #region Initialization

        public TagThreadContext( ITagLogAdapter logAdapter )
        {
            _logAdapter = logAdapter;
        }

        #endregion // Initialization

        #region "ITagThreadContext" interface implementation

        public ITagScope CreateScope()
        {
            TagScope tagScope;

            if ( 0 == _tagScopes.Count )
                // Create first scope. This scope inherits application scope tags.
                tagScope = new TagScope( this, TagScopeManager.TagsAsReadonlyCollection );
            else
                tagScope = new TagScope( this, _tagScopes.Last.Value );

            _tagScopes.AddLast( tagScope );
            InvalidateScope( tagScope );
            return tagScope;
        }

        public ITagScope CreateScope( IDictionary<string, string> tags )
        {
            TagScope tagScope;

            if ( 0 == _tagScopes.Count )
                // Create first scope. This scope inherits application scope tags.
                tagScope = new TagScope( this, TagScopeManager.TagsAsReadonlyCollection, tags );
            else
                tagScope = new TagScope( this, _tagScopes.Last.Value, tags );

            _tagScopes.AddLast( tagScope );
            InvalidateScope( tagScope );
            return tagScope;
        }

        public void InvalidateScope( ITagScope tagScope )
        {
            InvalidateLogAdapterTags();
        }

        public void ReleaseScope( ITagScope tagScope )
        {
            if ( 0 == _tagScopes.Count )
                return;

            if ( tagScope == _tagScopes.Last.Value )
            {
                // Pop topmost tag scope out of the stack.
                _tagScopes.RemoveLast();
            }
            else
            {
                // This is unusual case.
                // An attempt to release scope for the item that is not on the top of tag scope stack.
                throw new InvalidOperationException( "Attempt to release tag scope that is not the last scope in the chain." );
            }

            InvalidateLogAdapterTags();
        }

        #endregion // "ITagThreadContext" interface implementation

        #region Helpers

        void InvalidateLogAdapterTags()
        {
            if ( 0 != _tagScopes.Count )
            {
                // Use "EffectiveTags" collection of latest scope as actual set of tags.
                _logAdapter.InvalidateTags(
                    _tagScopes.Last.Value.EffectiveTags,
                    _tagScopes.Last.Value.BuiltInTags );
            }
            else
            {
                // No thread tag scopes were found. Use application level tags from "TagScopeManager".
                _logAdapter.InvalidateTags(
                    TagScopeManager.TagsAsReadonlyCollection,
                    new TagReadonlyCollection( new TagCollection() ));
            }
        }

        #endregion // Helpers

        #region Data

        readonly ITagLogAdapter _logAdapter;
        readonly LinkedList<TagScope> _tagScopes = new LinkedList<TagScope>();

        #endregion // Data
    }
}
