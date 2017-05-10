namespace DBViewer.UI
{
    partial class ChangeListViewer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new DBViewer.UI.DiffViewerGrid();
            this.colOrder = new SpanedDataGridView.DataGridViewTextBoxColumnEx();
            this.colRecDate = new SpanedDataGridView.DataGridViewTextBoxColumnEx();
            this.colTableName = new SpanedDataGridView.DataGridViewTextBoxColumnEx();
            this.colStatus = new SpanedDataGridView.DataGridViewTextBoxColumnEx();
            this.colPKValue = new SpanedDataGridView.DataGridViewTextBoxColumnEx();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("SimSun", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(674, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "变更记录";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrder,
            this.colRecDate,
            this.colTableName,
            this.colStatus,
            this.colPKValue});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 39);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(674, 346);
            this.dataGridView1.TabIndex = 1;
            // 
            // colOrder
            // 
            this.colOrder.DataPropertyName = "SeqNo";
            this.colOrder.HeaderText = "序号";
            this.colOrder.Name = "colOrder";
            this.colOrder.ReadOnly = true;
            this.colOrder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colRecDate
            // 
            this.colRecDate.DataPropertyName = "recdate";
            this.colRecDate.HeaderText = "时间";
            this.colRecDate.Name = "colRecDate";
            this.colRecDate.ReadOnly = true;
            this.colRecDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRecDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colTableName
            // 
            this.colTableName.DataPropertyName = "tablename";
            this.colTableName.HeaderText = "表名";
            this.colTableName.Name = "colTableName";
            this.colTableName.ReadOnly = true;
            this.colTableName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "status";
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colPKValue
            // 
            this.colPKValue.DataPropertyName = "pkValue";
            this.colPKValue.HeaderText = "主键值";
            this.colPKValue.Name = "colPKValue";
            this.colPKValue.ReadOnly = true;
            this.colPKValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colPKValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ChangeListViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "ChangeListViewer";
            this.Size = new System.Drawing.Size(674, 385);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private DiffViewerGrid dataGridView1;
        private SpanedDataGridView.DataGridViewTextBoxColumnEx colOrder;
        private SpanedDataGridView.DataGridViewTextBoxColumnEx colRecDate;
        private SpanedDataGridView.DataGridViewTextBoxColumnEx colTableName;
        private SpanedDataGridView.DataGridViewTextBoxColumnEx colStatus;
        private SpanedDataGridView.DataGridViewTextBoxColumnEx colPKValue;
    }
}
