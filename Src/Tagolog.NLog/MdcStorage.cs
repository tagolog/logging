using System.Collections.Generic;
using System.Linq;
using NLog;

namespace Tagolog.NLog
{
    static class MdcStorage
    {
        public static void Save( string context )
        {
            MappedDiagnosticsContext.Set( TagologContextParamName, context );
        }

        public static void Clear()
        {
            MappedDiagnosticsContext.Remove( TagologContextParamName );
        }

        public static string GetContext()
        {
            if ( !MappedDiagnosticsContext.Contains( TagologContextParamName ) )
                return string.Empty;

            return MappedDiagnosticsContext.Get( TagologContextParamName );
        }

        public static Tag[] GetTags( bool builtInTagsEnabled, bool? orderByAsc )
        {
            var tagologContext = GetContext();

            if ( string.IsNullOrEmpty( tagologContext ) )
                return null;

            IDictionary<string, string> tags;
            IDictionary<string, string> builtInTags;

            TagSerializer.TagsFromString( tagologContext, out tags, out builtInTags );

            int totalTagsCount = ( builtInTagsEnabled )
                ? tags.Count + builtInTags.Count
                : tags.Count;

            if ( 0 == totalTagsCount )
                return null;

            var resultedTags = new Tag[ totalTagsCount ];
            int i = 0;
            for ( ; i < tags.Count ; i ++ )
            {
                var key = tags.Keys.ToList()[ i ];
                var value = tags[ key ];
                resultedTags[ i ] = new Tag( key, value );
            }

            if ( builtInTagsEnabled )
            {
                int j = 0;
                for ( ; i < resultedTags.Length ; i++ )
                {
                    var key = builtInTags.Keys.ToList()[ j ++ ];
                    var value = builtInTags[ key ];
                    resultedTags[ i ] = new Tag( key, value );
                }
            }

            if ( null != orderByAsc )
            {
                resultedTags = ( ( orderByAsc.Value )
                    ? resultedTags.OrderBy( _ => _.Key )
                    : resultedTags.OrderByDescending( _ => _.Key ) )
                    .ToArray();
            }

            return resultedTags;
        }

        const string TagologContextParamName = "tagolog-NLog-MDC-2C4FE90F-07F5-4E3C-AA4F-350312A353C7";
    }
}
