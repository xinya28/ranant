using System;
using System.Windows.Forms;
using ranant.Services;

namespace ranant.Pages
{
    public partial class MainHome : UserControl
    {
        public event EventHandler ConfirmClicked;
        public event EventHandler SettingsClicked;

        public MainHome()
        {
            InitializeComponent();

            // 初始化算法下拉框
            cboMethod.Items.Clear();
            cboMethod.Items.AddRange(new string[] { "C#Random", "MT19937", "Xorshift", "LCG", "CryptoRandom" });

            // 绑定按钮事件
            btnConfirm.Click += (s, e) => ConfirmClicked?.Invoke(this, EventArgs.Empty);
            btnSettings.Click += (s, e) => SettingsClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 从设置中加载上次保存的值
        /// </summary>
        public void LoadFromSettings()
        {
            // 加载人数
            int lastNumber = AppSettings.LastTotalNumber;
            if (lastNumber >= numInputPeople.Minimum && lastNumber <= numInputPeople.Maximum)
                numInputPeople.Value = lastNumber;
            else
                numInputPeople.Value = 45;

            // 加载算法
            string lastAlgo = AppSettings.LastAlgorithm;
            int index = cboMethod.Items.IndexOf(lastAlgo);
            if (index >= 0)
                cboMethod.SelectedIndex = index;
            else
                cboMethod.SelectedIndex = 0;
        }

        /// <summary>
        /// 将当前选择保存到设置中
        /// </summary>
        public void SaveToSettings()
        {
            AppSettings.LastTotalNumber = GetSelectedNumber();
            AppSettings.LastAlgorithm = GetSelectedMethod();
            AppSettings.Save();
        }

        // 公开获取当前选择的方法
        public int GetSelectedNumber() => (int)numInputPeople.Value;
        public string GetSelectedMethod() => cboMethod.SelectedItem?.ToString() ?? "C#Random";
        public bool GetUniqueMode() => checkBox1.Checked;
    }
}