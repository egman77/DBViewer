using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;
using System.IO;

namespace DBViewer.UI
{
    public partial class MainForm : Form
    {
        private IDBViewerModel m_currentModel;
        
        public MainForm()
        {
            InitializeComponent();
            m_currentModel = ConfigService.CreateCurrentModel(GetConfigFileName());
            if (m_currentModel == null)
            {
                tabMain.TabPages.Remove(tabViewer);
                tabMain.TabPages.Remove(tabTools);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (configCtl1.SaveConfig())
            {
                m_currentModel = ConfigService.CreateCurrentModel(GetConfigFileName());
                if (m_currentModel != null)
                {
                    if (!tabMain.TabPages.Contains(tabViewer))
                    {
                        tabMain.TabPages.Insert(0, tabViewer);
                    }

                    if (!tabMain.TabPages.Contains(tabTools))
                    {
                        tabMain.TabPages.Insert(1, tabTools);
                    }
                }
            }
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {

            TableListForm form = new TableListForm();
            //重构触发器
            form.Action =new DoActionDelegate(DoRebuilTrigger);
            //创建表
            form.StartAction = new DoStartActionDelegate(DoCreateTable);
            form.CurrentModel = m_currentModel;
            form.Show(this);

        }

        private void DoCreateTable()
        {
            m_currentModel.CreateTraceTable();
        }
        private void DoRebuilTrigger(DataRowView row)
        {
            m_currentModel.ReBuildTrigger(Util.ToString(row["name"]));
        }

        private void DoDeleteTrigger(DataRowView row)
        {
            m_currentModel.DeleteTrigger(Util.ToString(row["name"]));
        }

        private void btnView_Click(object sender, EventArgs e)
        {

            RefreshData();
        }

        private void RefreshData()
        {
            string fileName = GetConfigFileName();
            DBViewerConfig config = ConfigService.GetConfig(fileName);
            DataTable table = m_currentModel.GetTraceData(config.TraceUser, DateTime.Today.AddDays(-2));
            this.changeListViewer1.Table = table;
        }

        private static string GetConfigFileName()
        {
            return Path.Combine(Application.StartupPath, "dbviewerconfig.xml");
        }

        private void viewDiff_Click(object sender, EventArgs e)
        {
            DataRow[] rows = this.changeListViewer1.GetSelectData();
            this.diffListViewer1.SetData(rows, this.changeListViewer1.MaxDataCols);
            tabReport.SelectedTab = pageDiff;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            string fileName = GetConfigFileName();
            DBViewerConfig config = ConfigService.GetConfig(fileName);
            m_currentModel.ClearTraceData(config.TraceUser);

            RefreshData();
            
        }

        private void btnDeleteTrigger_Click(object sender, EventArgs e)
        {
            TableListForm form = new TableListForm();
            form.Action = new DoActionDelegate(DoDeleteTrigger);
            form.CurrentModel = m_currentModel;
            form.Show(this);
        }

 

       
 
    }
}
