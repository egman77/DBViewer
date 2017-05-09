using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;

namespace DBViewer.UI
{
    public delegate void DoActionDelegate(DataRowView row);
    public delegate void DoStartActionDelegate();

    public partial class TableListForm : Form
    {
        public IDBViewerModel CurrentModel;

        public DoActionDelegate Action;
        public DoStartActionDelegate StartAction;
        public TableListForm()
        {
            InitializeComponent();
            
        }

        private void TableListForm_Load(object sender, EventArgs e)
        {
            DataTable table = CurrentModel.GetTableList();
            table.Columns.Add("selAll", typeof(bool));

            DataView view = new DataView(table);
            this.dataGridView1.DataSource = view;
            
            
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            DataView view = this.dataGridView1.DataSource as DataView;
            foreach (DataRowView row in view)
            {
                row["selAll"] = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DataView view = this.dataGridView1.DataSource as DataView;
            foreach (DataRowView row in view)
            {
                row["selAll"] = false;
            }
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataView view = this.dataGridView1.DataSource as DataView;
                view.RowFilter = "name like '" + txtFilter.Text + "%'";
 
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            DataView view = this.dataGridView1.DataSource as DataView;
            this.dataGridView1.EndEdit();
            this.dataGridView1.CurrentCell = null;
            int count = 0;
            int failedCount = 0;

            OnStartAction();

            foreach (DataRowView row in view)
            {
                if (Util.ToBool(row["selAll"]))
                {
                    try
                    {
                        OnAction(row);
                        row["selAll"] = false;
                        count++;
                    }
                    catch {
                        failedCount++; 
                    }
                }
            }

            statusLabel.Text = string.Format("成功创建{0}个触发器.创建失败{1}个触发器.", count, failedCount);
            this.Cursor = Cursors.Default;
            btnRun.Enabled = true;
            
            Util.ShowMessage(statusLabel.Text);
        }

        private void OnStartAction()
        {
            if (StartAction != null)
            {
                StartAction();
            }
        }

        private void OnAction(DataRowView row)
        {
            if (Action != null)
            {
                Action(row);
            }
        }

      
    }
}
