using System.Data;
using System.Data.SqlClient;

namespace Subscription.DataAccess
{
    public class SqlHelper : ISqlHelper
    {
        const string strConString = @"Server=localhost\SQLEXPRESS;Database=Subscription;Trusted_Connection=True;";

        private SqlConnection conn = new SqlConnection(strConString);


        // ExecuteDataSet(DbCommand) Executes the command and returns the results in a new DataSet.
        public DataSet ExecuteDataSet(string selectStatement)
        {
            SqlDataAdapter sda = new SqlDataAdapter(selectStatement, this.conn);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;

        }

        // ExecuteDataSet(String, Object)  Executes the storedProcedureName with parameterValues and returns the results in a new DataSet.
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] sqlParameter)
        {
            SqlCommand cmd = new SqlCommand(commandText, this.conn);
            // how do we know that we have to use commandtype property or parameters ???
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddRange(sqlParameter);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public bool ExecuteNonQuery(string sqlStatement)
        {
            try
            {
                this.conn.Open();
                var beginTransaction = this.conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand(sqlStatement, this.conn);
                cmd.ExecuteNonQuery();
                beginTransaction.Commit();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }

            finally
            {
                if (this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                    this.conn.Dispose();
                }

            }
        }

        public bool ExecuteNonQuery(string sqlStatement, SqlParameter[] sqlParameter)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, this.conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlParameter);
                this.conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }

            finally
            {
                if (this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                    this.conn.Dispose();
                }
            }
        }

    }
}
