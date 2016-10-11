using System;
using System.Collections.Generic;
using Tagolog.Helpers;
using Tagolog.Private;
using Tagolog.Collections;

namespace Tagolog
{
    public static class TagScopeManager
    {
        public static ITagScope CreateScope()
        {
            LoadLogAdapter();
            return ThreadContext.CreateScope();
        }

        public static ITagScope CreateScope( IDictionary<string, string> tags )
        {
            LoadLogAdapter();
            return ThreadContext.CreateScope( tags );
        }

        /// <summary>
        /// Gets collection of tags available on application scope.
        /// </summary>
        public static ITagCollection Tags { get { return ApplicationScopeTags; } }
        readonly static TagCollection ApplicationScopeTags = new TagCollection();

        /// <summary>
        /// Gets log adapter (wrapper for underlying logging subsystem).
        /// </summary>
       public static ITagLogAdapter LogAdapter
        {
            get
            {
                if ( _logAdapter == null )
                {
                    lock ( _logAdapterLock )
                    {
                        if ( _logAdapter == null )
                        {
                            var logAdapter = LoadLogAdapter();

                            System.Threading.Thread.MemoryBarrier();
                            _logAdapter = logAdapter;
                        }
                    }
                }

                return _logAdapter;
            }
            set
            {
                if ( null == value )
                    throw ThrowHelper.LogAdapterIsNullException();

                lock ( _logAdapterLock )
                {
                    System.Threading.Thread.MemoryBarrier();
                    _logAdapter = value;
                }
            }
        }

        #region Internals

        internal static ITagReadonlyCollection TagsAsReadonlyCollection
        {
            get { return ApplicationScopeTags; }
        }

        #endregion // Internals

        #region Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static ITagThreadContext ThreadContext
        {
            get
            {
                if ( null == _threadContext )
                    _threadContext = new TagThreadContext( LogAdapter );
                return _threadContext;
            }
        }

        /// <summary>
        /// Builds the logger factory adapter.
        /// </summary>
        /// <returns>a factory adapter instance. Is never <c>null</c>.</returns>
        static ITagLogAdapter LoadLogAdapter()
        {
            var setting = ConfigurationSectionHandler.Load();
            return ( ITagLogAdapter ) Activator.CreateInstance( setting.LogAdapterType );
        }

        #endregion // Helpers

        #region Data

        /// <summary>
        /// "TagContexts" instances stored in "ThreadContext".
        /// Thread context stored in per thread variable marked with [ThreadStatic] attribute.
        /// Each thread will automatically have its own instance of thread context.
        /// </summary>
        [ThreadStatic]
        static ITagThreadContext _threadContext;

        /// <summary>
        /// Lock object for log adapter (to ensure single instance to be loaded).
        /// </summary>
        static readonly object _logAdapterLock = new object();

        /// <summary>
        /// Log adapter - wrapper for underlying logging subsystem.
        /// </summary>
        static volatile ITagLogAdapter _logAdapter;

        #endregion // Data
    }
}
