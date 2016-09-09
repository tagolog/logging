using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Tagolog.Examples.CloudHosting.Emulator.Helpers
{
    static class EnglishNameHelper
    {
        public static IReadOnlyList<string> FirstNames
        {
            get
            {
                var content = LoadFileContent( GetFileNameWithNamespace( "FirstNames.txt" ) );
                return ParseContent( content );
            }
        }

        public static IReadOnlyList<string> LastNames
        {
            get
            {
                var content = LoadFileContent( GetFileNameWithNamespace( "LastNames.txt" ) );
                return ParseContent( content );
            }
        }

        static IReadOnlyList<string> ParseContent( string content )
        {
            return content.Split(
                new [] { "\r\n" },
                StringSplitOptions.RemoveEmptyEntries );
        }

        static string GetFileNameWithNamespace( string fileName )
        {
            return string.Format( "{0}.{1}",
                typeof ( EnglishNameHelper ).Namespace,
                fileName );
        }

        /// <summary>
        /// Gets content from embedded resource file.
        /// </summary>
        /// <param name="resourceFileName">Key of the embedded resource file.</param>
        /// <returns>Content of embedded resource file.</returns>
        static string LoadFileContent( string resourceFileName )
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
