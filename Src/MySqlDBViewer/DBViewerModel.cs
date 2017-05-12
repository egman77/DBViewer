using System;
using System.Collections.Generic;
using System.Text;
using DBViewer.Model.Core;
using System.Data;
using System.Windows.Forms;
using System.Net;

namespace DBViewer.Model.MySql
{
    class DBViewerModel:IDBViewerModel 
    {
        private const string TR_PREFIX = "TR_";
        private const string TABLENAME = "DBV_LOGDATA";
       // private const string SEQUENCES = "DBVSEQUENCES";
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

       /// <summary>
       /// 删除触发器
       /// </summary>
       /// <param name="tableName"></param>
        public void DeleteTrigger(string tableName)
        {
            DeleteTrigger(m_cm, tableName);
        }

        /// <summary>
        ///  重建触发器
        /// </summary>
        /// <param name="tableName"></param>
        public void ReBuildTrigger(string tableName)
        {

            DataTable tableColumns = GetTableColumns(m_cm,tableName);

            DataRow[] rows = tableColumns.Select("typeName in ('long')");
            if (rows.Length > 0)
            {
                throw new Exception(string.Format("无法创建表{0}的触发器.表不能包含text,ntext,image字段",tableName));
            }
            else
            {
                BuildTrigger(m_cm, tableName, tableColumns);
            }
        
        }

        /// <summary>
        /// 返回指定表的列信息
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private DataTable GetTableColumns(ConnectionManager cm, string tableName)
        {
            //string sql = " select t.column_name as name,t.data_type as typeName from \n"
            //            + "user_tab_cols t \n"
            //            + "where t.table_name = '{0}' and t.data_type not in ('long')\n";
            string sql = "select COLUMN_NAME as name , DATA_TYPE as typeName from information_schema.columns where  TABLE_SCHEMA='{0}' and table_name = '{1}' ";

            return cm.GetData(string.Format(sql,cm.config.DbName, tableName));
        }

        private string GetTriggerName(string tableName)
        {
            string triggerName = TR_PREFIX + tableName.ToUpper();
            if (triggerName.Length > 30)
            {
                triggerName = TR_PREFIX + tableName.Substring(tableName.Length - 30 + TR_PREFIX.Length).ToUpper();
            }

            return triggerName;
        }
        private void BuildTrigger(ConnectionManager cm,string tableName,DataTable tableColumns)
        {
            string triggerName = GetTriggerName(tableName);

            DeleteTrigger(cm, triggerName);


            StringBuilder sqlBuilder = new StringBuilder();

            string newTableFieldValue = GetTableValue(tableColumns, "NEW");
            string oldTableFieldValue = GetTableValue(tableColumns, "OLD");
            string newPKValue = GetPKValue(cm, tableColumns, tableName, "NEW");
            string oldPKValue = GetPKValue(cm, tableColumns, tableName, "OLD");

            var TracerUser = cm.config.TraceUser??GetIP(); //如果没有,则用客户机IP代替
            //string newUserFieldName = " ";
            //string oldUserFieldName = " ";
            //string newUserFieldName = GetSafeUserFieldName(tableColumns,"NEW");
            //string oldUserFieldName = GetSafeUserFieldName(tableColumns, "OLD");

            //1--insert 2--update 3--delete ,DELETE,UPDATE
            //Insert 语句记录
            sqlBuilder.AppendFormat("create trigger {0}_insert after INSERT  on {1} for each row \n", triggerName, tableName);
            sqlBuilder.Append("BEGIN \n");    
            //RecDate,tableName,tabletype,status,PK,data,updateuser
            sqlBuilder.AppendFormat("   insert into {4}(RecDate,tableName,tabletype,status,PK,data,updateuser) values( now(),'{3}',{6},{5},{0},{1},'{2}');\n", newPKValue, newTableFieldValue, TracerUser, tableName, TABLENAME, EnumOperatorType.New,EnumTableType.None);
            sqlBuilder.Append("END;\n");

            //Update 语句记录
            sqlBuilder.AppendFormat("create trigger {0}_update after Update  on {1} for each row \n", triggerName, tableName);
            sqlBuilder.Append("BEGIN \n");
            //注意,用tabletype与status来共同来识别 是更新的新值
            sqlBuilder.AppendFormat("   insert into {4}(RecDate,tableName,tabletype,status,PK,data,updateuser) values( now(),'{3}',{6},{5},{0},{1},'{2}');\n", newPKValue, newTableFieldValue, TracerUser, tableName, TABLENAME, EnumOperatorType.Update,EnumTableType.Inserted);
            //注意,用tabletype与status来共同来识别 是更新的旧值
            sqlBuilder.AppendFormat("   insert into {4}(RecDate,tableName,tabletype,status,PK,data,updateuser) values( now(),'{3}',{6},{5},{0},{1},'{2}');\n", oldPKValue, oldTableFieldValue, TracerUser, tableName, TABLENAME, EnumOperatorType.Update,EnumTableType.Deleted);
            sqlBuilder.Append("END;\n");

            //Delete 语句记录
            sqlBuilder.AppendFormat("create trigger {0}_delete after Delete  on {1} for each row \n", triggerName,tableName);         
            sqlBuilder.Append("BEGIN \n");
            sqlBuilder.AppendFormat("   insert into {4}(RecDate,tableName,tabletype,status,PK,data,updateuser) values( now(),'{3}',{6},{5},{0},{1},'{2}');\n", oldPKValue, oldTableFieldValue, TracerUser, tableName, TABLENAME,EnumOperatorType.Delete, EnumTableType.None);
            sqlBuilder.Append("END; \n");

      
            cm.ExecuteCmd(sqlBuilder.ToString());


        }

