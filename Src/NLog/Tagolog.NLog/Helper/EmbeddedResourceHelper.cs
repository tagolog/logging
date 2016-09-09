using System;
using System.IO;
using System.Reflection;

namespace Tagolog.NLog.Helper
{
    class EmbeddedResourceHelper
    {
        /// <summary>
        /// Gets content from embedded resource file.
        /// </summary>
        /// <param name="resourceFileName">Key of the embedded resource file.</param>
        /// <returns>Content of embedded resource file.</returns>
        public static string LoadFileContent( string resourceFileName )
        {
            var scriptText = string.Empty;

            // Load script text from embedded sql file.
            var asm = Assembly.GetExecutingAssembly();
            using ( var stream = asm.GetManifestResourceStream( resourceFileName ) )
            {
                if ( null != stream )
                {
                    using ( var reader = new StreamReader( stream ) )
                    {
                        scriptText = reader.ReadToEnd();
                    }
                }
            }

            if ( string.IsNullOrEmpty( scriptText ) )
            {
                var message = string.Format( "Content of the embedded script resource \"{0}\" is missing or empty.", resourceFileName );
                throw new ArgumentException( message, "resourceFileName" );
            }

            return scriptText;
        }
    }
}
