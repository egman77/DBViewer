namespace DBViewer.UI
{
    partial class ConfigCtl
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
            this.cmbDBType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelConfig = new System.Windows.Forms.Panel();
            this.chkDisplayNull = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserFieldName = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbDBType
            // 
            this.cmbDBType.FormattingEnabled = true;
            this.cmbDBType.Items.AddRange(new object[] {
            "SQL Server",
            "Oracle",
            "MySql"});
            this.cmbDBType.Location = new System.Drawing.Point(194, 18);
            this.cmbDBType.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cmbDBType.Name = "cmbDBType";
            this.cmbDBType.Size = new System.Drawing.Size(219, 29);
            this.cmbDBType.TabIndex = 0;
            this.cmbDBType.SelectedIndexChanged += new System.EventHandler(this.cmbDBType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "数据库类型：";
            // 
            // panelConfig
            // 
            this.panelConfig.Location = new System.Drawing.Point(40, 63);
            this.panelConfig.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panelConfig.Name = "panelConfig";
            this.panelConfig.Size = new System.Drawing.Size(803, 359);
            this.panelConfig.TabIndex = 2;
            // 
            // chkDisplayNull
            // 
            this.chkDisplayNull.AutoSize = true;
            this.chkDisplayNull.Location = new System.Drawing.Point(40, 432);
            this.chkDisplayNull.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkDisplayNull.Name = "chkDisplayNull";
            this.chkDisplayNull.Size = new System.Drawing.Size(120, 25);
            this.chkDisplayNull.TabIndex = 3;
            this.chkDisplayNull.Text = "显示空值";
            this.chkDisplayNull.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 486);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "用户字段名:";
            // 
            // txtUserFieldName
            // 
            this.txtUserFieldName.Location = new System.Drawing.Point(178, 481);
            this.txtUserFieldName.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtUserFieldName.Name = "txtUserFieldName";
            this.txtUserFieldName.Size = new System.Drawing.Size(343, 31);
            this.txtUserFieldName.TabIndex = 5;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(178, 525);
            this.txtUser.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(343, 31);
            this.txtUser.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 530);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "跟踪用户:";
            // 
            // ConfigCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUserFieldName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkDisplayNull);
            this.Controls.Add(this.panelConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbDBType);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "ConfigCtl";
            this.Size = new System.Drawing.Size(882, 593);
            this.Load += new System.EventHandler(this.ConfigCtl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDBType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelConfig;
        private System.Windows.Forms.CheckBox chkDisplayNull;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserFieldName;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label3;
    }
}
