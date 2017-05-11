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
        private const string SEQUENCES = "DBVSEQUENCES";
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

        private DataTable GetTableColumns(ConnectionManager cm, string tableName)
        {
            string sql = " select t.column_name as name,t.data_type as typeName from \n"
                        + "user_tab_cols t \n"
                        + "where t.table_name = '{0}' and t.data_type not in ('long')\n";

            return cm.GetData(string.Format(sql, tableName.ToUpper()));
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

            string newTableFieldValue = GetTableValue(tableColumns,"NEW");
            string oldTableFieldValue = GetTableValue(tableColumns,"OLD");
            string newPKValue = GetPKValue(cm, tableColumns, tableName,"NEW");
            string oldPKValue = GetPKValue(cm, tableColumns, tableName, "OLD");
            //string newUserFieldName = GetSafeUserFieldName(tableColumns,"NEW");
            //string oldUserFieldName = GetSafeUserFieldName(tableColumns, "OLD");



            sqlBuilder.AppendFormat("CREATE OR REPLACE TRIGGER {1} AFTER INSERT OR DELETE OR UPDATE  on {0} \n", tableName, triggerName);
            sqlBuilder.Append("REFERENCING OLD AS OLD NEW AS NEW \n");
            sqlBuilder.Append("FOR EACH ROW \n");
            sqlBuilder.Append("BEGIN\n"); 
            sqlBuilder.Append("DECLARE status number(4); \n"); 
            sqlBuilder.Append("BEGIN\n");
            sqlBuilder.Append("     IF INSERTING THEN\n");
            sqlBuilder.Append("         status := 1;\n");
            sqlBuilder.Append("     ELSIF UPDATING THEN\n");
            sqlBuilder.Append("         status :=2;\n");
            sqlBuilder.Append("     ELSE\n");
            sqlBuilder.Append("         status :=3;\n");
            sqlBuilder.Append("     END IF;\n");    
            

            //Insert 语句记录
            sqlBuilder.Append("IF (status = 1) THEN\n");
            sqlBuilder.Append("   INSERT INTO dbv_LOGDATA (SEQNO,RecDate,tableName,tabletype,status,PK,data,updateuser)\n");
            sqlBuilder.AppendFormat("   VALUES ({3}.NEXTVAL, SYSDATE,'{2}',0,status,{0},{1},SYS_CONTEXT('USERENV','IP_ADDRESS'));\n"
                                , newPKValue
                                , newTableFieldValue
                                //, newUserFieldName
                                , tableName
                                , SEQUENCES
                                );
            sqlBuilder.Append("END IF;\n");

            //Update 语句记录
            sqlBuilder.Append("IF (status = 2) THEN\n");

            sqlBuilder.Append("   INSERT INTO dbv_LOGDATA (SEQNO,RecDate,tableName,tabletype,status,PK,data,updateuser)\n");
            sqlBuilder.AppendFormat("   VALUES ({3}.NEXTVAL,SYSDATE,'{2}',0,status,{0},{1},SYS_CONTEXT('USERENV','IP_ADDRESS'));\n"
                                , newPKValue
                                , newTableFieldValue
                                //, newUserFieldName
                                , tableName
                                , SEQUENCES
                                );

            sqlBuilder.AppendFormat("   insert into dbv_LOGDATA (SEQNO,RecDate,tableName,tabletype,status,PK,data,updateuser)\n");
            sqlBuilder.AppendFormat("   VALUES ({3}.NEXTVAL,SYSDATE,'{2}',1,status,{0},{1},SYS_CONTEXT('USERENV','IP_ADDRESS'));\n"
                                        , newPKValue
                                        , oldTableFieldValue
                                        //, newUserFieldName
                                        , tableName
                                        , SEQUENCES
                                        );
            sqlBuilder.Append("END IF;\n");


            //Delete 语句记录
            sqlBuilder.Append("IF (status = 3) THEN\n");
            sqlBuilder.AppendFormat("   insert into dbv_LOGDATA (SEQNO,RecDate,tableName,tabletype,status,PK,data,updateuser)\n");
            sqlBuilder.AppendFormat("   VALUES ({3}.NEXTVAL,SYSDATE,'{2}',1,status,{0},{1},SYS_CONTEXT('USERENV','IP_ADDRESS'));\n"
                                    , oldPKValue
                                    , oldTableFieldValue
                                    //, oldUserFieldName
                                    , tableName
                                    , SEQUENCES
                                    );
            sqlBuilder.Append("END IF;\n");


            sqlBuilder.Append(" END;\n");
            sqlBuilder.Append("END;\n");

            cm.ExecuteCmd(sqlBuilder.ToString());


        }

        private static void DeleteTrigger(ConnectionManager cm, string triggerName)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DECLARE \n");
            sql.Append("    v_count int;\n");
            sql.Append("BEGIN \n");
            sql.AppendFormat("SELECT  count(*) INTO v_count FROM USER_TRIGGERS WHERE TRIGGER_NAME = '{0}'; \n", triggerName);
            sql.Append("    IF (v_count >= 1) THEN \n");
            sql.AppendFormat("        EXECUTE IMMEDIATE 'drop trigger {0}';\n", triggerName);
            sql.Append("    END IF; \n");
            sql.Append("END; \n");

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
            string sql = string.Format("SELECT t1.COLUMN_NAME FROM USER_CONS_COLUMNS t1\n"
                            + "  INNER JOIN USER_CONSTRAINTS t2 ON t1.TABLE_NAME = t2.TABLE_NAME and t1.CONSTRAINT_NAME = t2.CONSTRAINT_NAME \n"
                            + "  WHERE t1.TABLE_NAME = '{0}' and t2.CONSTRAINT_TYPE = 'P' \n"
                            + "  ORDER BY t1.POSITION", tableName);

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
                return ret.Substring(2);
            }
            else
            {
                return "''";
            }

        }

        private static void FillFieldValueString(StringBuilder sqlBuilder,DataRow row,string perfix)
        {
            sqlBuilder.AppendFormat("|| NVL('{0}=' || substr(to_char(:{1}.{0}),0,20) || ';','')", row["name"],perfix);
        }

        private string GetTableValue(DataTable table, string perfix)
        {
            StringBuilder sql = new StringBuilder();
            foreach (DataRow row in table.Rows )
            {
                FillFieldValueString(sql, row, perfix);
            }
            string ret = sql.ToString();
            return ret.Substring(2);
        }

        public void RemoveTrigger()
        {
            throw new NotImplementedException();
        }

        public DataTable GetTraceData(string userName, DateTime startTime)
        {
            string ip = GetIP(); 
            //string sql = "SELECT * FROM {0} WHERE (UPDATEUSER = '{1}' OR UPDATEUSER= ' ') AND RECDATE >= TO_DATE('{2}','yyyy/mm/dd hh24:mi:ss') order by SEQNO";
            string sql = "SELECT * FROM {0} WHERE (UPDATEUSER = '{1}' ) AND RECDATE >= TO_DATE('{2}','yyyy/mm/dd hh24:mi:ss') order by SEQNO";

            return m_cm.GetData(string.Format(sql, TABLENAME, ip, startTime.ToString("yyyy/MM/dd HH:mm:ss")));
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
                m_cm.ExecuteCmd("DELETE FROM  " + TABLENAME + " WHERE (UpdateUser = '" + GetIP() + "' )");
            }
            catch { }
        }

        private void EnsureCreateTable(ConnectionManager cm)
        {
            
            StringBuilder sql = new StringBuilder();
            sql.Append("DECLARE \n");
            sql.Append("    v_count int;\n");
            sql.Append("BEGIN \n");
            sql.AppendFormat("SELECT  count(*) INTO v_count FROM USER_TABLES WHERE TABLE_NAME = '{0}'; \n", TABLENAME);
            sql.Append("  IF (v_count >= 1) THEN \n");
            sql.AppendFormat("        EXECUTE IMMEDIATE 'DROP TABLE {0}'; \n", TABLENAME);
            sql.Append("    END IF;\n");
            sql.Append("END;\n");

            cm.ExecuteCmd(sql.ToString());

            sql = new StringBuilder();
            sql.AppendFormat("  CREATE  TABLE {0} ( \n"
                     + " SEQNO  int   NOT NULL\n"
                     + ",TABLETYPE int  \n"
                     + ",RecDate DATE\n"
                     + ",tablename  varchar2(40)  \n"
                     + ",status  int  \n"
                     + ",PK  VARCHAR2(200) \n"
                     + ",Data  VARCHAR2(3000) \n"
                     + ",UpdateUser  VARCHAR2(20) not null \n"
                     + ",CONSTRAINT PK_DGV_LOGDATA PRIMARY KEY  (SeqNo) \n"
                     + " ) "
                    , TABLENAME);

            cm.ExecuteCmd(sql.ToString());

        }


        private void EnsureCreateSequences(ConnectionManager cm)
        {

            StringBuilder sql = new StringBuilder();
            sql.Append("DECLARE \n");
            sql.Append("    v_count int;\n");
            sql.Append("BEGIN \n");
            sql.AppendFormat("SELECT  count(*) INTO v_count FROM USER_SEQUENCES WHERE SEQUENCE_NAME = '{0}'; \n", SEQUENCES);
            sql.Append("  IF (v_count = 0) THEN \n");
            sql.AppendFormat("        EXECUTE IMMEDIATE 'CREATE SEQUENCE  {0} minvalue 1 maxvalue 999999999999999999999999999 start with 1 "
                                + " increment by 1 cache 50 order'; \n", SEQUENCES);

            sql.Append("    END IF;\n");
            sql.Append("END;\n");

            cm.ExecuteCmd(sql.ToString());
            

        }

        public DataTable GetTableList()
        {

            //var sql = string.Format("SELECT TABLE_NAME AS name FROM USER_TABLES\n"
            //                                    + " WHERE TABLE_NAME != '{0}' "
            //                                    + " ORDER BY TABLE_NAME ", TABLENAME);

            var sql = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES\n"
                                               + " WHERE TABLE_SCHEMA = '{0}' and TABLE_NAME != '{0}' "
                                               + " ORDER BY TABLE_NAME ",m_cm.config.dbname, TABLENAME);

            DataTable table = m_cm.GetData(sql );
            return table;
        }

      

        public void CreateTraceTable()
        {

            EnsureCreateTable(m_cm);
            EnsureCreateSequences(m_cm);
        }

        #endregion
    }
}

