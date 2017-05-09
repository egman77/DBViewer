using System;
using System.Collections.Generic;
using System.Text;
using DBViewer.Model.Core;
using System.Data;
using System.Windows.Forms;

namespace DBViewer.Model.SQLServer
{
    class DBViewerModel:IDBViewerModel 
    {
        private const string TR_PREFIX = "dbvtr_";
        private const string TABLENAME = "dbv_LOGDATA";
        #region IDBViewerModel 成员

        private ConnectionManager m_cm;
        private DBViewerConfig m_config;
        public DBViewerConfig Coinfig
        {
            get { return m_config; }
            set { m_config = value;
            m_cm = new ConnectionManager(m_config);
            }

        }

     
        public void DeleteTrigger(string tableName)
        {
            DeleteTrigger(m_cm, tableName);
        }

        public void ReBuildTrigger(string tableName)
        {

            DataTable tableColumns = GetTableColumns(m_cm,tableName);

            DataRow[] rows = tableColumns.Select("typeName in ('text','ntext','image')");
            if (rows.Length > 0)
            {
                throw new Exception(string.Format("无法创建表{0}的触发器.表不能包含text,ntext,image字段",tableName));
            }
            else
            {
                BuildTrigger(m_cm, tableName, tableColumns);
            }
        
        }

        private DataTable GetTableColumns(ConnectionManager cm, string tableName)
        {
            string sql = " select c.name,t.name as typeName from \n"
                        + "sys.types t\n"
                        + "inner join sys.columns c on t.system_type_id = c.system_type_id\n"
                        + "where c.object_id = object_id('{0}') and t.name not in ('text','ntext','image')\n";

            return cm.GetData(string.Format(sql, tableName));
        }

        private void BuildTrigger(ConnectionManager cm,string tableName,DataTable tableColumns)
        {
            DeleteTrigger(cm, tableName);


            StringBuilder sqlBuilder = new StringBuilder();

            string tableFieldValue = GetTableValue(tableColumns);
            string pkValue = GetPKValue(cm, tableColumns, tableName);
            string userFieldName = GetSafeUserFieldName(tableColumns);

            sqlBuilder.AppendFormat("create trigger dbvtr_{0} on {0} for INSERT,DELETE,UPDATE AS \n",tableName);
            sqlBuilder.Append("BEGIN \n");
            sqlBuilder.Append("declare @status int \n");
            sqlBuilder.Append(" select @status = 2 \n");
            sqlBuilder.Append("if not exists(select * from inserted) \n");
            sqlBuilder.Append(" select @status = 3  \n");
            sqlBuilder.Append("if not exists(select * from deleted) \n");
            sqlBuilder.Append(" select @status = 1 \n");

            //Insert 语句记录
            sqlBuilder.Append("IF (@status = 1 or @status = 2)\n");
            sqlBuilder.Append("begin\n");
            sqlBuilder.Append("   insert into dbv_LOGDATA (RecDate,tableName,tabletype,status,PK,data,updateuser)\n");
            sqlBuilder.AppendFormat("   select getdate(),'{3}',0,@status,{0},{1},{2}\n", pkValue, tableFieldValue, userFieldName, tableName);
            sqlBuilder.AppendFormat("   from inserted\n");
            sqlBuilder.Append("end\n");


            //Delete 语句记录
            sqlBuilder.Append("IF (@status = 3 or @status = 2)\n");
            sqlBuilder.Append("begin\n");
            sqlBuilder.AppendFormat("   insert into dbv_LOGDATA (RecDate,tableName,tabletype,status,PK,data,updateuser)\n");
            sqlBuilder.AppendFormat("   select getdate(),'{3}',1,@status,{0},{1},{2}\n", pkValue, tableFieldValue, userFieldName, tableName);
            sqlBuilder.Append("   from deleted\n");
            sqlBuilder.Append("end\n");


            sqlBuilder.Append("END \n");

            cm.ExecuteCmd(sqlBuilder.ToString());


        }

        private static void DeleteTrigger(ConnectionManager cm, string tableName)
        {
            cm.ExecuteCmd(string.Format(" IF EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[{1}{0}]'))"
                + " drop trigger {1}{0} ", tableName, TR_PREFIX));
        }

        private string GetSafeUserFieldName(DataTable tableColumns)
        {
            if (this.Coinfig.UserFieldName != string.Empty)
            {
                DataRow[] rows = tableColumns.Select("name = '" + this.Coinfig.UserFieldName + "'");
                if (rows.Length > 0)
                {
                    return this.Coinfig.UserFieldName;
                }
            }

            return "' '";

            
        }

     


        private string GetPKValue(ConnectionManager cm,DataTable tableColumns, string tableName)
        {
            string sql = string.Format("sp_pkeys '{0}'", tableName);
            DataTable pkData = cm.GetData(sql);

            if (pkData.Rows.Count > 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                foreach (DataRow row in pkData.Rows)
                {
                    string columnName = (string)row["COLUMN_NAME"];
                    DataRow[] columnRows = tableColumns.Select("name = '" + columnName + "'");

                    FillFieldValueString(sqlBuilder, columnRows[0]);
                }

                string ret = sqlBuilder.ToString();
                return ret.Substring(1);
            }
            else
            {
                return "''";
            }

        }

        private static void FillFieldValueString(StringBuilder sqlBuilder,DataRow row)
        {
            sqlBuilder.AppendFormat("+ isnull('{0}=' + convert(varchar,[{0}]) + ';','')", row["name"]);
           
        }




        private string GetTableValue(DataTable table)
        {
            StringBuilder sql = new StringBuilder();
            foreach (DataRow row in table.Rows )
            {
                FillFieldValueString(sql, row);
            }
            string ret = sql.ToString();
            return ret.Substring(1);
        }



        public DataTable GetTraceData(string userName, DateTime startTime)
        {
            string sql = "select * from {0} where (updateuser = '{1}' or updateuser= ' ') and RecDate >= '{2}'";
            
            return m_cm.GetData(string.Format(sql,TABLENAME,userName,startTime.ToString("yyyy/MM/dd HH:mm:ss")));
        }

        public void ClearTraceData(string userName)
        {
            try
            {
                m_cm.ExecuteCmd("delete from  " + TABLENAME + " where (UpdateUser = '" + userName + "' or updateuser= ' ' )");
            }
            catch { }
        }

        private void EnsureCreateTable(ConnectionManager cm)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("if exists (select * from sys.objects where name = '{0}' and type = 'u') \n", TABLENAME);
            sql.AppendFormat("drop table {0} \n", TABLENAME);
            sql.AppendFormat(" create table {0} (\n"
                            + " SeqNo	int identity not null  \n"
                             + " ,tabletype int  \n"
                            + " ,RecDate datetime   \n"
                            + " ,tablename	varchar(40)  \n"
                            + " ,status	int  \n"
                            + " ,PK	varchar(200) \n"
                            + " ,Data	varchar(3000) \n"
                            + " ,UpdateUser	varchar(20) not null default('  ') \n"
                            + " PRIMARY KEY (SeqNo) \n"
                            + " ) "
                              , TABLENAME);

            cm.ExecuteCmd(sql.ToString());

        }
 

        public DataTable GetTableList()
        {
            DataTable table = m_cm.GetData(string.Format("select name from sys.objects\n"
                                                +" where type = 'u' and name <> '{0}' order by name "
                                                ,TABLENAME));
            return table;
        }

        public void CreateTraceTable()
        {
            EnsureCreateTable(m_cm);
        }

        #endregion
    }
}
