COMPILE NONE

using System;
using System.Data;
using System.Linq;
using Tagolog.NLog.Model;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Repository
{
    class MessageRepository
    {
        public MessageRepository( DbRepository dbRepository, IQueryCommandTextRepository queryCommandTextRepository, TagRepository tagRepository )
        {
            _dbRepository = dbRepository;
            _queryCommandTextRepository = queryCommandTextRepository;
            _tagRepository = tagRepository;
        }

        public void Insert( MessageEntity message )
        {
            var query = new SqlQuery();
            query.Parameters.Add( new SqlQueryParameter( "@customerId", DbType.Guid, 0, message.CustomerId ) );
            query.Parameters.Add( new SqlQueryParameter( "@applicationId", DbType.Guid, 0, message.ApplicationId ) );
            query.Parameters.Add( new SqlQueryParameter( "@logLevelId", DbType.Int16, 0, ( int ) message.LogLevel ) );
            query.Parameters.Add( new SqlQueryParameter( "@created", DbType.DateTime, 0, message.Created ) );
            query.Parameters.Add( new SqlQueryParameter( "@logged", DbType.DateTime, 0, message.Logged ) );
            query.Parameters.Add( new SqlQueryParameter( "@createdTicks", DbType.Int64, 0, message.Created.Ticks ) );
            query.Parameters.Add( new SqlQueryParameter( "@logUserIdentity", DbType.String, MaxUserIdentityLen, message.UserIdentity ) );
            query.Parameters.Add( new SqlQueryParameter( "@logIPAddress", DbType.String, MaxIpAddressLen, message.LogIPAddress ) );
            query.Parameters.Add( new SqlQueryParameter( "@requestIPAddress", DbType.String, MaxIpAddressLen, message.RequestIPAddress ) );
            query.Parameters.Add( new SqlQueryParameter( "@messageText", DbType.String, MaxMessageTextLen, message.MsgText ) );
            query.Parameters.Add( new SqlQueryParameter( "@exceptionText", DbType.String, MaxExceptionTextLen, message.ExceptionText ) );
            query.Parameters.Add( new SqlQueryParameter( "@correlationId", DbType.String, MaxCorrelationIdLen,
                !string.IsNullOrWhiteSpace( message.CorrelationId ) ? message.CorrelationId : ( object ) DBNull.Value ) );
            query.Parameters.Add( new SqlQueryParameter( "@packetLength", DbType.Int32, 0, message.PacketLength ) );
            query.Parameters.Add( new SqlQueryParameter( "@sphinxTagsText", DbType.String, MaxSphinxTagTextLen, message.SphinxTagsText ) );

            if ( 0 != message.Tags.Count )
            {
                var selectUnionAllTagCodeAndValue = _tagRepository.SelectUnionAllTagCodeAndValue(
                    message.Tags.ToDictionary( _ => _.Code, _ => _.Value ) );

                foreach ( var tagParameter in selectUnionAllTagCodeAndValue.Parameters )
                    query.Parameters.Add( tagParameter );

                query.CommandText = _queryCommandTextRepository.GetCommandText( QueryCommandType.MessageAndTagInsert );

                query.CommandText = query.CommandText.Replace(
                    TagSelectUnionAllParameterName, selectUnionAllTagCodeAndValue.CommandText );
            }
            else
                query.CommandText = _queryCommandTextRepository.GetCommandText( QueryCommandType.MessageInsert );

            _dbRepository.ExecuteNonQuery( query );

            //context.Messages
            //.Insert( () =>
            //    //item.ConvertToDb() так нельзя, объект должен иметь возможность преобразовываться в Expression<Func<T>>
            //  new MessageDbObject
            //  {
            //      Id = msgId,
            //      CustomerId = app.CustomerId,
            //      ApplicationId = app.Id,
            //      LevelId = item.Level.ConvertToDbValue(),
            //      Created = item.Created,
            //      Logged = DateTime.UtcNow, //item.Logged,
            //      LogUserIdentity = item.UserIdentity,
            //      LogIPAddress = item.LogIPAddress,
            //      RequestIPAddress = item.RequestIPAddress,
            //      MessageText = item.MsgText,
            //      ExceptionText = item.ExceptionText,
            //      SphinxTagsText = tagsTextForSphinx.ToString(),
            //      CorrelationId = item.CorrelationId,
            //      PacketLength = item.PacketLength,
            //      CreatedTicks = item.Created.Ticks
            //  } );

            //if ( item.Tags != null && item.Tags.Count > 0 )
            //{
            //    foreach ( KeyValuePair<string, string> tagDataPair in dictProcessedTagData )
            //    {
            //        Tag tag = dictUsedTags[ tagDataPair.Key ];

            //        context.MessageTags
            //          .Insert( () =>
            //              //item.ConvertToDb() так нельзя, объект должен иметь возможность преобразовываться в Expression<Func<T>>
            //            new MessageTagDbObject
            //            {
            //                MessageId = msgId,
            //                TagId = tag.Id,
            //                CustomerId = app.CustomerId,
            //                TagValue = tagDataPair.Value
            //            }
            //          );
            //    }
            //}
        }

        readonly DbRepository _dbRepository;
        readonly IQueryCommandTextRepository _queryCommandTextRepository;
        readonly TagRepository _tagRepository;

        const string MessageInsertSqlQueryUri = "Tagolog.NLog.DatabaseTarget.SqlQuery.MessageInsert.sql.txt";
        const string TagSelectUnionAllParameterName = "{logEntryTags_UnionAll}";

        const int MaxCorrelationIdLen = 1024;
        const int MaxUserIdentityLen = 64;
        const int MaxIpAddressLen = 1024;
        const int MaxMessageTextLen = -1;
        const int MaxExceptionTextLen = -1;
        const int MaxSphinxTagTextLen = -1;
    }
}
