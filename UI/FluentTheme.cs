using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ranant.UI
{
    public enum FluentThemeMode
    {
        Auto = 0,
        Light = 1,
        Dark = 2
    }

    /// <summary>
    /// 可选的 Fluent Design 外观：圆角卡片、系统主题色、深浅色配色、Win11 窗口圆角与深色标题栏。
    /// 关闭时恢复成普通的 WinForms 配色。
    /// </summary>
    public static class FluentTheme
    {
        private const int DwmUseImmersiveDarkMode = 20;
        private const int DwmWindowCornerPreference = 33;
        private const int DwmBorderColor = 34;
        private const int DwmCaptionColor = 35;

        public static bool Enabled { get; private set; }
        public static FluentThemeMode Mode { get; private set; }
        public static bool IsDark { get; private set; }
        public static Color Accent { get; private set; }
        public static Color WindowBack { get; private set; }
        public static Color CardBack { get; private set; }
        public static Color CardBorder { get; private set; }
        public static Color TextPrimary { get; private set; }
        public static Color TextSecondary { get; private set; }
        public static Color ControlBack { get; private set; }
        public static Color ControlBorder { get; private set; }

        static FluentTheme()
        {
            Mode = FluentThemeMode.Auto;
            ApplyPalette(false, false);
        }

        public static FluentThemeMode ParseMode(string value)
        {
            FluentThemeMode mode;
            return Enum.TryParse(value, true, out mode) ? mode : FluentThemeMode.Auto;
        }

        /// <summary>把设置应用到整个窗体（包含所有子控件）与窗口边框。</summary>
        public static void Apply(Form form, bool enabled, FluentThemeMode mode)
        {
            Enabled = enabled;
            Mode = mode;
            IsDark = enabled && DetectDark(mode);
            Accent = DetectAccent();
            ApplyPalette(enabled, IsDark);

            if (form == null)
            {
                return;
            }

            ApplyTo(form);

            if (!form.IsHandleCreated)
            {
                // 句柄还没创建时先跳过 DWM 设置，等窗体的 Load/Shown 再调用一次即可。
                return;
            }

            ApplyWindowChrome(form, enabled);
        }

        /// <summary>递归套用当前配色。</summary>
        public static void ApplyTo(Control root)
        {
            if (root == null)
            {
                return;
            }

            ApplyControl(root, 0);
        }

        private static void ApplyControl(Control control, int depth)
        {
            if (control == null || depth > 16)
            {
                return;
            }

            var card = control as FluentCard;
            if (card != null)
            {
                card.BackColor = CardBack;
                card.BorderColor = CardBorder;
                card.Invalidate();
            }
            else if (control is Form)
            {
                control.BackColor = WindowBack;
                control.ForeColor = TextPrimary;
            }
            else if (control is UserControl)
            {
                control.BackColor = WindowBack;
                control.ForeColor = TextPrimary;
            }
            else if (control is Panel)
            {
                control.BackColor = control.Parent is FluentCard ? CardBack : WindowBack;
                control.ForeColor = TextPrimary;
            }
            else if (control is LinkLabel)
            {
                control.ForeColor = TextPrimary;
                ((LinkLabel)control).LinkColor = Accent;
                ((LinkLabel)control).ActiveLinkColor = Accent;
            }
            else if (control is Label)
            {
                var secondary = string.Equals(control.Tag as string, "secondary", StringComparison.OrdinalIgnoreCase);
                control.ForeColor = secondary ? TextSecondary : TextPrimary;
            }
            else if (control is CheckBox || control is RadioButton)
            {
                control.ForeColor = TextPrimary;
                control.BackColor = control.Parent is FluentCard ? CardBack : WindowBack;
            }
            else if (control is ComboBox || control is TextBox)
            {
                control.ForeColor = TextPrimary;
                control.BackColor = ControlBack;
            }
            else if (control is AntdUI.InputNumber)
            {
                // 数字输入框是 AntdUI 自绘的，只跟随配色改文字与底色。
                control.BackColor = ControlBack;
                control.ForeColor = TextPrimary;
                control.Invalidate();
            }
            else if (control is AntdUI.ButtonShadow)
            {
                // AntdUI 的按钮自带圆角和悬停效果，这里只跟随深浅色调整文字色。
                control.ForeColor = IsDark ? TextPrimary : Accent;
                control.Invalidate();
            }
            else if (control is Button)
            {
                var fluentButton = control as FluentButton;
                if (fluentButton == null)
                {
                    control.BackColor = ControlBack;
                    control.ForeColor = TextPrimary;
                    var button = (Button)control;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = ControlBorder;
                }

                control.Invalidate();
            }

            foreach (Control child in control.Controls)
            {
                ApplyControl(child, depth + 1);
            }
        }

        private static void ApplyWindowChrome(Form form, bool enabled)
        {
            // 开启 Fluent 时取消半透明（半透明窗体拿不到系统圆角与深色标题栏）。
            form.Opacity = enabled ? 1.0D : 0.9D;

            try
            {
                if (!enabled)
                {
                    SetDwmInt(form, DwmUseImmersiveDarkMode, 0);
                    SetDwmInt(form, DwmWindowCornerPreference, 0);
                    return;
                }

                SetDwmInt(form, DwmUseImmersiveDarkMode, IsDark ? 1 : 0);

                var build = Environment.OSVersion.Version.Build;
                if (build >= 22000)
                {
                    SetDwmInt(form, DwmWindowCornerPreference, 2); // 2 = 圆角
                    SetDwmInt(form, DwmCaptionColor, ColorTranslator.ToOle(IsDark ? Color.FromArgb(0x20, 0x20, 0x20) : Color.FromArgb(0xF3, 0xF3, 0xF3)));
                    SetDwmInt(form, DwmBorderColor, ColorTranslator.ToOle(IsDark ? Color.FromArgb(0x30, 0x30, 0x30) : Color.FromArgb(0xE5, 0xE5, 0xE5)));
                }
            }
            catch
            {
                // 非 Windows 10/11 或 dwmapi 不可用时静默忽略。
            }
        }

        private static void SetDwmInt(Form form, int attribute, int value)
        {
            try
            {
                DwmSetWindowAttribute(form.Handle, attribute, ref value, sizeof(int));
            }
            catch
            {
                // 忽略不受支持的属性。
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int value, int size);

        private static void ApplyPalette(bool enabled, bool dark)
        {
            if (!enabled)
            {
                WindowBack = SystemColors.Control;
                CardBack = Color.White;
                CardBorder = Color.FromArgb(200, 200, 200);
                TextPrimary = SystemColors.ControlText;
                TextSecondary = SystemColors.GrayText;
                ControlBack = Color.White;
                ControlBorder = Color.FromArgb(160, 160, 160);
                if (Accent.IsEmpty)
                {
                    Accent = Color.FromArgb(0, 103, 192);
                }

                return;
            }

            if (dark)
            {
                WindowBack = Color.FromArgb(0x20, 0x20, 0x20);
                CardBack = Color.FromArgb(0x2B, 0x2B, 0x2B);
                CardBorder = Color.FromArgb(0x3A, 0x3A, 0x3A);
                TextPrimary = Color.FromArgb(0xF2, 0xF2, 0xF2);
                TextSecondary = Color.FromArgb(0xB0, 0xB0, 0xB0);
                ControlBack = Color.FromArgb(0x33, 0x33, 0x33);
                ControlBorder = Color.FromArgb(0x4A, 0x4A, 0x4A);
            }
            else
            {
                WindowBack = Color.FromArgb(0xF3, 0xF3, 0xF3);
                CardBack = Color.White;
                CardBorder = Color.FromArgb(0xE5, 0xE5, 0xE5);
                TextPrimary = Color.FromArgb(0x1F, 0x1F, 0x1F);
                TextSecondary = Color.FromArgb(0x61, 0x61, 0x61);
                ControlBack = Color.White;
                ControlBorder = Color.FromArgb(0xD0, 0xD0, 0xD0);
            }

            if (Accent.IsEmpty)
            {
                Accent = Color.FromArgb(0, 103, 192);
            }
        }

        private static bool DetectDark(FluentThemeMode mode)
        {
            if (mode == FluentThemeMode.Dark)
            {
                return true;
            }

            if (mode == FluentThemeMode.Light)
            {
                return false;
            }

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("AppsUseLightTheme");
                        if (value is int)
                        {
                            return (int)value == 0;
                        }
                    }
                }
            }
            catch
            {
                // 读取失败时按浅色处理。
            }

            return false;
        }

        private static Color DetectAccent()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM"))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("ColorizationColor");
                        if (value is int)
                        {
                            var argb = (int)value;
                            var color = Color.FromArgb((argb >> 16) & 0xFF, (argb >> 8) & 0xFF, argb & 0xFF);
                            if (color.GetBrightness() > 0.12F)
                            {
                                return color;
                            }
                        }
                    }
                }
            }
            catch
            {
                // 读不到就用默认蓝。
            }

            return Color.FromArgb(0, 103, 192);
        }

        /// <summary>把颜色按比例混向目标色，用于 hover/pressed 状态。</summary>
        public static Color Blend(Color from, Color to, float amount)
        {
            if (amount < 0F)
            {
                amount = 0F;
            }

            if (amount > 1F)
            {
                amount = 1F;
            }

            return Color.FromArgb(
                (int)(from.R + (to.R - from.R) * amount),
                (int)(from.G + (to.G - from.G) * amount),
                (int)(from.B + (to.B - from.B) * amount));
        }
    }
}
