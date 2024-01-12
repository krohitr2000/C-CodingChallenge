using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace LoanManagement.util
{
    public static class DBUtil
    {
        public static SqlConnection GetDBConn()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            return conn;
        }
    }
}
