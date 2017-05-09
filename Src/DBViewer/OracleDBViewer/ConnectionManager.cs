using System;
using System.Collections.Generic;
using System.Text;
using DBViewer.Model.Core;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace DBViewer.Model.Oracle
{
    class ConnectionManager
    {
        public DBViewerConfig config;

        public ConnectionManager(DBViewerConfig config)
        {
            this.config = config;
        }

        public DataTable GetData(string sql)
        {
            DataTable table = new DataTable();
            using (OracleConnection conn = new OracleConnection(GetConnectionString()))
            {
                conn.Open();
                OracleDataAdapter adp = new OracleDataAdapter(sql, conn);
                adp.Fill(table);
                conn.Close();
            }
            return table;
        }



        public void ExecuteCmd(string sql)
        {
            using (OracleConnection conn = new OracleConnection(GetConnectionString()))
            {
                
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        private string GetConnectionString()
        {
            return string.Format("Data Source={0};User Id={1};Password={2};"
                            ,config.dbname
                            ,config.user
                            ,config.password);

        }
    }
}
