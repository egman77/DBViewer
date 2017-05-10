using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DBViewer.Model.Core
{
    /// <summary>
    /// 配置管理类
    /// </summary>
    public class ConfigService
    {
        private static Dictionary<EnumDBType, ModelInfo> dicConfig = new Dictionary<EnumDBType, ModelInfo>();
        private static Dictionary<EnumDBType, ModelInfo> dicViewerModel = new Dictionary<EnumDBType, ModelInfo>();

        static ConfigService()
        {
            if (dicConfig.Count == 0)
            {
                dicConfig[EnumDBType.SQLServer] = new ModelInfo("DBViewer.Model.SQLServer.dll", "DBViewer.Model.SQLServer.DBConfig");
                dicConfig[EnumDBType.Oracle] = new ModelInfo("DBViewer.Model.Oracle.dll", "DBViewer.Model.Oracle.DBConfig");

                dicViewerModel[EnumDBType.SQLServer] = new ModelInfo("DBViewer.Model.SQLServer.dll", "DBViewer.Model.SQLServer.DBViewerModel");
                dicViewerModel[EnumDBType.Oracle] = new ModelInfo("DBViewer.Model.Oracle.dll", "DBViewer.Model.Oracle.DBViewerModel");

            }
        }

        private ConfigService()
        {
          
        }
        /// <summary>
        /// 配置文件名
        /// </summary>
        public string ConfigFileName;

        /// <summary>
        /// 创建Model
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IDBViewerModel CreateModel(DBViewerConfig config)
        {
            
            IDBViewerModel dbViewerModel = dicViewerModel[config.DbType].CreateModel() as IDBViewerModel;
            if (dbViewerModel != null)
            {
                dbViewerModel.Coinfig = config;
            }
            return dbViewerModel;
        }

        public static IDBViewerModel CreateCurrentModel(string fileName)
        {
            DBViewerConfig config = GetConfig(fileName);
            if (config != null)
            {
                return CreateModel(config);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="config"></param>
        public static DBViewerConfig GetConfig(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode node = doc.SelectSingleNode("dbtype");
            DBViewerConfig config = DBViewerConfig.Create(node);
            return config;
        }

        public static IDBConfig CreateConfigModel(EnumDBType dbType)
        {
            ModelInfo info = dicConfig[dbType];
            if (info != null)
            {
                return info.CreateModel() as IDBConfig;
            }
            return null;
        }


    }
    
}
