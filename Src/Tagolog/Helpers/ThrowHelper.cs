using System;

namespace Tagolog.Helpers
{
    static class ThrowHelper
    {
        public static void ThrowIfTagKeyIsNullEmptyOrWhiteSpace( string tagKey )
        {
            if ( null == tagKey )
                throw new ArgumentNullException( "tagKey", Properties.Resources.TagKeyCanNotBeNull_ErrorMessage );

            if ( string.IsNullOrEmpty( tagKey ) )
                throw new ArgumentException( Properties.Resources.TagKeyCanNotBeEmptyString_ErrorMessage, "tagKey" );

            if ( string.IsNullOrEmpty( tagKey.Trim() ) )
                throw new ArgumentException( Properties.Resources.TagKeyCanNotBeWhiteSpaceString_ErrorMessage, "tagKey" );
        }

        public static ArgumentNullException LogAdapterIsNullException()
        {
            return new ArgumentNullException( Properties.Resources.LogAdapterCanNotBeNull_ErrorMessage );
        }
    }
}
