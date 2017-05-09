using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DBViewer.Model.Core;
using SpanedDataGridView;

namespace DBViewer.UI
{
    public partial class ChangeListViewer : UserControl
    {
        private DataTable m_table;
        private int m_FixColumnCount;

        public DataTable Table
        {
            get { return m_table; }
            set
            {
                m_table = value;
                if (value != null)
                {
                    DataTable table = CalcData(m_table);
                    this.dataGridView1.SetData(m_FixColumnCount, table);
                }
            }
        }
        public ChangeListViewer()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }


        private DataTable CalcData(DataTable orginalData)
        {

            DataTable table = new DataTable();
            table.Columns.Add("SeqNo", typeof(string));

            table.Columns.Add("RecDate", typeof(string));
            table.Columns.Add("TableName", typeof(string));
            table.Columns.Add("status", typeof(string));
            table.Columns.Add("PKValue", typeof(string));

            m_FixColumnCount = table.Columns.Count;


            int updateRowCount = 0;

            for(int i = 0; i < orginalData.Rows.Count; i++)
            {
                DataRow row = orginalData.Rows[i];

         
                EnumOperatorType status =  (EnumOperatorType)Util.ToInt(row["status"]);
                EnumTableType tableType = (EnumTableType)Util.ToInt(row["tabletype"]);

                if (status == EnumOperatorType.Update && tableType == EnumTableType.deleted)
                {
                     updateRowCount = 0;
                    //更新的数据，delete已经处理过了，继续循环
                    continue;
                }

                if (status == EnumOperatorType.New || status == EnumOperatorType.Delete)
                {
                    CreateNewOrDeleteRowData(table, m_FixColumnCount, row, status);
                }
                else
                {
                    if (updateRowCount == 0)
                    {
                        updateRowCount = GetUpdateRowCount(orginalData, updateRowCount, i);
                    }
                    DataRow oldRow = orginalData.Rows[i + updateRowCount];

                    CreateUpdateRowData(table, m_FixColumnCount, row, oldRow);

                }


            }

            return table;
        }

        private void CreateUpdateRowData(DataTable table, int fixColumnsCount, DataRow newRow,DataRow oldRow)
        {
            string data = Util.ToString(newRow["Data"]);
            Dictionary<string, string> dicData = GetData(data);

            
            Dictionary<string, string> dicOldData = GetData(Util.ToString(oldRow["data"]));

            Dictionary<string, UpdateValue> dicUpdateData = new Dictionary<string, UpdateValue>();

            foreach (KeyValuePair<string, string> keyValue in dicData)
            {
                string oldValue = dicOldData[keyValue.Key];
                if (oldValue != keyValue.Value)
                {
                    UpdateValue updateValue = new UpdateValue();
                    dicUpdateData[keyValue.Key] = updateValue;
                    updateValue.OldValue = oldValue;
                    updateValue.NewValue = keyValue.Value;

                }
            }

            if (dicUpdateData.Count > 0)
            {
                EnsureTableColumn(table, fixColumnsCount, dicUpdateData.Keys.Count * 2);

                DataRow titleRow = table.NewRow();
                DataRow valueRow = table.NewRow();

                int k = 0;
                foreach (KeyValuePair<string, UpdateValue> keyValue in dicUpdateData)
                {
                    titleRow["F" + (k * 2).ToString()] = keyValue.Key;
                    titleRow["F" + (k * 2 + 1).ToString()] = keyValue.Key;

                    valueRow["F" + (k * 2).ToString()] = keyValue.Value.OldValue;
                    valueRow["F" + (k * 2 + 1).ToString()] = keyValue.Value.NewValue;
                    k++;
                }

                titleRow["RecDate"] = GetDisplayDate(Util.ToDateTime(newRow["RecDate"]));
                titleRow["status"] = EnumOperatorType.Update.ToString();
                titleRow["TableName"] = newRow["TableName"];
                titleRow["PKValue"] = newRow["PK"];
                titleRow["SeqNo"] = table.Rows.Count / 2 + 1;

                table.Rows.Add(titleRow);
                table.Rows.Add(valueRow);
            }
        }

