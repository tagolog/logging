COMPILE NONE

using System;
using System.Configuration;
using System.Collections.Generic;
using NLog;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;
using NLog.LayoutRenderers;
using Tagolog.NLog.Model;
using Tagolog.NLog.Helper;
using Tagolog.NLog.Factory;
using Tagolog.NLog.Interface;
using Tagolog.NLog.Properties;
using Tagolog.NLog.Repository;

namespace Tagolog.NLog
{
    [Target( "TagologDatabase" )]
    public sealed class TagologDatabaseTarget : Target
    {
        public TagologDatabaseTarget()
        {
            MessageLayout = DefaultMessageLayout;
            ExceptionLayout = DefaultExceptionLayout;
        }

        /// <summary>
        /// Gets or sets tagolog application ID.
        /// </summary>
        public string ApplicationCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the connection string in the "connectionStrings" section of application configuration file.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Gets or sets the layout used to format log message text.
        /// </summary>
        public Layout MessageLayout { get; set; }

        /// <summary>
        /// Gets or sets the layout used to format exception message text.
        /// </summary>
        public Layout ExceptionLayout { get; set; }

        /// <summary>
        /// Initializes the target. Can be used by inheriting classes to initialize logging.
        /// </summary>
        protected override void InitializeTarget()
        {
            var css = ConfigurationManager.ConnectionStrings[ ConnectionStringName ];
            var provider = SqlProviderSelector.GetProvider( css.ProviderName );
            _queryCommandTextRepository = QueryCommandTextRepositoryFactory.GetQueryCommandTextRepository( provider );
            _dbConnectionFactory = new DbConnectionFactory( css );
        }

        /// <summary>
        /// Closes the target and releases any unmanaged resources.
        /// </summary>
        protected override void CloseTarget()
        {
        }

        /// <summary>
        /// Writes the specified logging event to the database.
        /// Creates a new database command, prepares parameters
        /// for it by calculating layouts and executes the command.
        /// </summary>
        /// <param name="logEvent">Logging event instance.</param>
        protected override void Write( LogEventInfo logEvent )
        {
            var tagologContext = MdcStorage.GetContext();

            IDictionary<string, string> logEventTags = new Dictionary<string, string>();
            IDictionary<string, string> builtInTags = new Dictionary<string, string>();

            if ( !string.IsNullOrWhiteSpace( tagologContext ) )
                TagSerializer.TagsFromString( tagologContext, out logEventTags, out builtInTags );

            new LogEntryRepository( _dbConnectionFactory, _queryCommandTextRepository ).Insert(
                ApplicationCode,
                ComposeIdentityName( logEvent ),
                EnumHelpers.MapEnum<DbLogLevel, global::NLog.LogLevel>( logEvent.Level ),
                MessageLayout.Render( logEvent ),
                ExceptionLayout.Render( logEvent ),
                logEventTags );

            return;

            //    var messageId = Guid.NewGuid();

            //    {
            //        ProcessTags(...)

            //        context.Messages
            //            .Insert( () =>
            //                item.ConvertToDb() так нельзя, объект должен иметь возможность преобразовываться в Expression<Func<T>>
            //                new MessageDbObject
            //                {
            //                    Id = msgId,
            //                    CustomerId = app.CustomerId,
            //                    ApplicationId = app.Id,
            //                    LevelId = item.Level.ConvertToDbValue(),
            //                    Created = item.Created,
            //                    Logged = DateTime.UtcNow, //item.Logged,
            //                    LogUserIdentity = item.UserIdentity,
            //                    LogIPAddress = item.LogIPAddress,
            //                    RequestIPAddress = item.RequestIPAddress,
            //                    MessageText = item.MsgText,
            //                    ExceptionText = item.ExceptionText,
            //                    SphinxTagsText = tagsTextForSphinx.ToString(),
            //                    CorrelationId = item.CorrelationId,
            //                    PacketLength = item.PacketLength,
            //                    CreatedTicks = item.Created.Ticks
            //                } );

            //        if ( item.Tags != null && item.Tags.Count > 0 )
            //        {
            //            foreach ( KeyValuePair<string, string> tagDataPair in dictProcessedTagData )
            //            {
            //                Tag tag = dictUsedTags[ tagDataPair.Key ];

            //                context.MessageTags
            //                    .Insert( () =>
            //                        item.ConvertToDb() так нельзя, объект должен иметь возможность преобразовываться в Expression<Func<T>>
            //                        new MessageTagDbObject
            //                        {
            //                            MessageId = msgId,
            //                            TagId = tag.Id,
            //                            CustomerId = app.CustomerId,
            //                            TagValue = tagDataPair.Value
            //                        }
            //                    );
            //            }
            //        }
            //        trnScope.Complete();
            //        context.CommitTran();
            //    }
            //}


            //this.EnsureConnectionOpen( this.BuildConnectionString( logEvent ) );

            //IDbCommand command = this.activeConnection.CreateCommand();
            //command.CommandText = this.CommandText.Render( logEvent );

            //InternalLogger.Trace( "Executing {0}: {1}", command.CommandType, command.CommandText );

            //foreach ( DatabaseParameterInfo par in this.Parameters )
            //{
            //    IDbDataParameter p = command.CreateParameter();
            //    p.Direction = ParameterDirection.Input;
            //    if ( par.Key != null )
            //    {
            //        p.ParameterName = par.Key;
            //    }

            //    if ( par.Size != 0 )
            //    {
            //        p.Size = par.Size;
            //    }

            //    if ( par.Precision != 0 )
            //    {
            //        p.Precision = par.Precision;
            //    }

            //    if ( par.Scale != 0 )
            //    {
            //        p.Scale = par.Scale;
            //    }

            //    string stringValue = par.Layout.Render( logEvent );

            //    p.Value = stringValue;
            //    command.Parameters.Add( p );

            //    InternalLogger.Trace( "  Parameter: '{0}' = '{1}' ({2})", p.ParameterName, p.Value, p.DbType );
            //}

            //int result = command.ExecuteNonQuery();
            //InternalLogger.Trace( "Finished execution, result = {0}", result );
        }

        /// <summary>
        /// Writes an array of logging events to the log target. By default it iterates on all
        /// events and passes them to "Write" method. Inheriting classes can use this method to
        /// optimize batch writes.
        /// </summary>
        /// <param name="logEvents">Logging events to be written out.</param>
        protected override void Write( AsyncLogEventInfo[] logEvents )
        {
            throw new NotSupportedException( "Async calls are not supported yet." );
        }

        string ComposeIdentityName( LogEventInfo src )
        {
            return new WindowsIdentityLayoutRenderer().Render(
                new LogEventInfo(
                    src.Level,
                    src.LoggerName,
                    "${windows-identity:name}" ) );
        }

        DbConnectionFactory _dbConnectionFactory;
        IQueryCommandTextRepository _queryCommandTextRepository;

        readonly string DefaultMessageLayout = Settings.Default.DefaultMessageLayout;
        readonly string DefaultExceptionLayout = Settings.Default.DefaultExceptionLayout;
    }
}
