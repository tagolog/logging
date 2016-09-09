using System;
using System.Configuration;

namespace Tagolog.Private
{
    /// <summary>
    /// Container for configuration information.
    /// </summary>
    class TagologSetting
    {
        #region Initialization

        public TagologSetting( Type logAdapterType )
        {
            // Throws if log adapter type does not implement "ITaglogAdapter" interface.
            if ( !typeof( ITagLogAdapter ).IsAssignableFrom( logAdapterType ) )
                throw new ConfigurationErrorsException( string.Format(
                    "Type {0} does not implement {1} interface.",
                    logAdapterType.AssemblyQualifiedName,
                    typeof( ITagLogAdapter ).FullName) );

            LogAdapterType = logAdapterType;
        }

        #endregion // Initialization

        /// <summary>
        /// "ITagLogAdapter" type (wrapper for underlying logging subsystem).
        /// </summary>
        public Type LogAdapterType
        {
            get;
            private set;
        }
    }
}
