using System;
using System.Windows.Forms;

namespace ranant
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // CI 冒烟测试入口：不创建任何窗口，只验证设置读写。
            if (args != null && args.Length > 0 && string.Equals(args[0], "--self-test", StringComparison.OrdinalIgnoreCase))
            {
                Environment.ExitCode = Services.SelfTest.Run();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 设置文件、主题等都可能因为环境异常抛错，兜住它们，避免弹出“未处理的异常”。
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ReportException(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => ReportException(e.ExceptionObject as Exception);

            Application.Run(new MainForm());
        }

        private static void ReportException(Exception exception)
        {
            var logPath = WriteCrashLog(exception);
            var message = "程序遇到一个未处理的错误：\r\n" +
                          (exception == null ? "未知错误" : exception.Message);

            if (!string.IsNullOrEmpty(logPath))
            {
                message += "\r\n\r\n日志已保存到：\r\n" + logPath;
            }

            message += "\r\n\r\n点“确定”可以忽略该错误继续使用。";

            try
            {
                MessageBox.Show(message, "Ranant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch
            {
                // 连对话框都弹不出来时只能放弃。
            }
        }

        private static string WriteCrashLog(Exception exception)
        {
            try
            {
                var directory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Ranant");
                System.IO.Directory.CreateDirectory(directory);

                var file = System.IO.Path.Combine(
                    directory,
                    "crash-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log");

                System.IO.File.WriteAllText(
                    file,
                    exception == null ? "unknown exception" : exception.ToString(),
                    System.Text.Encoding.UTF8);

                return file;
            }
            catch
            {
                return null;
            }
        }
    }
}
