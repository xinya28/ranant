using ranant.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            LoadUserControls();
        }

        public void ShowMainHome()
        {
            ShowControl(mainHome);
        }
        private void LoadUserControls()
        {
            mainHome = new MainHome();
            runDraw = new RunDraw();
            mainSettings = new MainSettings();

            panelContainer.Controls.Add(mainHome);
            panelContainer.Controls.Add(runDraw);
            panelContainer.Controls.Add(mainSettings);

            mainHome.Dock = DockStyle.Fill;
            runDraw.Dock = DockStyle.Fill;
            mainSettings.Dock = DockStyle.Fill;

            mainHome.ConfirmClicked += MainHome_ConfirmClicked;
            mainHome.SettingsClicked += MainHome_SettingsClicked;

            ShowControl(mainHome);

        }
        private void MainHome_ConfirmClicked(object sender, EventArgs e)
        {
            // 从主界面获取人数和算法
            int peopleCount = mainHome.GetSelectedNumber();
            string method = mainHome.GetSelectedMethod();

            // 你可以把这些数据传递给抽取界面（例如通过一个公共属性）
            runDraw.SetDrawParameters(peopleCount, method);

            // 切换到抽取界面
            ShowControl(runDraw);
        }
        private void MainHome_SettingsClicked(object sender, EventArgs e)
        {
            // 切换到设置界面
            ShowControl(mainSettings);
        }
        private void ShowControl(UserControl controlToShow)
        {
            mainHome.Visible = false;
            runDraw.Visible = false;
            mainSettings.Visible = false;
            controlToShow.Visible = true;
            controlToShow.BringToFront();
        }

    }
}
