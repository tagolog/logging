using System;

namespace Tagolog.Helpers
{
    static class ThrowHelper
    {
        public static void ThrowIfTagKeyIsNullEmptyOrWhiteSpace( string tagKey )
        {
            if ( null == tagKey )
                throw new ArgumentNullException( "tagKey", "Tag key cannot be null." );

            if ( string.IsNullOrEmpty( tagKey ) )
                throw new ArgumentException( "Tag key cannot be empty string.", "tagKey" );

            if ( string.IsNullOrEmpty( tagKey.Trim() ) )
                throw new ArgumentException( "Tag key cannot be white space string.", "tagKey" );
        }

        public static ArgumentNullException LogAdapterIsNullException()
        {
            return new ArgumentNullException( "Log adapter cannot be null." );
        }

        public static ArgumentException TagKeyWasNotPresentInTagCollectionException( string key )
        {
            return new ArgumentException( $"The given tag key was not present in the tag collection. [tagKey={key}]" );
        }

        public static ArgumentNullException ValueCanNotBeNullException( string parameterName )
        {
            return new ArgumentNullException( parameterName, "Value cannot be null." );
        }
    }
}
