COMPILE NONE

using System;
using System.Text;
using System.Collections.Generic;
using Tagolog.NLog.Model;

namespace Tagolog.NLog.Helper
{
    static class SphinxTagHelper
    {
        public static string TagsToSearchString( IEnumerable<TagEntity> tags )
        {
            var sb = new StringBuilder();
            foreach ( var tag in tags )
                sb.Append( MakeSearchDataForMessageTag( tag.RowId, tag.Value ) );

            return sb.ToString();
        }

        /// <summary>
        /// Для поискового движка Sphinx было принято решение сформировать из тэгов сообщения строку в xml формате
        /// Каждый тэг обернут в xml элемент. Название элемента формируется в данном методе
        /// </summary>
        /// <param name="tagRowId"></param>
        /// <returns></returns>
        public static string MakeSearchTagCode( string tagRowId )
        {
            if ( string.IsNullOrEmpty( tagRowId ) )
            {
                throw new ArgumentException( "tagRowId" );
            }
            return "t" + tagRowId;
        }

        /// <summary>
        /// Для заданного тэга сообщения сформировать строку, по которой будет в дальлнейшем производиться индексирование 
        /// и поиск поисковой системой (сделано для поисковой системы Sphinx в виде xml формата для поиска по зонам)
        /// </summary>
        /// <param name="tagRowId">идентификатор тэга</param>
        /// <param name="tagData">данные тэга для заданного сообщения</param>
        /// <returns></returns>
        public static string MakeSearchDataForMessageTag( long tagRowId, string tagData )
        {
            if ( tagRowId < 0 )
            {
                throw new ArgumentException( "tagRowId" );
            }
            if ( string.IsNullOrWhiteSpace( tagData ) )
            {
                return "";
            }

            string tagCode = MakeSearchTagCode( tagRowId.ToString() );
            return string.Format( "<{0}>{1}</{0}>", tagCode, tagData.Trim() );
        }
    }
}
