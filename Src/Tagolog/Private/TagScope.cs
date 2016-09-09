using System.Collections.Generic;
using Tagolog.Collections;
using Tagolog.Private.Correlation;
using Tagolog.Private.CustomDictionary;

namespace Tagolog.Private
{
    class TagScope : ITagScope
    {
        #region Initialization

        TagScope( ITagThreadContext threadContext )
        {
            _builtInTags = new TagCollection();
            _builtInTagsAsReadonlyCollection = new TagReadonlyCollection( _builtInTags );

            _threadContext = threadContext;
            GenerateThreadCorrelationId( null ); // null = not parent correlation ID.
            _tags.Changed += OnTagsChanged;
        }

        public TagScope( ITagThreadContext threadContext, ITagReadonlyCollection applicationScopeTags )
            : this( threadContext )
        {
            _inheritedTags = new TagReadonlyCollection( applicationScopeTags );
            _effectiveTags = new TagEffectiveCollection( _inheritedTags, _tags );
        }

        public TagScope( ITagThreadContext threadContext, ITagReadonlyCollection applicationScopeTags, IDictionary<string, string> tags )
            : this( threadContext, applicationScopeTags )
        {
            AddOrReplaceTags( tags );
        }

        public TagScope( ITagThreadContext threadContext, TagScope parentTagScope )
            : this( threadContext )
        {
            _parentTagScope = parentTagScope;
            GenerateThreadCorrelationId( parentTagScope.ThreadCorrelationId );
            _inheritedTags = new TagReadonlyCollection( parentTagScope._effectiveTags );
            _effectiveTags = new TagEffectiveCollection( _inheritedTags, _tags );
        }

        public TagScope( ITagThreadContext threadContext, TagScope parentTagScope, IDictionary<string, string> tags )
            : this( threadContext, parentTagScope )
        {
            AddOrReplaceTags( tags );
        }

        #endregion // Initialization

        #region "ITagScope" interface implementation

        /// <summary>
        /// Gets collection of tags available within the scope.
        /// </summary>
        public ITagCollection Tags { get { return _tags; } }
        readonly TagCollection _tags = new TagCollection();

        /// <summary>
        /// Gets readonly collection of tags inherited from upper scopes.
        /// </summary>
        public ITagReadonlyCollection InheritedTags { get { return _inheritedTags; } }
        readonly ITagReadonlyCollection _inheritedTags;

        /// <summary>
        /// Gets readonly collection of tags effective (inherited + tags from current scope).
        /// </summary>
        public ITagReadonlyCollection EffectiveTags { get { return _effectiveTags; } }
        readonly ITagReadonlyCollection _effectiveTags;

        /// <summary>
        /// Gets readonly collection of built-in tags.
        /// </summary>
        public ITagReadonlyCollection BuiltInTags { get { return _builtInTagsAsReadonlyCollection; } }
        readonly TagCollection _builtInTags;
        readonly ITagReadonlyCollection _builtInTagsAsReadonlyCollection;

        #endregion // "ITagScope" interface implementation

        #region "IDisposable" interface implementation

        public void Dispose()
        {
            _threadContext.ReleaseScope( this );
        }

        #endregion // "IDisposable" interface implementation

        #region Event handlers

        void OnTagsChanged( object sender, DictionaryChangedEventArgs<string, Tag> eventArgs )
        {
            _threadContext.InvalidateScope( this );
        }

        #endregion // Event handlers

        void GenerateThreadCorrelationId( ThreadCorrelationId parentCorrelationId )
        {
            ThreadCorrelationId = ( null == parentCorrelationId ) ?
                new ThreadGuidCorrelationId() :
                new ThreadGuidCorrelationId( parentCorrelationId );

            _builtInTags[ Constants.ThreadCorrelationIdTagKey ] = ThreadCorrelationId.ToString();
        }

        void AddOrReplaceTags( IDictionary<string, string> tags )
        {
            foreach ( KeyValuePair<string, string> pair in tags )
                _tags[ pair.Key ] = pair.Value;
        }

        #region Public properties

        public ThreadCorrelationId ThreadCorrelationId { get; protected set; }

        #endregion // Public properties

        #region Data

        ITagThreadContext _threadContext;
        readonly ITagScope _parentTagScope;

        #endregion // Data
    }
}
