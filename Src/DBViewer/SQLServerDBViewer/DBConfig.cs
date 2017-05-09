using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;

namespace DBViewer.Model.SQLServer
{
    class DBConfig:IDBConfig
    {
        private ConnectionConfig m_ctl = null;
        #region IDBConfig 成员

        public Control GetConfigPanel(DBViewerConfig config)
        {
            if (m_ctl == null)
            {
                m_ctl = new ConnectionConfig();
            }
            m_ctl.Config = config;
            return m_ctl;
        }

        public DBViewerConfig GetConfig()
        {
            if (m_ctl != null)
            {
                return m_ctl.Config;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
