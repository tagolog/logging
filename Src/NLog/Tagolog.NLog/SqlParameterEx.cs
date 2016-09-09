using System;
using System.Data;
using System.Data.SqlClient;

namespace Tagolog.NLog.DatabaseTarget
{
    /// <summary>
    /// Extends generic "SqlParameter" functionality.
    /// </summary>
    public static class SqlParameterEx
    {
        /// <summary>
        /// Creates new SqlParameter instance with value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to map.</param>
        /// <param name="dbType">One of the SqlDbType values.</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>New SqlParameter instance.</returns>
        public static SqlParameter CreateWithValue( string parameterName, SqlDbType dbType, int size, object value )
        {
            // Create new parameter.
            SqlParameter parameter = new SqlParameter( parameterName, dbType, size );

            // Set parameter value.
            if ( null != value )
                parameter.Value = value;
            else
                // Handle NULL values.
                parameter.Value = DBNull.Value;

            return parameter;
        }
    }
}
