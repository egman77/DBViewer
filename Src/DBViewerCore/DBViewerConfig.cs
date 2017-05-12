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
        /// <summary>
        /// 默认值,相当于Inserted
        /// </summary>
        None=0,
        /// <summary>
        /// 新增
        /// </summary>
        Inserted = 0 ,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 1,
    }

    /// <summary>
    /// SQL操作类型
    /// </summary>
    public enum EnumOperatorType
    {
        /// <summary>
        /// 插入操作
        /// </summary>
        New = 1,
        /// <summary>
        /// 更新操作
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除操作
        /// </summary>
        Delete = 3,

    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum EnumDBType
    {
        /// <summary>
        /// SQLServer数据库
        /// </summary>
        SQLServer,
        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle,
        /// <summary>
        /// MySql数据库
        /// </summary>
        MySql
    }

    /// <summary>
    /// 参数配置
    /// </summary>
    public class DBViewerConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public EnumDBType DbType { get; set; }
        public bool DisplayNull { get; set; } = false;
        public string UserFieldName { get; set; }
        /// <summary>
        /// 跟踪用户名
        /// </summary>
        public string TraceUser { get; set; }

        /// <summary>
        /// 数据库服务器名
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// 数据库服务器登录帐号
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 数据库服务器登录密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DbName { get; set; }

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
                config.Server = node.SelectSingleNode("server").InnerXml;
                config.User = node.SelectSingleNode("user").InnerXml;
                config.Password = node.SelectSingleNode("password").InnerXml;
                config.DbName = node.SelectSingleNode("dbname").InnerXml;
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

            node.SelectSingleNode("server").InnerXml = Server;
            node.SelectSingleNode("user").InnerXml = User;
            node.SelectSingleNode("password").InnerXml = Password;
            node.SelectSingleNode("dbname").InnerXml = DbName;
            
           
        }
    }
}
