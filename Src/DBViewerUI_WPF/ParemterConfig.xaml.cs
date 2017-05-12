using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DBViewer.Model.Core;
using System.Xml;

namespace DBViewer.UI.WPF
{
    /// <summary>
    /// ParemterConfig.xaml 的交互逻辑
    /// </summary>
    public partial class ParemterConfig : UserControl
    {
        private DBViewerConfig m_currentDBConfig;
        private XmlDocument m_doc;

        public ParemterConfig()
        {
            InitializeComponent();
            
           
        }


        private void cmbDBType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            m_doc = new XmlDocument();
            m_doc.Load("dbviewerconfig.xml");
            EnumDBType enuDBType;

            switch (cmbDBType.SelectedIndex)
            {
                case 0://SQL Server
                    enuDBType = EnumDBType.SQLServer;
                    txtServer.Visibility = Visibility.Visible;
                    lblServer.Visibility = Visibility.Visible;
                    break;
                case 1:
                    enuDBType = EnumDBType.Oracle;
                    txtServer.Visibility = Visibility.Hidden;
                    lblServer.Visibility = Visibility.Hidden;
                    break;
                default:
                    return;
            }

            m_currentDBConfig = DBViewerConfig.Create(GetDBTypeNode(enuDBType));
            txtServer.Text = m_currentDBConfig.Server;
            txtDBName.Text = m_currentDBConfig.DbName;
            txtDBUser.Text = m_currentDBConfig.User;
            txtPassword.Text = m_currentDBConfig.Password;
            chkDisplayNull.IsChecked = m_currentDBConfig.DisplayNull;
            txtUserFieldName.Text = m_currentDBConfig.UserFieldName;
            txtUser.Text = m_currentDBConfig.TraceUser;

        }


        private XmlNode GetDBTypeNode(EnumDBType enuDbType)
        {
            string dbtype = enuDbType.ToString();
            XmlNode node = m_doc.SelectSingleNode(string.Format("dbtype[@type='{0}']", dbtype));
            if (node == null)
            {
                m_doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><dbtype></dbtype>");
                node = m_doc.SelectSingleNode("dbtype");
                XmlAttribute attr = m_doc.CreateNode(XmlNodeType.Attribute, "type", string.Empty) as XmlAttribute;
                attr.Value = dbtype;

                node.Attributes.Append(attr);
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "displaynull", string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "userfieldname", string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "traceuser", string.Empty));

                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "server", string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "user", string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "password", string.Empty));
                node.AppendChild(m_doc.CreateNode(XmlNodeType.Element, "dbname", string.Empty));



                node = m_doc.SelectSingleNode(string.Format("dbtype[@type='{0}']", dbtype));
            }

            return node;
        }

        public bool SaveConfig()
        {
            if (cmbDBType.SelectedIndex != -1)
            {
                m_currentDBConfig.DbType = cmbDBType.SelectedIndex == 0 ? EnumDBType.SQLServer : EnumDBType.Oracle;
                m_currentDBConfig.Server = txtServer.Text;
                m_currentDBConfig.DbName = txtDBName.Text;
                m_currentDBConfig.User = txtDBUser.Text;
                m_currentDBConfig.Password = txtPassword.Text;

                m_currentDBConfig.DisplayNull = chkDisplayNull.IsChecked.Value;
                m_currentDBConfig.UserFieldName = txtUserFieldName.Text;
                m_currentDBConfig.TraceUser = txtUser.Text;

                XmlNode node = GetDBTypeNode(m_currentDBConfig.DbType);
                m_currentDBConfig.SaveToNode(node);

                m_doc.Save("dbviewerconfig.xml");

               
                Util.ShowMessage("保存成功.");
                return true;

            }
            else
            {
                Util.ShowMessage("请选择数据库类型.");
                return false;
            }
        }

    }
}