        /// <summary>
        /// 删除触发器
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="triggerName"></param>
        private static void DeleteTrigger(ConnectionManager cm, string triggerName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("drop trigger if exists {0};\n", triggerName);
            sql.AppendFormat("drop trigger if exists {0}_insert;\n", triggerName);
            sql.AppendFormat("drop trigger if exists {0}_update;\n", triggerName);
            sql.AppendFormat("drop trigger if exists {0}_delete;\n", triggerName);


            cm.ExecuteCmd(sql.ToString());
        }

        //private string GetSafeUserFieldName(DataTable tableColumns,string perfix)
        //{
        //    if (this.Coinfig.UserFieldName != string.Empty)
        //    {
        //        DataRow[] rows = tableColumns.Select("name = '" + this.Coinfig.UserFieldName + "'");
        //        if (rows.Length > 0)
        //        {
        //            return ":" + perfix + "." + this.Coinfig.UserFieldName;
        //        }
        //    }

        //    return "' '";

            
        //}

        private string GetPKValue(ConnectionManager cm,DataTable tableColumns, string tableName,string perfix)
        {
            //string sql = string.Format("SELECT t1.COLUMN_NAME FROM USER_CONS_COLUMNS t1\n"
            //                + "  INNER JOIN USER_CONSTRAINTS t2 ON t1.TABLE_NAME = t2.TABLE_NAME and t1.CONSTRAINT_NAME = t2.CONSTRAINT_NAME \n"
            //                + "  WHERE t1.TABLE_NAME = '{0}' and t2.CONSTRAINT_TYPE = 'P' \n"
            //                + "  ORDER BY t1.POSITION", tableName);

            string sql = "select COLUMN_NAME from information_schema.columns \n"+
                $"where  TABLE_SCHEMA='{cm.config.DbName}' and table_name = '{tableName}' and COLUMN_KEY='PRI'"+
                "ORDER BY ORDINAL_POSITION";

            DataTable pkData = cm.GetData(sql);

            if (pkData.Rows.Count > 0)
            {
                StringBuilder sqlBuilder = new StringBuilder();
                foreach (DataRow row in pkData.Rows)
                {
                    string columnName = (string)row["COLUMN_NAME"];
                    DataRow[] columnRows = tableColumns.Select("name = '" + columnName + "'");

                    
                    FillFieldValueString(sqlBuilder, columnRows[0],perfix);
                }

                string ret = sqlBuilder.ToString();
                return  $"concat({ret.Substring(2)})";
            }
            else
            {
                return "''";
            }

        }

