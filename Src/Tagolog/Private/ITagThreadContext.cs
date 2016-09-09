using System.Collections.Generic;

namespace Tagolog.Private
{
    interface ITagThreadContext
    {
        ITagScope CreateScope();
        ITagScope CreateScope( IDictionary<string, string> tags );

        /// <summary>
        /// React on changes made in tag scope.
        /// </summary>
        /// <param name="tagScope">Tag scope that was changed.</param>
        void InvalidateScope( ITagScope tagScope );

        void ReleaseScope( ITagScope tagScope );
    }
}
