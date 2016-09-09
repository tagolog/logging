COMPILE NONE

using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Tagolog.NLog.Interface;
using TagEntity = Tagolog.NLog.Model.TagEntity;

namespace Tagolog.NLog.Repository
{
    class TagRepository
    {
        public TagRepository( DbRepository dbRepository, IQueryCommandTextRepository queryCommandTextRepository )
        {
            _dbRepository = dbRepository;
            _queryCommandTextRepository = queryCommandTextRepository;
        }

        public List<TagEntity> GetTags( Guid customerId, IDictionary<string, string> tags )
        {
            if ( 0 == tags.Count )
                return new List<TagEntity>();

            var selectUnionAllTagCodes = SelectUnionAllTagCode( tags );

            var query = new SqlQuery()
            {
                CommandText = _queryCommandTextRepository.GetCommandText( QueryCommandType.TagInsertAndSelect ).Replace(
                    @"{logEntryTagCodes_UnionAll}", selectUnionAllTagCodes.CommandText ),
                Parameters = selectUnionAllTagCodes.Parameters
            };

            query.Parameters.Add( new SqlQueryParameter( "@customerId", DbType.Guid, 0, customerId ) );

            var tagEntities = new List<TagEntity>();
            _dbRepository.ExecuteReader( query, delegate( IDataReader reader )
            {
                // var customerId = ( Guid ) reader[ "CustomerId" ];
                var id = ( Guid ) reader[ "Id" ];
                var code = ( string ) reader[ "Code" ];
                var rowId = ( long ) reader[ "RowId" ];
                tagEntities.Add( new TagEntity
                {
                    Id = id,
                    Code = code,
                    RowId = rowId,
                    Value = tags[ code ]
                } );
            } );

            return tagEntities;
        }

        /// <summary>
        /// Compose SQL query with virtual "select tagCode union all" table for each tag.
        /// </summary>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Query with virtual "select tagCode union all" table.</returns>
        public SqlQuery SelectUnionAllTagCode( IEnumerable<KeyValuePair<string, string>> tags )
        {
            var query = new SqlQuery();

            // Convert each tag to SQL "select ...values..." query with parameters.
            var i = 0;
            var selects = new List<string>();
            foreach ( var tag in tags )
            {
                var tagCodeParam = TagParameter( TagParameterType.TagCode, i, tag );
                query.Parameters.Add( tagCodeParam );
                selects.Add( string.Format(
                    "select {0} as Code", tagCodeParam.Name ) );
                i++;
            }

            query.CommandText = UnionAll( selects );

            return query;
        }

        /// <summary>
        /// Compose SQL query with virtual "select tagCode, tagValue union all" table for each tag.
        /// </summary>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Query with virtual "select tagCode, tagValue union all" table.</returns>
        public SqlQuery SelectUnionAllTagCodeAndValue( IEnumerable<KeyValuePair<string, string>> tags )
        {
            var query = new SqlQuery();

            // Convert each tag to SQL "select ...values..." query with parameters.
            var i = 0;
            var selects = new List<string>();
            foreach ( var tag in tags )
            {
                var tagCodeParam = TagParameter( TagParameterType.TagCode, i, tag );
                var tagValueParam = TagParameter( TagParameterType.TagValue, i, tag );
                query.Parameters.Add( tagCodeParam );
                query.Parameters.Add( tagValueParam );
                selects.Add( string.Format(
                    "select {0} as Code, {1} as Value", tagCodeParam.Name, tagValueParam.Name ) );
                i ++;
            }

            query.CommandText = UnionAll( selects );

            return query;
        }

        enum TagParameterType
        {
            TagCode,
            TagValue
        }

        SqlQueryParameter TagParameter( TagParameterType type, int index, KeyValuePair<string, string> tag )
        {
            var name = ( type == TagParameterType.TagCode )
                ? string.Format( "@tagCode{0}", index )
                : string.Format( "@tagValue{0}", index );

            var size = ( type == TagParameterType.TagCode ) ? MaxTagCodeLen : MaxTagValueLen;

            var value = ( type == TagParameterType.TagCode ) ? tag.Key : tag.Value;

            return new SqlQueryParameter( name, DbType.String, size, value );
        }

        string UnionAll( IEnumerable<string> selects )
        {
            return string.Join( " union all\r\n", selects );
        }

        static IDictionary<string, string> Ellipsis( IEnumerable<KeyValuePair<string, string>> tags )
        {
            return
                FilterInvalidTags( tags )
                .ToDictionary(
                    _ => _.Key,
                    _ => ( _.Value.Length > TagValueMaxLength )
                        ? _.Value.Substring( 0, TagValueMaxLength ) + TagValueEllipsis
                        : _.Value );
        }

        static IEnumerable<KeyValuePair<string, string>> FilterInvalidTags( IEnumerable<KeyValuePair<string, string>> tags )
        {
            return tags
                .Where( _ => !string.IsNullOrWhiteSpace( _.Value ) )
                .Where( _ => !string.IsNullOrWhiteSpace( _.Value.Trim() ) )
                .ToDictionary( _ => _.Key, _ => _.Value );
        }

        void DoSmthElse( IDictionary<string, string> tags )
        {
            StringBuilder tagsTextForSphinx = new StringBuilder();
            IDictionary<string, Tag> dictUsedTags = new Dictionary<string, Tag>();
            IDictionary<string, string> dictProcessedTagData = new Dictionary<string, string>();

            if ( null == tags || 0 == tags.Count )
                return;

            foreach ( KeyValuePair<string, string> tagPair in tags )
            {
                string value = tagPair.Value;
                if ( string.IsNullOrWhiteSpace( value ) )
                    continue;

                // Since database column is of a restricted length, let's cut tag value.
                value = value.Trim();
                if ( value.Length > TagValueMaxLength )
                    value = value.Substring( 0, TagValueMaxLength ) + TagValueEllipsis;

                //var tag = database.Tags.Single( _ => _.CustomerId == application.CustomerId );
                //if ( null == tag )
                //{
                //    tag = new Tag();
                //    tag.TagId = Guid.NewGuid();
                //    tag.CustomerId = application.CustomerId;
                //    tag.Code = tagPair.Key;
                //    tag.Key = tagPair.Key;

                //    database.Insert( tag );
                //}

                //Tag tag = _tagRepository.GetOrInsert( app.CustomerId, tagData.Key, () => context );

                //dictUsedTags.Add( tagData.Key, tag );
                //dictProcessedTagData.Add( tagData.Key, value );
                //tagsTextForSphinx.Append( TagHelper.MakeSearchDataForMessageTag( tag.RowId, value ) );

                //trnScope.Complete();

            }
        }

        readonly DbRepository _dbRepository;
        readonly IQueryCommandTextRepository _queryCommandTextRepository;

        const string TagValueEllipsis = "...";
        static readonly int TagValueMaxLength = 512 - TagValueEllipsis.Length;

        const int MaxTagCodeLen = 64;
        const int MaxTagValueLen = 128;

        const string TagInsertAndSelectSqlQueryUri = "Tagolog.NLog.DatabaseTarget.SqlQuery.TagInsertAndSelect.sql.txt";
    }
}
