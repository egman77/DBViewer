namespace DBViewer.UI
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabViewer = new System.Windows.Forms.TabPage();
            this.tabReport = new System.Windows.Forms.TabControl();
            this.pageChangeList = new System.Windows.Forms.TabPage();
            this.changeListViewer1 = new DBViewer.UI.ChangeListViewer();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.btnView = new System.Windows.Forms.ToolStripButton();
            this.viewDiff = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.pageDiff = new System.Windows.Forms.TabPage();
            this.diffListViewer1 = new DBViewer.UI.DiffListViewer();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.btnRebuild = new System.Windows.Forms.Button();
            this.btnDeleteTrigger = new System.Windows.Forms.Button();
            this.tabParam = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.configCtl1 = new DBViewer.UI.ConfigCtl();
            this.tabMain.SuspendLayout();
            this.tabViewer.SuspendLayout();
            this.tabReport.SuspendLayout();
            this.pageChangeList.SuspendLayout();
            this.toolMain.SuspendLayout();
            this.pageDiff.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.tabParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("宋体", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1135, 77);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库跟踪";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabViewer);
            this.tabMain.Controls.Add(this.tabTools);
            this.tabMain.Controls.Add(this.tabParam);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 77);
            this.tabMain.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1135, 689);
            this.tabMain.TabIndex = 1;
            // 
            // tabViewer
            // 
            this.tabViewer.Controls.Add(this.tabReport);
            this.tabViewer.Location = new System.Drawing.Point(4, 31);
            this.tabViewer.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabViewer.Name = "tabViewer";
            this.tabViewer.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabViewer.Size = new System.Drawing.Size(1127, 654);
            this.tabViewer.TabIndex = 0;
            this.tabViewer.Text = "记录查看";
            this.tabViewer.UseVisualStyleBackColor = true;
            // 
            // tabReport
            // 
            this.tabReport.Controls.Add(this.pageChangeList);
            this.tabReport.Controls.Add(this.pageDiff);
            this.tabReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabReport.Location = new System.Drawing.Point(6, 5);
            this.tabReport.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabReport.Name = "tabReport";
            this.tabReport.SelectedIndex = 0;
            this.tabReport.Size = new System.Drawing.Size(1115, 644);
            this.tabReport.TabIndex = 2;
            // 
            // pageChangeList
            // 
            this.pageChangeList.Controls.Add(this.changeListViewer1);
            this.pageChangeList.Controls.Add(this.toolMain);
            this.pageChangeList.Location = new System.Drawing.Point(4, 31);
            this.pageChangeList.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pageChangeList.Name = "pageChangeList";
            this.pageChangeList.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pageChangeList.Size = new System.Drawing.Size(1107, 609);
            this.pageChangeList.TabIndex = 0;
            this.pageChangeList.Text = "变更记录";
            this.pageChangeList.UseVisualStyleBackColor = true;
            // 
            // changeListViewer1
            // 
            this.changeListViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.changeListViewer1.Location = new System.Drawing.Point(6, 40);
            this.changeListViewer1.Margin = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.changeListViewer1.Name = "changeListViewer1";
            this.changeListViewer1.Size = new System.Drawing.Size(1095, 564);
            this.changeListViewer1.TabIndex = 1;
            this.changeListViewer1.Table = null;
            // 
            // toolMain
            // 
            this.toolMain.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnView,
            this.viewDiff,
            this.btnClear});
            this.toolMain.Location = new System.Drawing.Point(6, 5);
            this.toolMain.Name = "toolMain";
            this.toolMain.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolMain.Size = new System.Drawing.Size(1095, 35);
            this.toolMain.TabIndex = 2;
            this.toolMain.Text = "toolStrip1";
            // 
            // btnView
            // 
            this.btnView.Image = ((System.Drawing.Image)(resources.GetObject("btnView.Image")));
            this.btnView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(86, 32);
            this.btnView.Text = "显示";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // viewDiff
            // 
            this.viewDiff.Image = ((System.Drawing.Image)(resources.GetObject("viewDiff.Image")));
            this.viewDiff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.viewDiff.Name = "viewDiff";
            this.viewDiff.Size = new System.Drawing.Size(86, 32);
            this.viewDiff.Text = "差分";
            this.viewDiff.Click += new System.EventHandler(this.viewDiff_Click);
            // 
            // btnClear
            // 
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(128, 32);
            this.btnClear.Text = "删除数据";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pageDiff
            // 
            this.pageDiff.Controls.Add(this.diffListViewer1);
            this.pageDiff.Location = new System.Drawing.Point(4, 31);
            this.pageDiff.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pageDiff.Name = "pageDiff";
            this.pageDiff.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.pageDiff.Size = new System.Drawing.Size(1107, 609);
            this.pageDiff.TabIndex = 1;
            this.pageDiff.Text = "差分";
            this.pageDiff.UseVisualStyleBackColor = true;
            // 
            // diffListViewer1
            // 
            this.diffListViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diffListViewer1.Location = new System.Drawing.Point(6, 5);
            this.diffListViewer1.Margin = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.diffListViewer1.Name = "diffListViewer1";
            this.diffListViewer1.Size = new System.Drawing.Size(1095, 599);
            this.diffListViewer1.TabIndex = 0;
            this.diffListViewer1.Table = null;
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.btnRebuild);
            this.tabTools.Controls.Add(this.btnDeleteTrigger);
            this.tabTools.Location = new System.Drawing.Point(4, 31);
            this.tabTools.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabTools.Name = "tabTools";
            this.tabTools.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabTools.Size = new System.Drawing.Size(1127, 655);
            this.tabTools.TabIndex = 1;
            this.tabTools.Text = "系统工具";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // btnRebuild
            // 
            this.btnRebuild.Location = new System.Drawing.Point(40, 228);
            this.btnRebuild.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnRebuild.Name = "btnRebuild";
            this.btnRebuild.Size = new System.Drawing.Size(370, 88);
            this.btnRebuild.TabIndex = 2;
            this.btnRebuild.Text = "重建触发器";
            this.btnRebuild.UseVisualStyleBackColor = true;
            this.btnRebuild.Click += new System.EventHandler(this.btnRebuild_Click);
            // 
            // btnDeleteTrigger
            // 
            this.btnDeleteTrigger.Location = new System.Drawing.Point(40, 65);
            this.btnDeleteTrigger.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnDeleteTrigger.Name = "btnDeleteTrigger";
            this.btnDeleteTrigger.Size = new System.Drawing.Size(370, 88);
            this.btnDeleteTrigger.TabIndex = 0;
            this.btnDeleteTrigger.Text = "删除触发器";
            this.btnDeleteTrigger.UseVisualStyleBackColor = true;
            this.btnDeleteTrigger.Click += new System.EventHandler(this.btnDeleteTrigger_Click);
            // 
            // tabParam
            // 
            this.tabParam.Controls.Add(this.btnSave);
            this.tabParam.Controls.Add(this.configCtl1);
            this.tabParam.Location = new System.Drawing.Point(4, 31);
            this.tabParam.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabParam.Name = "tabParam";
            this.tabParam.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tabParam.Size = new System.Drawing.Size(1127, 655);
            this.tabParam.TabIndex = 2;
            this.tabParam.Text = "参数配置";
            this.tabParam.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(488, 30);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 40);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // configCtl1
            // 
            this.configCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configCtl1.Location = new System.Drawing.Point(6, 5);
            this.configCtl1.Margin = new System.Windows.Forms.Padding(11, 9, 11, 9);
            this.configCtl1.Name = "configCtl1";
            this.configCtl1.Size = new System.Drawing.Size(1115, 645);
            this.configCtl1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1135, 766);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "MainForm";
            this.Text = "DB Viewer";
            this.tabMain.ResumeLayout(false);
            this.tabViewer.ResumeLayout(false);
            this.tabReport.ResumeLayout(false);
            this.pageChangeList.ResumeLayout(false);
            this.pageChangeList.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            this.pageDiff.ResumeLayout(false);
            this.tabTools.ResumeLayout(false);
            this.tabParam.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabViewer;
        private System.Windows.Forms.TabPage tabTools;
        private System.Windows.Forms.TabPage tabParam;
        private ConfigCtl configCtl1;
        private System.Windows.Forms.Button btnRebuild;
        private System.Windows.Forms.Button btnDeleteTrigger;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabReport;
        private System.Windows.Forms.TabPage pageChangeList;
        private System.Windows.Forms.TabPage pageDiff;
        private ChangeListViewer changeListViewer1;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripButton btnView;
        private System.Windows.Forms.ToolStripButton viewDiff;
        private DiffListViewer diffListViewer1;
        private System.Windows.Forms.ToolStripButton btnClear;
    }
}

