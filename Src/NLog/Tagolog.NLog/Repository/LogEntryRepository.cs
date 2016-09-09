COMPILE NONE

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Transactions;
using System.Collections.Generic;
using Tagolog.NLog.Model;
using Tagolog.NLog.Helper;
using Tagolog.NLog.Factory;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Repository
{
    class LogEntryRepository
    {
        public LogEntryRepository( DbConnectionFactory dbConnectionFactory, IQueryCommandTextRepository queryCommandTextRepository )
        {
            _dbConnectionFactory = dbConnectionFactory;
            _queryCommandTextRepository = queryCommandTextRepository;
        }

        public void Insert(
            string applicationCodeText,
            string identityName,
            DbLogLevel logLevel,
            string messageText,
            string exceptionText,
            IDictionary<string, string> logEventTags )
        {
            var correlationId = string.Empty;
            if ( logEventTags.ContainsKey( Constants.ThreadCorrelationIdTagKey ) )
                correlationId = logEventTags[ Constants.ThreadCorrelationIdTagKey ];

            // Exclude tagolog internal correlation ID tag.
            logEventTags = logEventTags
                .Where( _ => _.Key != Constants.ThreadCorrelationIdTagKey )
                .ToDictionary( _ => _.Key, _ => _.Value );

            // TransactionScopeOption.Suppress - hope this will give the abilty to log with external transaction rollback.
            // TransactionOptions.Timeout and TransactionOptions.IsolationLevel should be configurable.
            var tranOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            };

            using ( var transactionScope = new TransactionScope( TransactionScopeOption.Required, tranOptions ) )
            {
                using ( var dbConnection = _dbConnectionFactory.CreateConnection() )
                {
                    dbConnection.Open();

                    var dbRepository = new DbRepository( dbConnection );
                    var applicationRepository = new ApplicationRepository( dbRepository, _queryCommandTextRepository );
                    var tagRepository = new TagRepository( dbRepository, _queryCommandTextRepository );
                    var messageRepository = new MessageRepository( dbRepository, _queryCommandTextRepository, tagRepository );

                    Guid applicationCode;
                    if ( !Guid.TryParse( applicationCodeText, out applicationCode ) )
                        throw new Exception( string.Format(
                            "Application code should be a string in GUID format " +
                            "(32 digits separated by hyphens: 00000000-0000-0000-0000-000000000000)." +
                            " [applicationCode={0}]",
                            applicationCodeText ) );

                    var application = applicationRepository.GetByCode( applicationCode );

                    var tags = tagRepository.GetTags( application.CustomerId, logEventTags );

                    DateTime utcNow = DateTime.UtcNow;
                    var ipAddress = ComposeIpAddress();

                    var message = new MessageEntity
                    {
                        ApplicationId = application.Id,
                        CustomerId = application.CustomerId,
                        LogLevel = logLevel,
                        Created = utcNow,
                        Logged = utcNow,
                        UserIdentity = identityName,
                        LogIPAddress = ipAddress,
                        RequestIPAddress = ipAddress,
                        MsgText = messageText,
                        ExceptionText = exceptionText,
                        CorrelationId = correlationId,
                        SphinxTagsText = SphinxTagHelper.TagsToSearchString( tags ),
                        Tags = tags
                    };
                    messageRepository.Insert( message );
                }

                transactionScope.Complete();
            }
        }

        string ComposeIpAddress()
        {
            return Dns.GetHostEntry( Dns.GetHostName() ).AddressList.FirstOrDefault( ip => ip.AddressFamily == AddressFamily.InterNetwork ).ToString();
        }

        readonly DbConnectionFactory _dbConnectionFactory;
        readonly IQueryCommandTextRepository _queryCommandTextRepository;
    }
}
