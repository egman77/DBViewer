using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBViewer.Model.Core;
using System.Data;

namespace DBViewer.UI
{
    internal class UpdateValue
    {
        public string OldValue = string.Empty;
        public string NewValue = string.Empty;

        private bool m_OldValueSeted = false;

        public bool IsChanged()
        {
            return OldValue != NewValue;
        }

        public UpdateValue SetNewValue(string value)
        {
            NewValue = value;
            return this;
        }

        public UpdateValue SetDeletedValue(string value)
        {
            OldValue = value;
            return this;
        }

        public UpdateValue SetUpdatedValue(string oldValue, string newValue)
        {
            NewValue = newValue;
            if (!m_OldValueSeted)
            {
                OldValue = oldValue;
                m_OldValueSeted = true;
            }
            return this;
        }
    }

    internal class UpdateValueCollection:Dictionary<string,UpdateValue>
    {
        public EnumOperatorType Status;
        public string TableName;
        public string PKValue;

        public UpdateValueCollection(string tableName, string pkValue, EnumOperatorType  status)
        {
            this.TableName = tableName;
            this.PKValue = pkValue;
            this.Status = status;

        }

        public UpdateValueCollection GetChanges()
        {
            UpdateValueCollection c = new UpdateValueCollection(this.TableName,this.PKValue,this.Status);
            c.Status = this.Status;
            foreach (KeyValuePair<string,UpdateValue> keyValue in this)
            {
                if (keyValue.Value.IsChanged())
                {
                    c[keyValue.Key] = keyValue.Value;
                }
            }
            return c;
        }

        public UpdateValueCollection SetUpdateData(DataRow titleRow,DataRow dataRow,int maxCols,bool ignoreOldValue)
        {
            
            for (int i = 0; i < maxCols; i+=2)
            {
                string oldValue = Util.ToString(dataRow["F" + i.ToString()]);
                string newValue = Util.ToString(dataRow["F" + (i + 1).ToString()]);
                string title = Util.ToString(titleRow["F" + i.ToString()]);
                if (title != string.Empty)
                {
                    UpdateValue v;
                    if (this.ContainsKey(title))
                    {
                        v = this[title];
                    }
                    else
                    {
                        v =new UpdateValue();
                        this.Add(title, v);
                    }

                    if (ignoreOldValue)
                    {
                        v.SetNewValue(newValue);
                    }
                    else
                    {
                        v.SetUpdatedValue(oldValue, newValue);
                    }
                }
                else
                {
                    break;
                }
            }
            return this;
        }

        
        public UpdateValueCollection SetInsertedData(DataRow titleRow, DataRow dataRow, int maxCols)
        {
            for (int i = 0; i < maxCols; i ++)
            {

                string value = Util.ToString(dataRow["F" + i.ToString()]);
                string title = Util.ToString(titleRow["F" + i.ToString()]);
                if (title != string.Empty)
                {
                    UpdateValue v;
                    if (this.ContainsKey(title))
                    {
                        v = this[title];
                    }
                    else 
                    {
                        v = new UpdateValue();
                        this.Add(title,v);
                    }
                    v.SetNewValue(value);
                }
                else
                {
                    break;
                }
            }
            return this;
        }

        public static UpdateValueCollection CreateUpdateData(string pkValue,string tableName,DataRow titleRow, DataRow dataRow, int maxCols)
        {
            UpdateValueCollection c = new UpdateValueCollection(tableName,pkValue,EnumOperatorType.Update);
            c.SetUpdateData(titleRow, dataRow, maxCols,false);
            return c;

        }
        public static UpdateValueCollection CreateInsertedData(string pkValue, string tableName, DataRow titleRow, DataRow dataRow, int maxCols)
        {
            UpdateValueCollection c = new UpdateValueCollection(tableName, pkValue, EnumOperatorType.New);
            c.SetInsertedData(titleRow,dataRow,maxCols);
            return c;
        }
        public static UpdateValueCollection CreateDeletedData(string pkValue, string tableName, DataRow titleRow, DataRow dataRow, int maxCols)
        {
            UpdateValueCollection c = new UpdateValueCollection(tableName, pkValue, EnumOperatorType.Delete);
             for (int i = 0; i < maxCols; i ++)
            {
                string value = Util.ToString(dataRow["F" + i.ToString()]);
                string title = Util.ToString(titleRow["F" + i.ToString()]);

                if (title != string.Empty)
                {
                    c.Add(title, new UpdateValue().SetDeletedValue(value));
                }
                else
                {
                    break;
                }
            }
            return c;
        }
    }

    internal class UpdateValueManager: Dictionary<string, UpdateValueCollection>
    {
     

