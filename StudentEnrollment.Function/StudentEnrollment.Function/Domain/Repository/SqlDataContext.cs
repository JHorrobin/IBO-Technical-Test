using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain.Repository
{
    [ExcludeFromCodeCoverage]
    public class SqlDataContext : ISqlDataContext
    {
        public async Task ExecuteQueryAsync(string storeProcedureName, IEnumerable<SqlParameter> parameters = null)
        {
            var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            SqlConnection connection = null;

            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    
                    using var command = new SqlCommand(storeProcedureName, connection) 
                    { 
                        CommandType = CommandType.StoredProcedure 
                    };

                    if (parameters != null)
                    {
                        this.AddParameters(command, parameters);
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
            finally
            {
                await connection?.CloseAsync();
            }
        }

        public IEnumerable<IDataRecord> ExecuteReader(string storeProcedureName, IEnumerable<SqlParameter> parameters = null)
        {
            var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            SqlConnection connection = null;

            try
            {
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    using var command = new SqlCommand(storeProcedureName, connection) 
                    { 
                        CommandType = CommandType.StoredProcedure 
                    };
                    
                    if(parameters != null)
                    {
                        this.AddParameters(command, parameters);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return reader;
                        }
                    }
                }
            }
            finally
            {
                connection?.Close();
            }
        }

        private void AddParameters(SqlCommand command, IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }
    }
}
