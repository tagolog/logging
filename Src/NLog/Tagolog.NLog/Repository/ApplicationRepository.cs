using System;
using System.Data;
using System.Collections.Generic;
using Tagolog.Internal;
using Tagolog.NLog.Interface;

namespace Tagolog.NLog.Repository
{
    class ApplicationRepository : IApplicationRepository
    {
        public ApplicationRepository( DbRepository dbRepository, IQueryCommandTextRepository queryCommandTextRepository )
        {
            _dbRepository = dbRepository;
            _queryCommandTextRepository = queryCommandTextRepository;
        }

        public ApplicationEntity GetByCode( Guid applicationCode )
        {
            ApplicationEntity applicationEntity = null;
            var query = new SqlQuery
            {
                CommandText = _queryCommandTextRepository.GetCommandText( QueryCommandType.ApplicationSelect ),
                Parameters = new List<SqlQueryParameter>
                {
                    new SqlQueryParameter( "@applicationcode", DbType.Guid, 0, applicationCode )
                }
            };

  //ApplicationId uuid DEFAULT uuid_generate_v1() PRIMARY KEY,
  //CustomerId uuid NOT NULL REFERENCES Log.Customer(CustomerId),
  //Code uuid NOT NULL DEFAULT uuid_generate_v1() UNIQUE, -- Auto-generated key which is an OutsideId
  //Key varchar(128) NOT NULL,
  //IsEnabled boolean NOT NULL DEFAULT TRUE,
  //Description varchar(512) NULL,
  //EncryptionKey varchar(128) NULL, -- All info about encryption
  //ApplicationRowId int8 DEFAULT nextval('Log.seq_ApplicationRowId')  -- SphinxIdx

            _dbRepository.ExecuteReader( query, delegate( IDataReader reader )
            {
                var id = ( Guid ) reader[ "ApplicationId" ];
                var rowId = ( long ) reader[ "ApplicationRowId" ];
                var customerId = ( Guid ) reader[ "CustomerId" ];
                var code = ( Guid ) reader[ "Code" ];
                var name = ( string ) reader[ "Name" ];
                var description = ( string ) reader[ "Description" ];
                var enabled = ( bool ) reader[ "IsEnabled" ];

                applicationEntity = new ApplicationEntity()
                {
                    Id = id,
                    RowId = rowId,
                    CustomerId = customerId,
                    Code = code,
                    Name = name,
                    Description = description,
                    IsEnabled = enabled
                };
            } );

            if ( null == applicationEntity )
                throw new Exception( string.Format( "Application was not found. [applicationCode={0}]", applicationCode ) );

            return applicationEntity;
        }

        readonly DbRepository _dbRepository;
        readonly IQueryCommandTextRepository _queryCommandTextRepository;
    }
}
