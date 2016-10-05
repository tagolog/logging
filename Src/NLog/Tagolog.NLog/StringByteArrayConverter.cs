using System;

namespace Tagolog.NLog
{
    /// <summary>
    /// http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa/24343727#24343727
    /// </summary>
    class StringByteArrayConverter
    {
        public static byte[] StringToByteArray( string hex )
        {
            var numberChars = hex.Length;
            var bytes = new byte[ numberChars / 2 ];
            for ( int i = 0 ; i < numberChars ; i += 2 )
                bytes[ i / 2 ] = Convert.ToByte( hex.Substring( i, 2 ), 16 );
            return bytes;
        }

        public static string ByteArrayToString( byte[] bytes )
        {
            var lookup32 = Lookup32;
            var result = new char[ bytes.Length * 2 ];
            for ( int i = 0 ; i < bytes.Length ; i++ )
            {
                var val = lookup32[ bytes[ i ] ];
                result[ 2 * i ] = ( char ) val;
                result[ 2 * i + 1 ] = ( char ) ( val >> 16 );
            }
            return new string( result );
        }

        static readonly uint[] Lookup32 = CreateLookup32();

        static uint[] CreateLookup32()
        {
            var result = new uint[ 256 ];
            for ( int i = 0 ; i < 256 ; i++ )
            {
                var s = i.ToString( "X2" );
                result[ i ] = ( ( uint ) s[ 0 ] ) + ( ( uint ) s[ 1 ] << 16 );
            }
            return result;
        }
    }
}