        public UpdateValueManager GetChanges()
        {
            UpdateValueManager newData = new UpdateValueManager();
            foreach (KeyValuePair<string, UpdateValueCollection> keyValue in this)
            {
                UpdateValueCollection c = keyValue.Value.GetChanges();
                if (c.Count > 0)
                {
                    newData.Add(keyValue.Key, c);
                }
            }

            return newData;
        }

        public void FillToDataTable(DataTable table)
        {


            int fixedColumns = table.Columns.Count;

            int i = 0;
            foreach (KeyValuePair<string, UpdateValueCollection> keyValue in this)
            {
                UpdateValueCollection c = keyValue.Value.GetChanges();
                if (c.Count > 0)
                {
                    
                    EnsureTableColumn(table, fixedColumns, c.Status == EnumOperatorType.Update ? c.Count * 2 : c.Count);
                    DataRow titleRow = table.NewRow();
                    titleRow["SeqNo"] = i++;
                    titleRow["TableName"] = c.TableName;
                    titleRow["status"] = c.Status.ToString();
                    titleRow["PKValue"] = c.PKValue;

                    DataRow dataRow = table.NewRow();
                    table.Rows.Add(titleRow);
                    table.Rows.Add(dataRow);
                    

                    int k = 0;
                    foreach(KeyValuePair<string,UpdateValue> v in c)
                    {
                        titleRow["F" + k.ToString()] = v.Key;

                        if (c.Status == EnumOperatorType.Update)
                        {
                            titleRow["F" + (k + 1).ToString()] = v.Key;
                            dataRow["F" + k.ToString()] = v.Value.OldValue;
                            dataRow["F" + (k + 1).ToString()] = v.Value.NewValue;
                            k += 2;
                        }
                        else
                        {
                            if (c.Status == EnumOperatorType.New)
                            {
                                dataRow["F" + k.ToString()] = v.Value.NewValue;

                            }
                            else /*if (c.Status == EnumOperatorType.Delete)*/
                            {
                                dataRow["F" + k.ToString()] = v.Value.OldValue;
                                

                            }
                            k += 1;
                        }
                    }
                }
            }

            

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


        public void AddData(string pkValue,string tableName, EnumOperatorType status, DataRow titleRow,DataRow dataRow,int maxCols)
        {
            string newKey = tableName + "|" + pkValue;

            if (this.ContainsKey(newKey))
            {
                UpdateValueCollection c = this[newKey];

                if (status == EnumOperatorType.Delete)
                {
                    if (c.Status == EnumOperatorType.New)
                    {
                        //新规的数据，删除掉，则不需要计算差分
                        this.Remove(newKey);
                    }
                    else if (c.Status == EnumOperatorType.Delete)
                    {
                        //删除的数据，再次删除，无视
                        //DoNothing
                    }
                    else /*if (c.Status == EnumOperatorType.Update)*/
                    {
                        //修改的数据，删除掉，表示数据删除了
                        c.Status = EnumOperatorType.Delete;
                    }

                }
                else if (status == EnumOperatorType.New)
                {
                    if (c.Status == EnumOperatorType.Delete)
                    {
                        //删除的数据，再次新规，认为是更新
                        c.Status = EnumOperatorType.Update;
                        c.SetInsertedData(titleRow,dataRow,maxCols);
                    }
                    else 
                    {
                        //异常数据
                        throw new Exception("新规数据已经存在.");
                    }
                }
                else /*if (status == EnumOperatorType.Update)*/
                {
                    
                    if (c.Status == EnumOperatorType.New)
                    {
                        //新规的数据，再次修改,仍然认为是新规
                        //只保存新的数据
                        c.SetUpdateData(titleRow, dataRow, maxCols,true);
                    }
                    else if(c.Status == EnumOperatorType.Update)
                    {
                        //修正的数据，再次修改,仍然是修改
                        //只保存新的数据
                        c.SetUpdateData(titleRow, dataRow, maxCols,false);
                    }
                    else 
                    {
                        //删除的数据，再次修改,异常数据
                         throw new Exception("数据已经被删除，不能修改.");
                    }

                }


            }
            else
            {
                UpdateValueCollection c;
                switch (status)
                {
                    case EnumOperatorType.New:
                        c = UpdateValueCollection.CreateInsertedData(pkValue,tableName,titleRow, dataRow, maxCols);
                        break;
                    case EnumOperatorType.Delete:
                        c = UpdateValueCollection.CreateDeletedData(pkValue, tableName, titleRow, dataRow, maxCols);
                        break;
                    case EnumOperatorType.Update:
                        c = UpdateValueCollection.CreateUpdateData(pkValue, tableName, titleRow, dataRow, maxCols);
                        break;
                    default:
                        throw new Exception("错误的操作状态." + status.ToString());
                }

                this.Add(newKey, c);
                
            }
        }

    }
  
}