        /// <summary>
        /// 创建字段值的字符串表示
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="row"></param>
        /// <param name="perfix"></param>
        private static void FillFieldValueString(StringBuilder sqlBuilder,DataRow row,string perfix)
        {
            // sqlBuilder.AppendFormat("|| NVL('{0}=' || substr(to_char(:{1}.{0}),0,20) || ';','')", row["name"],perfix);
            sqlBuilder.AppendFormat(", ifnull(nullif(concat('{0}=' , convert({1}.{0},char) , ';'),'{0}=;'),'')", row["name"],perfix);
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
        private string GetTableValue(DataTable table, string perfix)
        {
            StringBuilder sql = new StringBuilder();
            foreach (DataRow row in table.Rows)
            {
                FillFieldValueString(sql, row, perfix);
            }
            string ret = sql.ToString();
            return $"concat({ret.Substring(2)})";
        }


        public void RemoveTrigger()
        {
            throw new NotImplementedException();
        }

        public DataTable GetTraceData(string userName, DateTime startTime)
        {
           // string ip = GetIP();
            var tracerUser = userName ?? GetIP(); //如果没有,则用客户机IP代替
            //string sql = "SELECT * FROM {0} WHERE (UPDATEUSER = '{1}' OR UPDATEUSER= ' ') AND RECDATE >= TO_DATE('{2}','yyyy/mm/dd hh24:mi:ss') order by SEQNO";
            string sql = "SELECT * FROM {0} WHERE (UPDATEUSER = '{1}' ) AND RECDATE >= DATE_FORMAT('{2}','%Y/%m/%d %H:%i:%s') order by SEQNO";

            return m_cm.GetData(string.Format(sql, TABLENAME, tracerUser, startTime.ToString("yyyy/MM/dd HH:mm:ss")));
        }

        private string GetIP()
        {
            IPAddress[] ipAddresses = Dns.GetHostAddresses(Dns.GetHostName()); 
            foreach(IPAddress ip in ipAddresses)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
        public void ClearTraceData(string userName)
        {
            try
            {
                var tracerUser = userName ?? GetIP(); //如果没有,则用客户机IP代替

                m_cm.ExecuteCmd("DELETE FROM  " + TABLENAME + " WHERE (UpdateUser = '" + tracerUser + "' )");
            }
            catch { }
        }

        /// <summary>
        /// 删除和创建指定的表
        /// </summary>
        /// <param name="cm"></param>
        private void EnsureCreateTable(ConnectionManager cm)
        {
            
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat("DROP TABLE IF EXISTS {0}",TABLENAME);
          
            cm.ExecuteCmd(sql.ToString());

            sql = new StringBuilder();
            sql.AppendFormat("  CREATE  TABLE {0} ( \n"
                     + " SEQNO  int auto_increment  NOT NULL\n"                    
                     + ",RecDate DATE\n"
                     + ",tablename  varchar(40)  \n"
                     + ",TABLETYPE int  \n"
                     + ",status  int  \n"
                     + ",PK  VARCHAR(200) \n"
                     + ",Data  VARCHAR(3000) \n"
                     + ",UpdateUser  VARCHAR(20) not null \n"
                     + ",CONSTRAINT PK_DGV_LOGDATA PRIMARY KEY  (SeqNo) \n"
                     + " ) "
                    , TABLENAME);

            cm.ExecuteCmd(sql.ToString());

        }


        ///// <summary>
        ///// 清除和创建的表队列
        ///// </summary>
        ///// <param name="cm"></param>
        //[Obsolete("可能不需要")]
        //private void EnsureCreateSequences(ConnectionManager cm)
        //{

        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("DECLARE \n");
        //    sql.Append("    v_count int;\n");
        //    sql.Append("BEGIN \n");
        //    sql.AppendFormat("SELECT  count(*) INTO v_count FROM USER_SEQUENCES WHERE SEQUENCE_NAME = '{0}'; \n", SEQUENCES);
        //    sql.Append("  IF (v_count = 0) THEN \n");
        //    sql.AppendFormat("        EXECUTE IMMEDIATE 'CREATE SEQUENCE  {0} minvalue 1 maxvalue 999999999999999999999999999 start with 1 "
        //                        + " increment by 1 cache 50 order'; \n", SEQUENCES);

        //    sql.Append("    END IF;\n");
        //    sql.Append("END;\n");

        //    cm.ExecuteCmd(sql.ToString());
            

        //}

        /// <summary>
        /// 获取当前库下的所有表名
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableList()
        {

            //var sql = string.Format("SELECT TABLE_NAME AS name FROM USER_TABLES\n"
            //                                    + " WHERE TABLE_NAME != '{0}' "
            //                                    + " ORDER BY TABLE_NAME ", TABLENAME);

            var sql = string.Format("SELECT TABLE_NAME as name FROM INFORMATION_SCHEMA.TABLES\n"
                                               + " WHERE TABLE_SCHEMA = '{0}' and TABLE_NAME != '{1}' "
                                               + " ORDER BY TABLE_NAME ",m_cm.config.DbName, TABLENAME);

            DataTable table = m_cm.GetData(sql );
            return table;
        }

      
        /// <summary>
        /// 创建跟踪表
        /// </summary>
        public void CreateTraceTable()
        {

            EnsureCreateTable(m_cm);
           // EnsureCreateSequences(m_cm);
        }

        #endregion
    }
}

