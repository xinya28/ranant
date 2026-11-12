using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ranant.Services
{
    /// <summary>
    /// 无界面自检：验证设置读写与版本号，供 CI 在构建后跑一次冒烟测试。
    /// 用法：Ranant.exe --self-test（可用 RANANT_SETTINGS_PATH / RANANT_SELFTEST_LOG 指定路径）
    /// </summary>
    public static class SelfTest
    {
        public static int Run()
        {
            var lines = new List<string>();

            try
            {
                AppSettings.Save();
                lines.Add("initial storage: " + (AppSettings.StoragePath ?? "<memory only>"));
                lines.Add("initial error: " + (AppSettings.LastError ?? "-"));

                AppSettings.LastTotalNumber = 47;
                AppSettings.LastAlgorithm = "MT19937";
                AppSettings.LastUpdateSource = "GhProxy";
                AppSettings.CustomUpdateSource = "https://example.com/mirror/";
                AppSettings.FluentEnabled = true;
                AppSettings.FluentThemeMode = "Dark";

                var saved = AppSettings.Save();
                lines.Add("save ok: " + saved);
                lines.Add("save path: " + (AppSettings.StoragePath ?? "<none>"));
                lines.Add("save error: " + (AppSettings.LastError ?? "-"));

                AppSettings.Load();
                var roundTrip =
                    AppSettings.LastTotalNumber == 47 &&
                    AppSettings.LastAlgorithm == "MT19937" &&
                    AppSettings.LastUpdateSource == "GhProxy" &&
                    AppSettings.CustomUpdateSource == "https://example.com/mirror/" &&
                    AppSettings.FluentEnabled &&
                    AppSettings.FluentThemeMode == "Dark";

                lines.Add("round trip: " + roundTrip);
                lines.Add("version: " + typeof(SelfTest).Assembly.GetName().Version);
                lines.Add("theme: dark=" + UI.FluentTheme.IsDark + " accent=" + UI.FluentTheme.Accent);

                // 复现崩溃现场：主窗体 + 设置页的构造与保存路径必须不再抛异常。
                var uiOk = true;
                try
                {
                    using (var form = new MainForm())
                    {
                        lines.Add("ui: MainForm ok");
                    }

                    using (var page = new Pages.MainSettings())
                    {
                        lines.Add("ui: MainSettings ok");
                    }

                    lines.Add("ui: save ok = " + AppSettings.Save());
                }
                catch (Exception ex)
                {
                    uiOk = false;
                    lines.Add("ui exception: " + ex);
                }

                WriteLog(lines);
                return saved && roundTrip && uiOk ? 0 : 2;
            }
            catch (Exception ex)
            {
                lines.Add("exception: " + ex);
                WriteLog(lines);
                return 1;
            }
        }

        private static void WriteLog(List<string> lines)
        {
            try
            {
                var target = Environment.GetEnvironmentVariable("RANANT_SELFTEST_LOG");
                if (string.IsNullOrWhiteSpace(target))
                {
                    target = Path.Combine(Path.GetTempPath(), "ranant-selftest.log");
                }

                var directory = Path.GetDirectoryName(target);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllLines(target, lines.ToArray(), new UTF8Encoding(false));
            }
            catch
            {
                // 自检日志写不出来也不影响退出码。
            }
        }
    }
}
