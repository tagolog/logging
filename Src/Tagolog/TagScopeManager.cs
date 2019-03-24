using System;
using System.Collections.Generic;
using Tagolog.Helpers;
using Tagolog.Private;
using Tagolog.Collections;

namespace Tagolog
{
    public static class TagScopeManager
    {
        public static void ConfigureLogAdapter( string logAdapterFullyQualifiedTypeName )
        {
            _logAdapterFullyQualifiedTypeName = logAdapterFullyQualifiedTypeName;
        }

        public static ITagScope CreateScope()
        {
            return ThreadContext.CreateScope();
        }

        public static ITagScope CreateScope( IDictionary<string, string> tags )
        {
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
                            var logAdapter = string.IsNullOrEmpty( _logAdapterFullyQualifiedTypeName )
                                ? LoadLogAdapter()
                                : CreateLogAdapter( _logAdapterFullyQualifiedTypeName );

                            // Commented after reading following articles:
                            // https://csharpindepth.com/Articles/Singleton - Implementing the Singleton Pattern in C#
                            // http://www.albahari.com/threading/part4.aspx - Threading in C# Joseph Albahari
                            // Commented: System.Threading.Thread.MemoryBarrier();

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
                    // Commented after reading following articles:
                    // https://csharpindepth.com/Articles/Singleton - Implementing the Singleton Pattern in C#
                    // http://www.albahari.com/threading/part4.aspx - Threading in C# Joseph Albahari
                    // Commented: System.Threading.Thread.MemoryBarrier();

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
        /// Creates an instance of the log adapter.
        /// </summary>
        /// <returns>Log adapter instance.</returns>
        static ITagLogAdapter CreateLogAdapter( string fullyQualifiedTypeName )
        {
            var tagLogAdapterType = Type.GetType( fullyQualifiedTypeName, true, false );
            return ( ITagLogAdapter ) Activator.CreateInstance( tagLogAdapterType );
        }

        /// <summary>
        /// Builds the logger factory adapter.
        /// </summary>
        /// <returns>a factory adapter instance. Is never <c>null</c>.</returns>
        static ITagLogAdapter LoadLogAdapter()
        {
            throw new InvalidOperationException(
                "This operation is not supported on .NET Core platform. " +
                "You should use TagScopeManager.ConfigureLogAdapter( ... ) method call to avoid this exception." );
#if NETCOREAPP
#else
            //var setting = ConfigurationSectionHandler.Load();
            //return ( ITagLogAdapter ) Activator.CreateInstance( setting.LogAdapterType );
#endif // NETCOREAPP
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

        /// <summary>
        /// By default - fallback to NO OP adapter.
        /// </summary>
        static string _logAdapterFullyQualifiedTypeName = "Tagolog.Adapters.NoOpLogAdapter,Tagolog";

        #endregion // Data
    }
}
