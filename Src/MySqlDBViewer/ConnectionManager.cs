using System;
using System.Collections.Generic;
using System.Text;
using DBViewer.Model.Core;
using System.Data;
using System.Data.SqlClient;

namespace DBViewer.Model.MySQL
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
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                SqlDataAdapter adp = new SqlDataAdapter(sql, conn);
                adp.Fill(table);
                conn.Close();
            }
            return table;
        }



        public void ExecuteCmd(string sql)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        private string GetConnectionString()
        {
           return string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3}"
                            ,config.server
                            ,config.dbname
                            ,config.user
                            ,config.password);

        }
    }
}