        private void CreateNewOrDeleteRowData(DataTable table, int fixColumnsCount, DataRow row, EnumOperatorType status)
        {

            string data = Util.ToString(row["Data"]);
            Dictionary<string, string> dicData = GetData(data);

            EnsureTableColumn(table, fixColumnsCount, dicData.Keys.Count);

            DataRow titleRow = table.NewRow();
            DataRow valueRow = table.NewRow();

            titleRow["RecDate"] = GetDisplayDate(Util.ToDateTime(row["RecDate"]));
            titleRow["status"] = status.ToString();
            titleRow["TableName"] = row["TableName"];
            titleRow["PKValue"] = row["PK"];
            titleRow["SeqNo"] = table.Rows.Count / 2 + 1;
            int k = 0;
            foreach (KeyValuePair<string, string> keyValue in dicData)
            {
                titleRow["F" + k.ToString()] = keyValue.Key;
                valueRow["F" + k.ToString()] = keyValue.Value;
                k++;
            }

            table.Rows.Add(titleRow);
            table.Rows.Add(valueRow);
        }

        /// <summary>
        /// 获取更新行数
        /// </summary>
        /// <param name="orginalData"></param>
        /// <param name="updateRowCount"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private static int GetUpdateRowCount(DataTable orginalData, int updateRowCount, int rowIndex)
        {
            for (int i = rowIndex; i < orginalData.Rows.Count; i++)
            {
                DataRow nextRow = orginalData.Rows[i];
                EnumTableType tableType = (EnumTableType)Util.ToInt(nextRow["tabletype"]);

                if (tableType == EnumTableType.deleted)
                {
                    //删除行，Update行已经结束
                    break;
                }

                updateRowCount++;
            }
            return updateRowCount;
        }

        private void EnsureTableColumn(DataTable table, int fixColumnsCount, int freeColumnCount)
        {
            if (table.Columns.Count < (fixColumnsCount + freeColumnCount))
            {
                for (int i = table.Columns.Count - fixColumnsCount; i < freeColumnCount; i++)
                {
                    table.Columns.Add("F" + i.ToString(), typeof(string));
                }
            }

        }

        private Dictionary<string, string> GetData(string data)
        {
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(data))
            {
                return dicData;
            }

            string[] dataArrays = data.Split(';');
            foreach (string s in dataArrays)
            {
                if (s != string.Empty)
                {
                    string[] values = s.Split('=');
                    dicData[values[0]] = values[1];
                }
            }

            return dicData;
        }

        private string GetDisplayDate(DateTime dateValue)
        {
            if (dateValue.Date == DateTime.Today)
            {
                return dateValue.ToString("HH:mm:ss");
            }
            else if (((TimeSpan)(dateValue.Date - DateTime.Today)).Days == -1)
            {
                return "昨日" + dateValue.ToString("HH:mm:ss");
            }
            else
            {
                return dateValue.ToString("yy/MM/dd HH:mm:ss");
            }

        }

      
        public DataRow[] GetSelectData()
        {
            if (this.dataGridView1.SelectedRows != null)
            {
                List<DataRow> rowList = new List<DataRow>();

                for(int i = 0; i < this.dataGridView1.Rows.Count;)
                {
                    if (this.dataGridView1.Rows[i].Selected)
                    {
                        if ((i % 2) == 0)
                        {
                            rowList.Add((this.dataGridView1.Rows[i].DataBoundItem as DataRowView).Row);
                            rowList.Add((this.dataGridView1.Rows[i + 1].DataBoundItem as DataRowView).Row);
                            i += 2;
                        }
                        else
                        {
                            rowList.Add((this.dataGridView1.Rows[i - 1].DataBoundItem as DataRowView).Row);
                            rowList.Add((this.dataGridView1.Rows[i].DataBoundItem as DataRowView).Row);
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                    }
                }
                return rowList.ToArray();
            }
            else
            {
                return null;
            }
        }

        public int MaxDataCols
        {
            get { return this.dataGridView1.Columns.Count - m_FixColumnCount; }
        }
    }


}
