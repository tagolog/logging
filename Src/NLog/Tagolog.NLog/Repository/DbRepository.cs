using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Tagolog.NLog.Repository
{
    internal class SqlQueryParameter
    {
        public SqlQueryParameter( string name, DbType type, int size, object value )
        {
            Name = name;
            Type = type;
            Size = size;
            Value = value;
        }

        public string Name { get; set; }
        public DbType Type { get; set; }
        public int Size { get; set; }
        public object Value { get; set; }
    }

    internal class SqlQuery
    {
        public SqlQuery()
        {
            Parameters = new List<SqlQueryParameter>();
        }

        public IList<SqlQueryParameter> Parameters { get; set; }
        public string CommandText { get; set; }
    }

    /// <summary>
	/// Read current portion of data from data reader.
	/// </summary>
	/// <param name="reader">Data reader instance.</param>
	/// <remarks>Don't use reader.Next() method inside this method.</remarks>
    internal delegate void ReadCurrent( IDataReader reader );

	/// <summary>
	/// Switch to next result set.
	/// </summary>
	/// <param name="reader">Data reader instance.</param>
	/// <remarks>
	/// Queries can return multiple result sets. This method called each time new result set is found.
	/// </remarks>
    internal delegate void NextResultSet( IDataReader reader );

    internal class DbRepository
    {
        #region Initialization

        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="dbConnection">Database connections.</param>
        public DbRepository( DbConnection dbConnection )
        {
            _dbConnection = dbConnection;
        }

        #endregion // Initialization

        public void ExecuteReader( SqlQuery query, ReadCurrent loadData )
        {
            // Assert and throw if load data	parameter is null.
            #region Assert and throw if load data	parameter is null

            if ( null == loadData )
            {
                string message = "Handler that accept data to load is not set.";
                throw new ArgumentNullException( "loadData", message );
            }

            #endregion // Assert and throw if load data	parameter is null

            ExecuteGeneric( query, loadData, null );
        }

        public void ExecuteReader( SqlQuery query, ReadCurrent loadData, NextResultSet nextResultSet )
        {
            // Assert and throw if load data parameter is null.
            #region Assert and throw if load data	parameter is null

            if ( null == loadData )
            {
                string message = "Handler that accept data to load is not set.";
                Debug.Assert( null != loadData, message );
                throw new ArgumentNullException( "loadData", message );
            }

            #endregion // Assert and throw if load data	parameter is null

            // Assert and throw if next result parameter is null.
            #region Assert and throw if next result	parameter is null

            if ( null == nextResultSet )
            {
                string message = "Handler that accept next result set is not set.";
                Debug.Assert( null != nextResultSet, message );
                throw new ArgumentNullException( "nextResultSet", message );
            }

            #endregion // Assert and throw if next result	parameter is null

            ExecuteGeneric( query, loadData, nextResultSet );
        }

        /// <summary>
        /// Execute non select queries like insert/update/delete/stored procedures.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        public void ExecuteNonQuery( SqlQuery query )
        {
            ExecuteGeneric( query, null, null ); // "null" at the end means no data to return.
        }

        #region Helpers

        /// <summary>
        /// Execute generic query.
        /// </summary>
        /// <param name="query">SQL query to execute.</param>
        /// <param name="loadData">Handler that accepts data load.</param>
        /// <param name="nextResultSet">Handler that accepts next result set.</param>
        void ExecuteGeneric( SqlQuery query, ReadCurrent loadData, NextResultSet nextResultSet )
        {
            // Allocate new connection.
            // using ( var connection = _dbConnectionFactory.CreateConnection() )
            //{
                ExecuteCommand( _dbConnection, query, loadData, nextResultSet );
            //}
        }

        void ExecuteCommand( DbConnection connection, SqlQuery query, ReadCurrent loadData, NextResultSet nextResultSet )
        {
            // Allocate new command to execute stored procedure.
            using ( var command = connection.CreateCommand() )
            {
                command.CommandText = query.CommandText;
                // Specify that command will be a stored procedure.
                command.CommandType = CommandType.Text;

                // Append external parameters.
                if ( null != query.Parameters && 0 != query.Parameters.Count )
                {
                    foreach ( var parameter in query.Parameters )
                    {
                        var commandParameter = command.CreateParameter();

                        commandParameter.ParameterName = parameter.Name;
                        commandParameter.Value = parameter.Value;
                        commandParameter.DbType = parameter.Type;

                        command.Parameters.Add( commandParameter );
                    }
                }

                // Append additional parameter - return value.
                // command.Parameters.Add( new SqlParameter( Parameter.ReturnValue, SqlDbType.Int, 4 ) );
                // command.Parameters[ Parameter.ReturnValue ].Direction = ParameterDirection.ReturnValue;

                // connection.Open();

                // Ready to execute.
                // Makes a decision what kind of query to execute
                // Either select query that returns data to be read or terminal query with no results.
                ExecuteReaderOrNonQuery( command, loadData, nextResultSet );
            }
        }

        /// <summary>
        /// Makes a decision what kind of query to execute (either select query that returns data to be read or terminal query with no results).
        /// </summary>
        /// <param name="command">Command instance.</param>
        /// <param name="loadData">Handler that accepts data load.</param>
        void ExecuteReaderOrNonQuery( DbCommand command, ReadCurrent loadData, NextResultSet nextResultSetDelegate )
        {
            // Set command timeout.
            // command.CommandTimeout = Properties.Settings.Default.SqlCommandTimeout;

            if ( null == loadData )
            {
                // Just simple execute with no results.
                command.ExecuteNonQuery();
            }
            else
            {
                // Create data reader instance for select like commands.
                using ( var reader = command.ExecuteReader() )
                {
                    bool nextResult;
                    do
                    {
                        while ( reader.Read() )
                        {
                            loadData( reader );
                        }

                        // Switch to next result set in case of several result set returned by SQL command.
                        nextResult = reader.NextResult();

                        // Notify external code by a call to "NextResultSet" delegate.
                        if ( nextResult && null != nextResultSetDelegate )
                            nextResultSetDelegate( reader );
                    }
                    while ( nextResult );
                }
            }
        }

        #endregion // Helpers

        #region Data

        readonly DbConnection _dbConnection;

        /// <summary>
        /// Names of stored procedure parameters.
        /// </summary>
        static class Parameter
        {
            public static readonly string ReturnValue = "@RETURN_VALUE";
        }

        #endregion // Data
    }
}
