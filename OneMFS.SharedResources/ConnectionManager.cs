using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OneMFS.SharedResources
{
    public class ConnectionManager
    {
        public string GetConnectionString()
        {
            //string connString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.156.0.186)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=mfsdb)));User Id = mfs; Password = MFSDB@12345; ";  // -- development test database
            //string connString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.156.0.186)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=mfsdb)));User Id = one; Password = one123; "; // -- development UAT Existing Database
            //string connString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.156.4.67)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=okdb)));User Id = one; Password = 123; "; // -- Live migrated copy DB 11g
            string connString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.156.4.69)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=okdb)));User Id = one; Password = one;" +
				" Connection Timeout = 15;Decr Pool Size = 1; Incr Pool Size = 5;Max Pool Size = 500;Min Pool Size = 1 "; // --Latest live migrated copy DB for 12c
            return connString;
        }

        public IDbConnection GetConnection()
        {
            var conn = new OracleConnection(GetConnectionString());
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        public void CloseConnection(IDbConnection conn)
        {
            if (conn.State == ConnectionState.Open || conn.State == ConnectionState.Broken)
            {
                conn.Close();
            }
        }
    }
}
