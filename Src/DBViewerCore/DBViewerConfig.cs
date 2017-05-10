using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DBViewer.Model.Core
{
    /// <summary>
    /// 临时表类型
    /// </summary>
    public enum EnumTableType
    {
        inserted = 0 ,
        deleted = 1,
    }

    /// <summary>
    /// SQL操作类型
    /// </summary>
    public enum EnumOperatorType
    {
        New = 1,
        Update = 2,
        Delete = 3,

    }

    public enum EnumDBType
    {
        SQLServer,
        Oracle,
        MySql
    }

    /// <summary>
    /// 参数配置
    /// </summary>
    public class DBViewerConfig
    {
        /// <summary>
        /// 参数配置类型
        /// </summary>
        public EnumDBType DbType;
        public bool DisplayNull = false;
        public string UserFieldName;
        public string TraceUser;

        public string server;
        public string user;
        public string password;
        public string dbname;

        public static DBViewerConfig Create(XmlNode node)
        {
            DBViewerConfig config = new DBViewerConfig();
            XmlAttribute attr = node.Attributes["type"];
            if (attr != null)
            {
                config.DbType = (EnumDBType)Enum.Parse(typeof(EnumDBType), node.Attributes["type"].Value);
                bool result;
                bool.TryParse(node.SelectSingleNode("displaynull").InnerXml, out result);
                config.DisplayNull = result;
                config.UserFieldName = node.SelectSingleNode("userfieldname").InnerXml;
                config.server = node.SelectSingleNode("server").InnerXml;
                config.user = node.SelectSingleNode("user").InnerXml;
                config.password = node.SelectSingleNode("password").InnerXml;
                config.dbname = node.SelectSingleNode("dbname").InnerXml;
                config.TraceUser = node.SelectSingleNode("traceuser").InnerXml;
                return config;
            }
            return null;
        }

        public void SaveToNode(XmlNode node)
        {
            node.SelectSingleNode("displaynull").InnerXml = DisplayNull ? bool.TrueString : bool.FalseString;
            node.SelectSingleNode("userfieldname").InnerXml = UserFieldName;
            node.SelectSingleNode("traceuser").InnerXml = TraceUser;

            node.SelectSingleNode("server").InnerXml = server;
            node.SelectSingleNode("user").InnerXml = user;
            node.SelectSingleNode("password").InnerXml = password;
            node.SelectSingleNode("dbname").InnerXml = dbname;
            

        }
    }
}
