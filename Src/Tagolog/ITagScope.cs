using System;

namespace Tagolog
{
    public interface ITagScope : IDisposable
    {
        /// <summary>
        /// Gets collection of tags available within this scope.
        /// </summary>
        ITagCollection Tags { get; }

        /// <summary>
        /// Gets readonly collection of tags inherited from upper scopes.
        /// </summary>
        ITagReadonlyCollection InheritedTags { get; }

        /// <summary>
        /// Gets readonly collection of tags effective (inherited + tags from current scope).
        /// </summary>
        ITagReadonlyCollection EffectiveTags { get; }
    }
}
