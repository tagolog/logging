using System;

namespace Tagolog.NLog.Helper
{
    static class EnumHelpers
    {
        /// <summary>
        /// Map one enum to another.
        /// </summary>
        /// <typeparam name="TEnumSrc">Type of source enum.</typeparam>
        /// <typeparam name="TEnumDst">Type of destination enum.</typeparam>
        /// <param name="src">Value of the source enum.</param>
        /// <returns>Value of the destination enum.</returns>
        static public TEnumDst MapEnum<TEnumDst, TEnumSrc>( TEnumSrc src )
        {
            return ( TEnumDst ) Enum.Parse( typeof( TEnumDst ), src.ToString() );
        }
    }
}
