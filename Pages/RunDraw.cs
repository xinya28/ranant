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
        private List<int> drawnNumbers = new List<int>();   // 记录已抽中的学号
        private int totalStudentCount = 30;                // 总学生数，可以从主界面传入
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
            labelInfo.Text = $"准备抽选 {targetCount} 人\n" +
                $"算法：{algorithm}";
        }

        private int DrawOnce()
        {
            if (drawnNumbers.Count >= totalStudentCount)
            {
                MessageBox.Show("所有学生都已抽过！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return -1;
            }
            Random rand = new Random();
            int candidate;
            do
            {
                candidate = rand.Next(1, totalStudentCount + 1);
            } while (drawnNumbers.Contains(candidate));
            drawnNumbers.Add(candidate);
            return candidate;
        }
    }
}
