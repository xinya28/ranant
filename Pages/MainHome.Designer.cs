namespace ranant.Pages
{
    partial class MainHome
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
            this.btnConfirm = new AntdUI.ButtonShadow();
            this.btnSettings = new AntdUI.ButtonShadow();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.numInputPeople = new AntdUI.InputNumber();
            this.算法选择 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(90, 308);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(200, 49);
            this.btnConfirm.TabIndex = 4;
            this.btnConfirm.Text = "确认";
            // 
            // btnSettings
            // 
            this.btnSettings.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.btnSettings.ForeHover = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(169)))), ((int)(((byte)(244)))));
            this.btnSettings.Location = new System.Drawing.Point(0, 0);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(50, 50);
            this.btnSettings.TabIndex = 5;
            this.btnSettings.Text = "⚙";
            // 
            // cboMethod
            // 
            this.cboMethod.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboMethod.FormattingEnabled = true;
            this.cboMethod.Items.AddRange(new object[] {
            "C#Random",
            "MT19937"});
            this.cboMethod.Location = new System.Drawing.Point(144, 219);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(200, 29);
            this.cboMethod.TabIndex = 6;
            this.cboMethod.Text = "C#Random";
            // 
            // numInputPeople
            // 
            this.numInputPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numInputPeople.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.numInputPeople.Font = new System.Drawing.Font("JetBrains Mono", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numInputPeople.Location = new System.Drawing.Point(85, 91);
            this.numInputPeople.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numInputPeople.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInputPeople.Name = "numInputPeople";
            this.numInputPeople.Radius = 5;
            this.numInputPeople.SelectionColor = System.Drawing.SystemColors.Highlight;
            this.numInputPeople.Size = new System.Drawing.Size(230, 112);
            this.numInputPeople.TabIndex = 0;
            this.numInputPeople.Text = "45";
            this.numInputPeople.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numInputPeople.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // 算法选择
            // 
            this.算法选择.AutoSize = true;
            this.算法选择.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.算法选择.Location = new System.Drawing.Point(50, 217);
            this.算法选择.Name = "算法选择";
            this.算法选择.Size = new System.Drawing.Size(54, 28);
            this.算法选择.TabIndex = 7;
            this.算法选择.Text = "算法";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBox1.Location = new System.Drawing.Point(144, 263);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(51, 21);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "去重";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // MainHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.算法选择);
            this.Controls.Add(this.cboMethod);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.numInputPeople);
            this.Name = "MainHome";
            this.Size = new System.Drawing.Size(400, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AntdUI.ButtonShadow btnConfirm;
        private AntdUI.ButtonShadow btnSettings;
        private System.Windows.Forms.ComboBox cboMethod;
        private AntdUI.InputNumber numInputPeople;
        private System.Windows.Forms.Label 算法选择;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
