using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace StudentEnrollment.Function.Domain
{
    public interface ISqlDataContext
    {
        Task ExecuteQueryAsync(string storeProcedureName, IEnumerable<SqlParameter> parameters = null);
        IEnumerable<IDataRecord> ExecuteReader(string storeProcedureName, IEnumerable<SqlParameter> parameters = null);
    }
}
