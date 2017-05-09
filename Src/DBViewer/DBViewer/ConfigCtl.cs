using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;
using System.Xml;
using System.IO;

namespace DBViewer.UI
{
    public partial class ConfigCtl : UserControl
    {
        private IDBConfig m_currentConfig;
        private DBViewerConfig  m_currentDBConfig;
        private XmlDocument m_doc = new XmlDocument();
        public ConfigCtl()
        {
            InitializeComponent();

        }

        private void cmbDBType_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelConfig.Controls.Clear();
            Control ctl = null;
            switch (cmbDBType.SelectedIndex)
            {
                case 0://SQL Server
                    ctl = GetPanelControl(EnumDBType.SQLServer);
                    break;
                case 1:
                    break;
            }

            if (ctl != null)
            {
                chkDisplayNull.Checked = m_currentDBConfig.DisplayNull;
                txtUserFieldName.Text = m_currentDBConfig.UserFieldName;
                txtUser.Text = m_currentDBConfig.TraceUser;

                panelConfig.Controls.Add(ctl);
            }
        }

        private Control GetPanelControl(EnumDBType enuDBType)
        {
            m_currentConfig = ConfigService.CreateConfigModel(enuDBType);
            m_currentDBConfig = DBViewerConfig.Create(GetDBTypeNode(enuDBType));
            Control ctl = m_currentConfig.GetConfigPanel(m_currentDBConfig);
            return ctl;
        }

        private XmlNode GetDBTypeNode(EnumDBType enuDbType)
        {
            string dbtype = enuDbType.ToString();
            XmlNode node = m_doc.SelectSingleNode(string.Format("dbtype[@type='{0}']", dbtype));
            if (node == null)
            {
                m_doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><dbtype></dbtype>");
                node = m_doc.SelectSingleNode("dbtype");
                XmlAttribute attr = m_doc.CreateNode(XmlNodeType.Attribute,"type",string.Empty) as XmlAttribute;
                attr.Value = dbtype;

                node.Attributes.Append(attr);
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "displaynull",string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "userfieldname",string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "user", string.Empty)); 
                
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "server", string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "user",string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "password",string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "dbname", string.Empty));
                

               
                node = m_doc.SelectSingleNode(string.Format("dbtype[@type='{0}']", dbtype));
            }

            return node;
        }



        public bool SaveConfig()
        {
            if (m_currentConfig != null)
            {
                m_currentDBConfig = m_currentConfig.GetConfig();
                m_currentDBConfig.DisplayNull = chkDisplayNull.Checked;
                m_currentDBConfig.UserFieldName = txtUserFieldName.Text;
                m_currentDBConfig.TraceUser = txtUser.Text;

                XmlNode node = GetDBTypeNode(m_currentDBConfig.DbType);
                m_currentDBConfig.SaveToNode(node);

                m_doc.Save(Path.Combine(Application.StartupPath, "dbviewerconfig.xml"));
                Util.ShowMessage("保存成功.");
                return true;

            }
            else
            {
                Util.ShowMessage("请选择数据库类型.");
                return false;
            }
        }


        private void ConfigCtl_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                m_doc.Load(Path.Combine(Application.StartupPath, "dbviewerconfig.xml"));
            }
        }
    }
}
