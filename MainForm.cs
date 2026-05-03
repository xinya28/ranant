using ranant.Pages;
using System;
using System.Windows.Forms;

namespace ranant
{
    public partial class MainForm : Form
    {
        private MainHome mainHome;
        private RunDraw runDraw;
        private MainSettings mainSettings;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing;
            LoadMainHome();
        }

        private void LoadMainHome()
        {
            if (mainHome == null)
            {
                mainHome = new MainHome();
                mainHome.Dock = DockStyle.Fill;
                mainHome.ConfirmClicked += MainHome_ConfirmClicked;
                mainHome.SettingsClicked += MainHome_SettingsClicked;
                panelContainer.Controls.Add(mainHome);
                // 加载上次保存的设置
                mainHome.LoadFromSettings();
            }
            ShowControl(mainHome);
        }

        private void LoadRunDraw()
        {
            if (runDraw == null)
            {
                runDraw = new RunDraw();
                runDraw.Dock = DockStyle.Fill;
                panelContainer.Controls.Add(runDraw);
            }
        }

        private void LoadMainSettings()
        {
            if (mainSettings == null)
            {
                mainSettings = new MainSettings();
                mainSettings.Dock = DockStyle.Fill;
                panelContainer.Controls.Add(mainSettings);
            }
        }

        private void MainHome_ConfirmClicked(object sender, EventArgs e)
        {
            // 进入抽取界面之前先保存设置
            mainHome.SaveToSettings();

            int count = mainHome.GetSelectedNumber();
            string method = mainHome.GetSelectedMethod();
            bool unique = mainHome.GetUniqueMode();

            LoadRunDraw();
            runDraw.SetDrawParameters(count, method, unique);
            ShowControl(runDraw);
        }

        private void MainHome_SettingsClicked(object sender, EventArgs e)
        {
            // 进入设置界面之前保存设置
            mainHome.SaveToSettings();
            LoadMainSettings();
            ShowControl(mainSettings);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 关闭窗口时保存设置（如果主界面已创建）
            mainHome?.SaveToSettings();
        }

        public void ShowMainHome()
        {
            ShowControl(mainHome);
        }

        private void ShowControl(UserControl controlToShow)
        {
            if (mainHome != null) mainHome.Visible = false;
            if (runDraw != null) runDraw.Visible = false;
            if (mainSettings != null) mainSettings.Visible = false;
            controlToShow.Visible = true;
            controlToShow.BringToFront();
        }
    }
}