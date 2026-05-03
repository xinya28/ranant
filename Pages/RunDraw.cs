using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ranant.Pages
{
    public partial class RunDraw : UserControl
    {
        private int targetCount;
        private string algorithm;
        public RunDraw()
        {
            InitializeComponent();
            btnBack.Click += (s, e) =>
            {
                var mainForm = this.FindForm() as MainForm;
                mainForm?.ShowMainHome();
            };
        }
        public void SetDrawParameters(int count, string method)
        {
            targetCount = count;
            algorithm = method;
            // 可选：立即开始抽选，或者显示“准备就绪”
            labelInfo.Text = $"准备抽选 {targetCount} 人，算法：{algorithm}";
        }
    }
}
