namespace ranant.Pages
{
    partial class MainSettings
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.cardUpdate = new ranant.UI.FluentCard();
            this.lblUpdateTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblSourceLabel = new System.Windows.Forms.Label();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.txtCustomSource = new System.Windows.Forms.TextBox();
            this.btnCheck = new ranant.UI.FluentButton();
            this.btnDownload = new ranant.UI.FluentButton();
            this.btnRelease = new ranant.UI.FluentButton();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cardUI = new ranant.UI.FluentCard();
            this.lblUITitle = new System.Windows.Forms.Label();
            this.chkFluent = new System.Windows.Forms.CheckBox();
            this.lblThemeLabel = new System.Windows.Forms.Label();
            this.cboTheme = new System.Windows.Forms.ComboBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.cardUpdate.SuspendLayout();
            this.cardUI.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("JetBrains Mono", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(0, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(50, 50);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "<";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(56, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(72, 28);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "设置";
            // 
            // cardUpdate
            // 
            this.cardUpdate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.cardUpdate.Controls.Add(this.lblStatus);
            this.cardUpdate.Controls.Add(this.btnRelease);
            this.cardUpdate.Controls.Add(this.btnDownload);
            this.cardUpdate.Controls.Add(this.btnCheck);
            this.cardUpdate.Controls.Add(this.txtCustomSource);
            this.cardUpdate.Controls.Add(this.cboSource);
            this.cardUpdate.Controls.Add(this.lblSourceLabel);
            this.cardUpdate.Controls.Add(this.lblVersion);
            this.cardUpdate.Controls.Add(this.lblUpdateTitle);
            this.cardUpdate.Location = new System.Drawing.Point(20, 60);
            this.cardUpdate.Name = "cardUpdate";
            this.cardUpdate.Radius = 8;
            this.cardUpdate.Size = new System.Drawing.Size(344, 214);
            this.cardUpdate.TabIndex = 2;
            // 
            // lblUpdateTitle
            // 
            this.lblUpdateTitle.AutoSize = true;
            this.lblUpdateTitle.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUpdateTitle.Location = new System.Drawing.Point(16, 12);
            this.lblUpdateTitle.Name = "lblUpdateTitle";
            this.lblUpdateTitle.Size = new System.Drawing.Size(37, 19);
            this.lblUpdateTitle.TabIndex = 0;
            this.lblUpdateTitle.Text = "更新";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVersion.Location = new System.Drawing.Point(16, 40);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(110, 17);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "当前版本：v1.0.0.4";
            // 
            // lblSourceLabel
            // 
            this.lblSourceLabel.AutoSize = true;
            this.lblSourceLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSourceLabel.Location = new System.Drawing.Point(16, 70);
            this.lblSourceLabel.Name = "lblSourceLabel";
            this.lblSourceLabel.Size = new System.Drawing.Size(44, 17);
            this.lblSourceLabel.TabIndex = 2;
            this.lblSourceLabel.Text = "更新源";
            // 
            // cboSource
            // 
            this.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboSource.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Items.AddRange(new object[] {
            "GitHub 官方",
            "镜像 gh-proxy.com",
            "镜像 ghfast.top",
            "自定义"});
            this.cboSource.Location = new System.Drawing.Point(76, 66);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(236, 25);
            this.cboSource.TabIndex = 3;
            // 
            // txtCustomSource
            // 
            this.txtCustomSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomSource.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCustomSource.Location = new System.Drawing.Point(76, 97);
            this.txtCustomSource.Name = "txtCustomSource";
            this.txtCustomSource.Size = new System.Drawing.Size(236, 23);
            this.txtCustomSource.TabIndex = 4;
            this.txtCustomSource.Visible = false;
            // 
            // btnCheck
            // 
            this.btnCheck.Kind = ranant.UI.FluentButtonKind.Primary;
            this.btnCheck.Location = new System.Drawing.Point(16, 128);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(100, 32);
            this.btnCheck.TabIndex = 5;
            this.btnCheck.Text = "检查更新";
            // 
            // btnDownload
            // 
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new System.Drawing.Point(124, 128);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 32);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "下载安装包";
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(232, 128);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(96, 32);
            this.btnRelease.TabIndex = 7;
            this.btnRelease.Text = "打开发布页";
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStatus.Location = new System.Drawing.Point(16, 168);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(312, 38);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Tag = "secondary";
            this.lblStatus.Text = "未检查更新";
            // 
            // cardUI
            // 
            this.cardUI.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.cardUI.Controls.Add(this.lblHint);
            this.cardUI.Controls.Add(this.cboTheme);
            this.cardUI.Controls.Add(this.lblThemeLabel);
            this.cardUI.Controls.Add(this.chkFluent);
            this.cardUI.Controls.Add(this.lblUITitle);
            this.cardUI.Location = new System.Drawing.Point(20, 286);
            this.cardUI.Name = "cardUI";
            this.cardUI.Radius = 8;
            this.cardUI.Size = new System.Drawing.Size(344, 150);
            this.cardUI.TabIndex = 3;
            // 
            // lblUITitle
            // 
            this.lblUITitle.AutoSize = true;
            this.lblUITitle.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUITitle.Location = new System.Drawing.Point(16, 12);
            this.lblUITitle.Name = "lblUITitle";
            this.lblUITitle.Size = new System.Drawing.Size(37, 19);
            this.lblUITitle.TabIndex = 0;
            this.lblUITitle.Text = "界面";
            // 
            // chkFluent
            // 
            this.chkFluent.AutoSize = true;
            this.chkFluent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkFluent.Location = new System.Drawing.Point(16, 40);
            this.chkFluent.Name = "chkFluent";
            this.chkFluent.Size = new System.Drawing.Size(196, 21);
            this.chkFluent.TabIndex = 1;
            this.chkFluent.Text = "启用 Fluent Design 外观";
            this.chkFluent.UseVisualStyleBackColor = false;
            // 
            // lblThemeLabel
            // 
            this.lblThemeLabel.AutoSize = true;
            this.lblThemeLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblThemeLabel.Location = new System.Drawing.Point(16, 74);
            this.lblThemeLabel.Name = "lblThemeLabel";
            this.lblThemeLabel.Size = new System.Drawing.Size(32, 17);
            this.lblThemeLabel.TabIndex = 2;
            this.lblThemeLabel.Text = "主题";
            // 
            // cboTheme
            // 
            this.cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTheme.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboTheme.FormattingEnabled = true;
            this.cboTheme.Items.AddRange(new object[] {
            "跟随系统",
            "浅色",
            "深色"});
            this.cboTheme.Location = new System.Drawing.Point(76, 70);
            this.cboTheme.Name = "cboTheme";
            this.cboTheme.Size = new System.Drawing.Size(140, 25);
            this.cboTheme.TabIndex = 3;
            // 
            // lblHint
            // 
            this.lblHint.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHint.Location = new System.Drawing.Point(16, 106);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(312, 32);
            this.lblHint.TabIndex = 4;
            this.lblHint.Tag = "secondary";
            this.lblHint.Text = "圆角卡片与系统主题色；Windows 11 下额外应用窗口圆角与深色标题栏。";
            // 
            // MainSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cardUI);
            this.Controls.Add(this.cardUpdate);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnBack);
            this.Name = "MainSettings";
            this.Size = new System.Drawing.Size(400, 500);
            this.cardUpdate.ResumeLayout(false);
            this.cardUpdate.PerformLayout();
            this.cardUI.ResumeLayout(false);
            this.cardUI.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AntdUI.ButtonShadow btnBack;
        private System.Windows.Forms.Label lblTitle;
        private ranant.UI.FluentCard cardUpdate;
        private System.Windows.Forms.Label lblUpdateTitle;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblSourceLabel;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.TextBox txtCustomSource;
        private ranant.UI.FluentButton btnCheck;
        private ranant.UI.FluentButton btnDownload;
        private ranant.UI.FluentButton btnRelease;
        private System.Windows.Forms.Label lblStatus;
        private ranant.UI.FluentCard cardUI;
        private System.Windows.Forms.Label lblUITitle;
        private System.Windows.Forms.CheckBox chkFluent;
        private System.Windows.Forms.Label lblThemeLabel;
        private System.Windows.Forms.ComboBox cboTheme;
        private System.Windows.Forms.Label lblHint;
    }
}
