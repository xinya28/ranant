namespace ranant.Pages
{
    partial class RunDraw
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBack = new AntdUI.ButtonShadow();
            this.progress = new AntdUI.Progress();
            this.draw = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lstDrawnResults = new System.Windows.Forms.ListBox();
            this.startORstop = new AntdUI.ButtonShadow();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("JetBrains Mono", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(0, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(50, 50);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "<";
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(45, 412);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(312, 34);
            this.progress.TabIndex = 4;
            this.progress.Text = "progress1";
            // 
            // draw
            // 
            this.draw.AutoSize = true;
            this.draw.Font = new System.Drawing.Font("Microsoft YaHei UI", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.draw.Location = new System.Drawing.Point(3, 109);
            this.draw.Name = "draw";
            this.draw.Size = new System.Drawing.Size(279, 104);
            this.draw.TabIndex = 6;
            this.draw.Text = "00000";
            this.draw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelInfo.Location = new System.Drawing.Point(0, 0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(32, 17);
            this.labelInfo.TabIndex = 7;
            this.labelInfo.Text = "状态";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelInfo);
            this.panel1.Location = new System.Drawing.Point(56, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 50);
            this.panel1.TabIndex = 8;
            // 
            // lstDrawnResults
            // 
            this.lstDrawnResults.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.lstDrawnResults.FormattingEnabled = true;
            this.lstDrawnResults.IntegralHeight = false;
            this.lstDrawnResults.ItemHeight = 21;
            this.lstDrawnResults.Location = new System.Drawing.Point(311, 54);
            this.lstDrawnResults.Name = "lstDrawnResults";
            this.lstDrawnResults.Size = new System.Drawing.Size(89, 352);
            this.lstDrawnResults.TabIndex = 9;
            // 
            // startORstop
            // 
            this.startORstop.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startORstop.Location = new System.Drawing.Point(21, 260);
            this.startORstop.Name = "startORstop";
            this.startORstop.Size = new System.Drawing.Size(239, 85);
            this.startORstop.TabIndex = 10;
            this.startORstop.Text = "开始/停止";
            // 
            // RunDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startORstop);
            this.Controls.Add(this.lstDrawnResults);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.draw);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.btnBack);
            this.Name = "RunDraw";
            this.Size = new System.Drawing.Size(400, 500);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AntdUI.ButtonShadow btnBack;
        private AntdUI.Progress progress;
        private System.Windows.Forms.Label draw;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lstDrawnResults;
        private AntdUI.ButtonShadow startORstop;
    }
}
