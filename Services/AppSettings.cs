using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ranant.Services
{
    /// <summary>
    /// 轻量设置存储。刻意不使用 Properties.Settings（ApplicationSettingsBase）：
    /// 从 URL/网络路径启动时 .NET 会把 user.config 指到 "%LOCALAPPDATA%\{公司}\{exe}_Url_{hash}\..."，
    /// 该目录不存在且 Save 会抛异常，直接导致启动崩溃。这里改为自己写 ini，并支持多级回退：
    /// %LOCALAPPDATA%\Ranant\settings.ini → exe 同级 → %TEMP%\Ranant\settings.ini。
    /// 任何一步失败都只记录 LastError，不抛异常。
    /// </summary>
    public static class AppSettings
    {
        private const string FileName = "settings.ini";
        private const string PathOverrideVariable = "RANANT_SETTINGS_PATH";

        public static int LastTotalNumber { get; set; }
        public static string LastAlgorithm { get; set; }
        public static string LastUpdateSource { get; set; }
        public static string CustomUpdateSource { get; set; }
        public static bool FluentEnabled { get; set; }
        public static string FluentThemeMode { get; set; }

        /// <summary>实际读写的文件路径；为空表示当前只在内存里。</summary>
        public static string StoragePath { get; private set; }

        /// <summary>最近一次读写失败的原因。</summary>
        public static string LastError { get; private set; }

        static AppSettings()
        {
            RestoreDefaults();
            Load();
        }

        private static void RestoreDefaults()
        {
            LastTotalNumber = 45;
            LastAlgorithm = "C#Random";
            LastUpdateSource = "GitHub";
            CustomUpdateSource = string.Empty;
            FluentEnabled = false;
            FluentThemeMode = "Auto";
        }

        public static void Load()
        {
            RestoreDefaults();
            StoragePath = null;

            foreach (var path in CandidatePaths())
            {
                try
                {
                    if (!File.Exists(path))
                    {
                        continue;
                    }

                    Apply(Parse(File.ReadAllLines(path, Encoding.UTF8)));
                    StoragePath = path;
                    return;
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                }
            }

            TryImportLegacy();
        }

        /// <summary>写入设置。全部候选路径都失败时返回 false，并保留 LastError。</summary>
        public static bool Save()
        {
            LastError = null;

            var lines = new List<string>
            {
                "LastTotalNumber=" + LastTotalNumber.ToString(CultureInfo.InvariantCulture),
                "LastAlgorithm=" + (LastAlgorithm ?? string.Empty),
                "LastUpdateSource=" + (LastUpdateSource ?? string.Empty),
                "CustomUpdateSource=" + (CustomUpdateSource ?? string.Empty),
                "FluentEnabled=" + (FluentEnabled ? "true" : "false"),
                "FluentThemeMode=" + (FluentThemeMode ?? string.Empty)
            };

            foreach (var path in CandidatePaths())
            {
                try
                {
                    var directory = Path.GetDirectoryName(path);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.WriteAllLines(path, lines.ToArray(), new UTF8Encoding(false));
                    StoragePath = path;
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                }
            }

            return false;
        }

        private static List<string> CandidatePaths()
        {
            var paths = new List<string>();

            try
            {
                var overridePath = Environment.GetEnvironmentVariable(PathOverrideVariable);
                if (!string.IsNullOrWhiteSpace(overridePath))
                {
                    paths.Add(overridePath.Trim());
                    return paths;
                }
            }
            catch
            {
                // 环境变量读不到就继续用默认路径。
            }

            AddCandidate(paths, delegate
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Ranant",
                    FileName);
            });

            AddCandidate(paths, delegate
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                return string.IsNullOrEmpty(baseDirectory) ? null : Path.Combine(baseDirectory, FileName);
            });

            AddCandidate(paths, delegate
            {
                return Path.Combine(Path.GetTempPath(), "Ranant", FileName);
            });

            return paths;
        }

        private static void AddCandidate(List<string> paths, Func<string> factory)
        {
            try
            {
                var path = factory();
                if (!string.IsNullOrEmpty(path) && !paths.Contains(path))
                {
                    paths.Add(path);
                }
            }
            catch
            {
                // 单个候选路径不可用就跳过。
            }
        }

        private static Dictionary<string, string> Parse(string[] lines)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var index = line.IndexOf('=');
                if (index <= 0)
                {
                    continue;
                }

                var key = line.Substring(0, index).Trim();
                var value = line.Substring(index + 1).Trim();
                if (key.Length > 0)
                {
                    values[key] = value;
                }
            }

            return values;
        }

        private static void Apply(Dictionary<string, string> values)
        {
            string raw;

            if (values.TryGetValue("LastTotalNumber", out raw))
            {
                int number;
                if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out number) && number >= 1 && number <= 65535)
                {
                    LastTotalNumber = number;
                }
            }

            if (values.TryGetValue("LastAlgorithm", out raw) && !string.IsNullOrEmpty(raw))
            {
                LastAlgorithm = raw;
            }

            if (values.TryGetValue("LastUpdateSource", out raw) && !string.IsNullOrEmpty(raw))
            {
                LastUpdateSource = raw;
            }

            if (values.TryGetValue("CustomUpdateSource", out raw))
            {
                CustomUpdateSource = raw;
            }

            if (values.TryGetValue("FluentEnabled", out raw))
            {
                bool enabled;
                if (bool.TryParse(raw, out enabled))
                {
                    FluentEnabled = enabled;
                }
            }

            if (values.TryGetValue("FluentThemeMode", out raw) && !string.IsNullOrEmpty(raw))
            {
                FluentThemeMode = raw;
            }
        }

        /// <summary>首次运行（还没有 ini）时，尝试把老版本写在 user.config 里的值迁过来，失败就忽略。</summary>
        private static void TryImportLegacy()
        {
            try
            {
                var legacy = ranant.Properties.Settings.Default;

                if (legacy.LastTotalNumber >= 1 && legacy.LastTotalNumber <= 65535)
                {
                    LastTotalNumber = legacy.LastTotalNumber;
                }

                if (!string.IsNullOrEmpty(legacy.LastAlgorithm))
                {
                    LastAlgorithm = legacy.LastAlgorithm;
                }

                if (!string.IsNullOrEmpty(legacy.LastUpdateSource))
                {
                    LastUpdateSource = legacy.LastUpdateSource;
                }

                if (legacy.CustomUpdateSource != null)
                {
                    CustomUpdateSource = legacy.CustomUpdateSource;
                }

                FluentEnabled = legacy.FluentEnabled;

                if (!string.IsNullOrEmpty(legacy.FluentThemeMode))
                {
                    FluentThemeMode = legacy.FluentThemeMode;
                }
            }
            catch (Exception ex)
            {
                // 老配置读不出来（例如 user.config 路径异常）时保持默认值。
                LastError = ex.Message;
            }
        }
    }
}
