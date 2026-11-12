using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ranant.Properties;
using ranant.Services;
using ranant.UI;

namespace ranant.Pages
{
    public partial class MainSettings : UserControl
    {
        private UpdateInfo latestUpdate;
        private bool checking;
        private bool downloading;

        public MainSettings()
        {
            InitializeComponent();

            btnBack.Click += (s, e) =>
            {
                var mainForm = this.FindForm() as MainForm;
                mainForm?.ShowMainHome();
            };

            cboSource.SelectedIndexChanged += CboSource_SelectedIndexChanged;
            cboTheme.SelectedIndexChanged += CboTheme_SelectedIndexChanged;
            chkFluent.CheckedChanged += ChkFluent_CheckedChanged;
            txtCustomSource.TextChanged += TxtCustomSource_TextChanged;
            btnCheck.Click += BtnCheck_Click;
            btnDownload.Click += BtnDownload_Click;
            btnRelease.Click += BtnRelease_Click;

            LoadFromSettings();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyTheme();
        }

        private void LoadFromSettings()
        {
            lblVersion.Text = "当前版本：v" + UpdateService.CurrentVersionText;

            var source = UpdateService.ParseSource(Settings.Default.LastUpdateSource);
            cboSource.SelectedIndex = (int)source;
            txtCustomSource.Text = Settings.Default.CustomUpdateSource ?? string.Empty;
            txtCustomSource.Visible = source == UpdateSource.Custom;

            chkFluent.Checked = Settings.Default.FluentEnabled;
            cboTheme.SelectedIndex = (int)FluentTheme.ParseMode(Settings.Default.FluentThemeMode);
            cboTheme.Enabled = chkFluent.Checked;
        }

        private UpdateSource SelectedSource()
        {
            var index = cboSource.SelectedIndex;
            if (index < 0 || index > (int)UpdateSource.Custom)
            {
                return UpdateSource.GitHub;
            }

            return (UpdateSource)index;
        }

        private void CboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var source = SelectedSource();
            txtCustomSource.Visible = source == UpdateSource.Custom;
            Settings.Default.LastUpdateSource = source.ToString();
            Settings.Default.Save();
            latestUpdate = null;
            btnDownload.Enabled = false;
        }

        private void TxtCustomSource_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.CustomUpdateSource = txtCustomSource.Text.Trim();
            Settings.Default.Save();
        }

        private void ChkFluent_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.FluentEnabled = chkFluent.Checked;
            Settings.Default.Save();
            cboTheme.Enabled = chkFluent.Checked;
            ApplyTheme();
        }

        private void CboTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.FluentThemeMode = ((FluentThemeMode)cboTheme.SelectedIndex).ToString();
            Settings.Default.Save();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var enabled = Settings.Default.FluentEnabled;
            var mode = FluentTheme.ParseMode(Settings.Default.FluentThemeMode);

            var form = FindForm();
            if (form != null)
            {
                FluentTheme.Apply(form, enabled, mode);
            }
            else
            {
                FluentTheme.ApplyTo(this);
            }

            lblVersion.Text = "当前版本：v" + UpdateService.CurrentVersionText;
        }

        private async void BtnCheck_Click(object sender, EventArgs e)
        {
            if (checking)
            {
                return;
            }

            var source = SelectedSource();
            if (source == UpdateSource.Custom && string.IsNullOrWhiteSpace(txtCustomSource.Text))
            {
                lblStatus.Text = "请先填写自定义更新源地址（例如 https://gh-proxy.com/）。";
                txtCustomSource.Focus();
                return;
            }

            checking = true;
            btnCheck.Enabled = false;
            btnDownload.Enabled = false;
            btnCheck.Text = "检查中…";
            lblStatus.Text = "正在通过" + UpdateService.Describe(source) + "检查更新…";

            try
            {
                var info = await UpdateService.CheckAsync(source, txtCustomSource.Text, CancellationToken.None);
                if (IsDisposed)
                {
                    return;
                }

                if (info.Version > UpdateService.CurrentVersion)
                {
                    latestUpdate = info;
                    var availability = string.IsNullOrEmpty(info.AssetUrl) ? "请到发布页下载" : "可下载安装包";
                    lblStatus.Text = "发现新版本 v" + info.Version + "（" + info.TagName + "），" + availability + "。" +
                                     (string.IsNullOrEmpty(info.AssetName) ? string.Empty : " 文件：" + info.AssetName);
                    btnDownload.Enabled = !string.IsNullOrEmpty(info.AssetUrl);
                }
                else
                {
                    latestUpdate = null;
                    lblStatus.Text = "已是最新版本 v" + UpdateService.CurrentVersionText + "（检查源：" +
                                     UpdateService.Describe(info.Source) + "）。";
                    btnDownload.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                if (IsDisposed)
                {
                    return;
                }

                latestUpdate = null;
                lblStatus.Text = ex.Message + " 可尝试把更新源切换为镜像后重试。";
                btnDownload.Enabled = false;
            }
            finally
            {
                if (!IsDisposed)
                {
                    checking = false;
                    btnCheck.Enabled = true;
                    btnCheck.Text = "检查更新";
                }
            }
        }

        private async void BtnDownload_Click(object sender, EventArgs e)
        {
            if (downloading || latestUpdate == null)
            {
                return;
            }

            var url = UpdateService.GetDownloadUrl(latestUpdate, txtCustomSource.Text);
            if (string.IsNullOrEmpty(url))
            {
                lblStatus.Text = "该版本没有可下载的安装包，请使用“打开发布页”。";
                return;
            }

            var suggested = string.IsNullOrEmpty(latestUpdate.AssetName)
                ? "Ranant-" + latestUpdate.Version + ".exe"
                : latestUpdate.AssetName;

            using (var dialog = new SaveFileDialog())
            {
                dialog.Title = "保存更新安装包";
                dialog.FileName = suggested;
                dialog.Filter = "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*";
                dialog.OverwritePrompt = true;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                downloading = true;
                btnDownload.Enabled = false;
                btnCheck.Enabled = false;

                try
                {
                    var path = dialog.FileName;
                    var progress = new Progress<int>(percent =>
                    {
                        if (!IsDisposed)
                        {
                            lblStatus.Text = "正在下载 " + percent + "% → " + Path.GetFileName(path);
                        }
                    });

                    await UpdateService.DownloadAsync(url, path, progress, CancellationToken.None);
                    if (IsDisposed)
                    {
                        return;
                    }

                    lblStatus.Text = "下载完成：" + path;
                    var answer = MessageBox.Show(
                        this,
                        "更新安装包已保存到：\r\n" + path + "\r\n\r\n是否打开所在文件夹？",
                        "下载完成",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (answer == DialogResult.Yes)
                    {
                        Process.Start("explorer.exe", "/select,\"" + path + "\"");
                    }
                }
                catch (Exception ex)
                {
                    if (!IsDisposed)
                    {
                        lblStatus.Text = "下载失败：" + ex.Message;
                    }
                }
                finally
                {
                    downloading = false;
                    if (!IsDisposed)
                    {
                        btnDownload.Enabled = latestUpdate != null && !string.IsNullOrEmpty(latestUpdate.AssetUrl);
                        btnCheck.Enabled = true;
                    }
                }
            }
        }

        private void BtnRelease_Click(object sender, EventArgs e)
        {
            var url = UpdateService.ReleasesPageUrl;
            if (latestUpdate != null && !string.IsNullOrEmpty(latestUpdate.ReleaseUrl))
            {
                url = latestUpdate.ReleaseUrl;
            }

            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "无法打开浏览器：" + ex.Message;
            }
        }
    }
}
