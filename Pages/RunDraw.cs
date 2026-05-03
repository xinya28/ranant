using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ranant.RandomGenerators;

namespace ranant.Pages
{
    public partial class RunDraw : UserControl
    {
        private int totalStudentCount;
        private string algorithm;
        private bool uniqueMode;

        private List<int> drawnNumbers = new List<int>();
        private IRandomGenerator randGen;

        private bool isRolling = false;
        private Timer rollTimer;
        private int rollingValue;

        public RunDraw()
        {
            InitializeComponent();
            btnBack.Click += (s, e) =>
            {
                var mainForm = this.FindForm() as MainForm;
                mainForm?.ShowMainHome();
            };
            startORstop.Click += StartOrStop_Click;
            reset.Click += Reset_Click;

            rollTimer = new Timer();
            rollTimer.Interval = 50;
            rollTimer.Tick += RollTimer_Tick;
        }

        public void SetDrawParameters(int count, string method, bool unique)
        {
            totalStudentCount = count;
            algorithm = method;
            uniqueMode = unique;
            ResetDrawState();

            // 传统 switch 语句，兼容 C# 7.3
            switch (method)
            {
                case "C#Random":
                    randGen = new CSharpRandomGenerator();
                    break;
                case "MT19937":
                    randGen = new Mt19937Generator();
                    break;
                case "Xorshift":
                    randGen = new XorshiftGenerator();
                    break;
                case "LCG":
                    randGen = new LcGGenerator();
                    break;
                case "CryptoRandom":
                    randGen = new CryptoRandomGenerator();
                    break;
                default:
                    randGen = new CSharpRandomGenerator();
                    break;
            }

            labelInfo.Text = $"总人数：{totalStudentCount}\n算法：{algorithm}\n去重：{(uniqueMode ? "是" : "否")}";
        }

        private void ResetDrawState()
        {
            if (isRolling)
            {
                isRolling = false;
                rollTimer.Stop();
            }
            drawnNumbers.Clear();
            lstDrawnResults.Items.Clear();
            draw.Text = "?????";
            rollingValue = 0;
            startORstop.Text = "开始";
            startORstop.Enabled = true;
        }

        private int DrawOnce()
        {
            if (uniqueMode && drawnNumbers.Count >= totalStudentCount)
                return -1;

            int candidate = randGen.Next(1, totalStudentCount + 1);

            if (uniqueMode)
            {
                while (drawnNumbers.Contains(candidate))
                {
                    candidate = randGen.Next(1, totalStudentCount + 1);
                }
                drawnNumbers.Add(candidate);
            }
            return candidate;
        }

        private void PerformDraw()
        {
            if (isRolling)
            {
                isRolling = false;
                rollTimer.Stop();
                startORstop.Text = "开始";
            }

            int result;
            if (rollingValue != 0)
            {
                result = rollingValue;
                if (uniqueMode && drawnNumbers.Contains(result))
                {
                    result = DrawOnce();
                }
                else if (uniqueMode)
                {
                    drawnNumbers.Add(result);
                }
                rollingValue = 0;
            }
            else
            {
                result = DrawOnce();
            }

            if (result == -1)
            {
                startORstop.Text = "已全抽完";
                startORstop.Enabled = false;
                return;
            }

            draw.Text = result.ToString();
            lstDrawnResults.Items.Insert(0, $"{result}");
            if (lstDrawnResults.Items.Count > 0)
                lstDrawnResults.TopIndex = 0;

            if (uniqueMode && drawnNumbers.Count >= totalStudentCount)
            {
                startORstop.Text = "已全抽完";
                startORstop.Enabled = false;
            }
        }

        private void RollTimer_Tick(object sender, EventArgs e)
        {
            rollingValue = randGen.Next(1, totalStudentCount + 1);
            draw.Text = rollingValue.ToString();
        }

        private void StartOrStop_Click(object sender, EventArgs e)
        {
            if (!startORstop.Enabled) return;

            if (uniqueMode && drawnNumbers.Count >= totalStudentCount)
            {
                startORstop.Text = "已全抽完";
                startORstop.Enabled = false;
                return;
            }

            if (isRolling)
            {
                PerformDraw();
            }
            else
            {
                isRolling = true;
                rollTimer.Start();
                startORstop.Text = "停止";
            }
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            ResetDrawState();
        }
    }
}