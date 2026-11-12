using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ranant.Services
{
    /// <summary>更新检查所使用的下载源。</summary>
    public enum UpdateSource
    {
        GitHub = 0,
        GhProxy = 1,
        GhFast = 2,
        Custom = 3
    }

    /// <summary>一次检查更新得到的结果。</summary>
    public sealed class UpdateInfo
    {
        public Version Version { get; set; }
        public string TagName { get; set; }
        public string ReleaseName { get; set; }
        public string Notes { get; set; }
        public string ReleaseUrl { get; set; }
        public string AssetName { get; set; }
        public string AssetUrl { get; set; }
        public long AssetSize { get; set; }
        public UpdateSource Source { get; set; }
    }

    public sealed class UpdateException : Exception
    {
        public UpdateException(string message) : base(message) { }
    }

    /// <summary>
    /// 从 GitHub Releases / 镜像站获取最新版本。
    /// API 走 api.github.com，镜像站以“前缀 + 原始地址”的方式代理（如 https://gh-proxy.com/https://api.github.com/...）。
    /// </summary>
    public static class UpdateService
    {
        public const string Owner = "xinya28";
        public const string Repository = "ranant";

        public static readonly string ApiLatestUrl =
            "https://api.github.com/repos/" + Owner + "/" + Repository + "/releases/latest";

        public static readonly string ReleasesPageUrl =
            "https://github.com/" + Owner + "/" + Repository + "/releases";

        public static readonly string LatestPageUrl = ReleasesPageUrl + "/latest";

        public const string GhProxyPrefix = "https://gh-proxy.com/";
        public const string GhFastPrefix = "https://ghfast.top/";

        private static readonly HttpClient Http = CreateClient(true);
        private static readonly HttpClient HttpNoRedirect = CreateClient(false);

        static UpdateService()
        {
            try
            {
                // .NET Framework 默认可能不启用 TLS 1.2，GitHub 只接受 TLS 1.2+。
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            catch
            {
                // 忽略：老系统可能不支持该枚举值。
            }
        }

        /// <summary>当前程序集版本，例如 1.0.0.4。</summary>
        public static Version CurrentVersion
        {
            get
            {
                var entry = System.Reflection.Assembly.GetEntryAssembly();
                var asm = entry ?? typeof(UpdateService).Assembly;
                return asm.GetName().Version ?? new Version(1, 0, 0, 0);
            }
        }

        public static string CurrentVersionText
        {
            get { return CurrentVersion.ToString(); }
        }

        private static HttpClient CreateClient(bool allowRedirect)
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = allowRedirect,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(20);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Ranant-Updater/1.0 (+https://github.com/" + Owner + "/" + Repository + ")");
            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
            return client;
        }

        public static string Describe(UpdateSource source)
        {
            switch (source)
            {
                case UpdateSource.GhProxy: return "镜像 gh-proxy.com";
                case UpdateSource.GhFast: return "镜像 ghfast.top";
                case UpdateSource.Custom: return "自定义更新源";
                default: return "GitHub 官方";
            }
        }

        /// <summary>把设置里的字符串还原成枚举，非法值回落到 GitHub 官方。</summary>
        public static UpdateSource ParseSource(string value)
        {
            UpdateSource source;
            return Enum.TryParse(value, true, out source) ? source : UpdateSource.GitHub;
        }

        public static string SourceToSetting(UpdateSource source)
        {
            return source.ToString();
        }

        /// <summary>把“自定义更新源”整理成前缀；支持 {url} 占位模板。</summary>
        public static string NormalizeCustomPrefix(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return string.Empty;
            }

            var value = raw.Trim();
            if (value.IndexOf("{url}", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return value;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                value += "/";
            }

            return value;
        }

        public static string GetPrefix(UpdateSource source, string customPrefix)
        {
            switch (source)
            {
                case UpdateSource.GhProxy: return GhProxyPrefix;
                case UpdateSource.GhFast: return GhFastPrefix;
                case UpdateSource.Custom: return NormalizeCustomPrefix(customPrefix);
                default: return string.Empty;
            }
        }

        public static string ApplyPrefix(string prefix, string url)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                return url;
            }

            if (prefix.IndexOf("{url}", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return prefix.Replace("{url}", url);
            }

            return prefix + url;
        }

        /// <summary>按“用户选择 + 其余源兜底”的顺序排列尝试列表。</summary>
        private static List<UpdateSource> BuildAttemptOrder(UpdateSource selected)
        {
            var order = new List<UpdateSource>();
            order.Add(selected);

            var fallbacks = new[] { UpdateSource.GitHub, UpdateSource.GhProxy, UpdateSource.GhFast };
            foreach (var item in fallbacks)
            {
                if (!order.Contains(item))
                {
                    order.Add(item);
                }
            }

            return order;
        }

        /// <summary>
        /// 检查最新版本。会依次尝试所选源与其余镜像，返回第一个成功解析的结果。
        /// </summary>
        public static async Task<UpdateInfo> CheckAsync(UpdateSource source, string customPrefix, CancellationToken token)
        {
            var errors = new List<string>();

            foreach (var attempt in BuildAttemptOrder(source))
            {
                var prefix = GetPrefix(attempt, customPrefix);
                if (attempt == UpdateSource.Custom && prefix.Length == 0)
                {
                    continue;
                }

                var url = ApplyPrefix(prefix, ApiLatestUrl);
                try
                {
                    var json = await GetStringAsync(url, token).ConfigureAwait(false);
                    var info = ParseRelease(json);
                    if (info != null && info.Version != null)
                    {
                        info.Source = attempt;
                        return info;
                    }

                    errors.Add(Describe(attempt) + "：返回内容不是有效的 Release 数据");
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    errors.Add(Describe(attempt) + "：" + Flatten(ex));
                }
            }

            // API 不可用（例如被限流）时，退化为读取 releases/latest 的跳转地址拿版本号。
            foreach (var attempt in BuildAttemptOrder(source))
            {
                var prefix = GetPrefix(attempt, customPrefix);
                if (attempt == UpdateSource.Custom && prefix.Length == 0)
                {
                    continue;
                }

                try
                {
                    var info = await ProbeLatestRedirectAsync(attempt, prefix, token).ConfigureAwait(false);
                    if (info != null)
                    {
                        return info;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    // 兜底路径失败不覆盖原始错误信息。
                }
            }

            var detail = errors.Count == 0 ? "没有可用的更新源" : string.Join("；", errors.ToArray());
            throw new UpdateException("检查更新失败：" + detail);
        }

        /// <summary>通过 releases/latest 的 302 跳转解析版本号（不依赖 API）。</summary>
        private static async Task<UpdateInfo> ProbeLatestRedirectAsync(UpdateSource source, string prefix, CancellationToken token)
        {
            var url = ApplyPrefix(prefix, LatestPageUrl);
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await HttpNoRedirect.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
            {
                var location = response.Headers.Location;
                var status = (int)response.StatusCode;
                if (location == null || (status != 301 && status != 302 && status != 307 && status != 308))
                {
                    return null;
                }

                var target = location.IsAbsoluteUri ? location.ToString() : new Uri(new Uri(url), location).ToString();
                var slash = target.LastIndexOf("/tag/", StringComparison.OrdinalIgnoreCase);
                if (slash < 0)
                {
                    return null;
                }

                var tag = Uri.UnescapeDataString(target.Substring(slash + 5));
                var version = ParseVersion(tag);
                if (version == null)
                {
                    return null;
                }

                return new UpdateInfo
                {
                    Version = version,
                    TagName = tag,
                    ReleaseName = tag,
                    ReleaseUrl = target,
                    Source = source
                };
            }
        }

        /// <summary>从 tag（如 v1.0.0.3_26H1）里取出可比较的版本号。</summary>
        public static Version ParseVersion(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return null;
            }

            var match = Regex.Match(tag, @"(\d+(?:\.\d+){0,3})");
            if (!match.Success)
            {
                return null;
            }

            Version version;
            return Version.TryParse(match.Groups[1].Value, out version) ? version : null;
        }

        private static UpdateInfo ParseRelease(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 8 * 1024 * 1024;
            var root = serializer.DeserializeObject(json) as Dictionary<string, object>;
            if (root == null)
            {
                return null;
            }

            var info = new UpdateInfo
            {
                TagName = GetString(root, "tag_name"),
                ReleaseName = GetString(root, "name"),
                Notes = GetString(root, "body"),
                ReleaseUrl = GetString(root, "html_url")
            };

            info.Version = ParseVersion(info.TagName);
            if (info.Version == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(info.ReleaseUrl))
            {
                info.ReleaseUrl = ReleasesPageUrl;
            }

            var assets = root.ContainsKey("assets") ? root["assets"] as object[] : null;
            if (assets != null && assets.Length > 0)
            {
                Dictionary<string, object> chosen = null;
                foreach (var item in assets)
                {
                    var asset = item as Dictionary<string, object>;
                    if (asset == null)
                    {
                        continue;
                    }

                    if (chosen == null)
                    {
                        chosen = asset;
                    }

                    var name = GetString(asset, "name") ?? string.Empty;
                    if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        chosen = asset;
                        break;
                    }
                }

                if (chosen != null)
                {
                    info.AssetName = GetString(chosen, "name");
                    info.AssetUrl = GetString(chosen, "browser_download_url");
                    info.AssetSize = GetLong(chosen, "size");
                }
            }

            if (string.IsNullOrEmpty(info.AssetUrl))
            {
                var browser = GetString(root, "html_url");
                if (!string.IsNullOrEmpty(browser))
                {
                    info.ReleaseUrl = browser;
                }
            }

            return info;
        }

        private static string GetString(Dictionary<string, object> dict, string key)
        {
            object value;
            if (!dict.TryGetValue(key, out value) || value == null)
            {
                return null;
            }

            return value as string;
        }

        private static long GetLong(Dictionary<string, object> dict, string key)
        {
            object value;
            if (!dict.TryGetValue(key, out value) || value == null)
            {
                return 0;
            }

            long parsed;
            if (value is long)
            {
                return (long)value;
            }

            if (value is int)
            {
                return (int)value;
            }

            return long.TryParse(Convert.ToString(value), out parsed) ? parsed : 0;
        }

        private static async Task<string> GetStringAsync(string url, CancellationToken token)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await Http.SendAsync(request, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("HTTP " + (int)response.StatusCode + " " + response.ReasonPhrase);
                }

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        /// <summary>下载时使用的地址：若检查时走的是镜像，则下载同样走镜像加速。</summary>
        public static string GetDownloadUrl(UpdateInfo info, string customPrefix)
        {
            if (info == null || string.IsNullOrEmpty(info.AssetUrl))
            {
                return null;
            }

            var prefix = GetPrefix(info.Source, customPrefix);
            return ApplyPrefix(prefix, info.AssetUrl);
        }

        /// <summary>下载到指定路径，progress 汇报 0-100 的百分比。</summary>
        public static async Task DownloadAsync(string url, string destinationPath, IProgress<int> progress, CancellationToken token)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await Http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("HTTP " + (int)response.StatusCode + " " + response.ReasonPhrase);
                }

                var total = response.Content.Headers.ContentLength ?? -1L;
                using (var input = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                using (var output = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var buffer = new byte[81920];
                    long received = 0;
                    var lastPercent = -1;
                    int read;
                    while ((read = await input.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false)) > 0)
                    {
                        await output.WriteAsync(buffer, 0, read, token).ConfigureAwait(false);
                        received += read;

                        if (progress != null && total > 0)
                        {
                            var percent = (int)(received * 100L / total);
                            if (percent != lastPercent)
                            {
                                lastPercent = percent;
                                progress.Report(percent);
                            }
                        }
                    }
                }
            }
        }

        private static string Flatten(Exception ex)
        {
            var current = ex;
            while (current.InnerException != null)
            {
                current = current.InnerException;
            }

            var message = current.Message ?? current.GetType().Name;
            return message.Replace("\r", " ").Replace("\n", " ").Trim();
        }
    }
}
