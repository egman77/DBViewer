using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;

namespace DBViewer.UI
{
    public partial class DiffListViewer : UserControl
    {
        private DataTable m_table;
        private int m_FixColumnCount;
        public DataTable Table
        {
            get { return m_table; }
            set { m_table = value; }
        }

        public DiffListViewer()
        {
            InitializeComponent();
        }

        public void SetData(DataRow[] rows, int maxCols)
        {
            UpdateValueManager data = new UpdateValueManager();

            for (int i = 0; i < rows.Length; i += 2)
            {
                DataRow titleRow = rows[i];
                DataRow dataRow = rows[i + 1];

                EnumOperatorType status = (EnumOperatorType) Enum.Parse(typeof(EnumOperatorType),Util.ToString(titleRow["status"]));
                string tableName = Util.ToString(titleRow["tablename"]);
                string key = Util.ToString(titleRow["PKValue"]);
                data.AddData(key,tableName, status, titleRow, dataRow, maxCols);

            }

            DataTable table = new DataTable();
            table.Columns.Add("SeqNo", typeof(string));
            table.Columns.Add("TableName", typeof(string));
            table.Columns.Add("status", typeof(string));
            table.Columns.Add("PKValue", typeof(string));
            
            m_FixColumnCount = table.Columns.Count;

            data.FillToDataTable(table);

            this.dataGridView1.SetData(m_FixColumnCount, table);

        }

      
    }

        
}

