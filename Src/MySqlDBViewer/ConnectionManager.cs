using System;
using System.Collections.Generic;
using System.Text;
using DBViewer.Model.Core;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

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
            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            {
                conn.Open();
                MySqlDataAdapter adp = new MySqlDataAdapter(sql, conn);
                adp.Fill(table);
                conn.Close();
            }
            return table;
        }



        public void ExecuteCmd(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            {
                
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        private string GetConnectionString()
        {
            //Server=127.0.0.1;Uid=root;Pwd=12345;Database=test;
            return string.Format("Data Source={0};User Id={1};Password={2};"
                            ,config.dbname
                            ,config.user
                            ,config.password);

        }
    }
}
