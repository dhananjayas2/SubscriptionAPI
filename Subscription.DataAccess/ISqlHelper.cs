using System.Data;
using System.Data.SqlClient;

namespace Subscription.DataAccess
{
   public interface ISqlHelper
    {
        DataSet ExecuteDataSet(string selectStatement);

        DataSet ExecuteDataSet(string commandText, SqlParameter[] sqlParameter);

        bool ExecuteNonQuery(string sqlStatement);

        bool ExecuteNonQuery(string sqlStatement, SqlParameter[] sqlParameter);
    }
}
