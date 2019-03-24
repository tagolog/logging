using System;
using Tagolog.Collections;

namespace Tagolog.UnitTests
{
    class InMemoryCollectionLogAdapter : ITagLogAdapter
    {
        public InMemoryCollectionLogAdapter()
        {
            _tagSnapshot = new TagSnapshot
            {
                Tags = new TagReadonlyCollection( new TagCollection() ),
                BuiltInTags = new TagReadonlyCollection( new TagCollection() )
            };
        }

        /// <summary>
        /// Emulates standard adapter method that stores tags to the underlying logging subsystem.
        /// This method just stores tag values to inmemory collection, to be accessible for test purposes.
        /// </summary>
        /// <param name="tags">Tag collection.</param>
        /// <param name="builtInTags">Built-in tag collection.</param>
        public void InvalidateTags( ITagReadonlyCollection tags, ITagReadonlyCollection builtInTags )
        {
            _tagSnapshot = new TagSnapshot
            {
                Tags = tags,
                BuiltInTags = builtInTags
            };
        }

        public ITagReadonlyCollection Tags
        {
            get { return _tagSnapshot.Tags; }
        }

        public ITagReadonlyCollection BuiltInTags
        {
            get { return _tagSnapshot.BuiltInTags; }
        }

        /// <summary>
        /// Unit test runs simultaneously.
        /// Log adapter should distinguish among test instances.
        /// Each thread will automatically have its own instance of tag snapshot.
        /// </summary>
        [ThreadStatic]
        static TagSnapshot _tagSnapshot;

        class TagSnapshot
        {
            public ITagReadonlyCollection Tags { get; set; }

            public ITagReadonlyCollection BuiltInTags { get; set; }
        }
    }
}
