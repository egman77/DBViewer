using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using SpanedDataGridView;
using System.Drawing;
using DBViewer.Model.Core;

namespace DBViewer.UI
{
    class DiffViewerGrid:DataGridView
    {
        public DiffViewerGrid()
        {
            this.AutoGenerateColumns = false;
        }

        private int m_FixColumnCount = -1;

        public void SetData(int fixColumn, DataTable table)
        {
            m_FixColumnCount = fixColumn;
            int maxColumns = this.Columns.Count;

            for (int i = m_FixColumnCount; i < maxColumns; i++)
            {
                string columnName = "F" + (i - m_FixColumnCount).ToString();
                this.Columns.Remove(columnName);
            }

            for (int i = m_FixColumnCount; i < table.Columns.Count; i++)
            {
                string columnName = "F" + (i - m_FixColumnCount).ToString();

                DataGridViewTextBoxColumnEx ex = new DataGridViewTextBoxColumnEx();
                this.Columns.Add(ex);
                ex.Name = columnName;
                ex.DataPropertyName = columnName;
                ex.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               
            }

          

            this.DataSource = table; 
        }
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            base.OnCellFormatting(e);

            if (m_FixColumnCount == -1)
            {
                return;
            }

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == this.Columns["colStatus"].Index)
                {
                    switch((EnumOperatorType)Enum.Parse(typeof(EnumOperatorType), e.Value.ToString()))  
                    {
                        case EnumOperatorType.Update:
                            e.Value = "更新";
                            break;
                        case EnumOperatorType.New:
                            e.Value = "新增";
                            break;
                        case EnumOperatorType.Delete:
                            e.Value = "删除";
                            break;
                    }
                    e.CellStyle.Font = new Font(e.CellStyle.Font,FontStyle.Bold);
                    e.CellStyle.ForeColor = Color.DarkGreen;
                }

                if (e.ColumnIndex >= m_FixColumnCount)
                {
                    if (Util.ToString(e.Value) != string.Empty)
                    {
                        if (e.RowIndex % 2 == 0)
                        {
                            //标题行
                            e.CellStyle.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (Util.ToString(this["colStatus", e.RowIndex].Value) == EnumOperatorType.Update.ToString())
                            {
                                //数据行
                                if ((e.ColumnIndex - m_FixColumnCount) % 2 == 1)
                                {
                                    e.CellStyle.ForeColor = Color.DarkRed;
                                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                                }
                            }

                        }
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.DarkGray;
                    }

                }

            }
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            for (int i = 0; i < this.Rows.Count; i += 2)
            {
                for (int j = 0; j < m_FixColumnCount; j++)
                {
                    DataGridViewTextBoxCellEx cell = this[j, i] as DataGridViewTextBoxCellEx;
                    cell.RowSpan = 2;
                }

                if (Util.ToString(this["colStatus", i].Value) == EnumOperatorType.Update.ToString())
                {
                    //更新行
                    for (int j = m_FixColumnCount; j < this.Columns.Count; j += 2)
                    {
                        if (Util.ToString(this[j, i].Value) != string.Empty)
                        {
                            DataGridViewTextBoxCellEx cell = this[j, i] as DataGridViewTextBoxCellEx;
                            cell.ColumnSpan = 2;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
