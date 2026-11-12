using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ranant.UI
{
    public enum FluentButtonKind
    {
        Secondary = 0,
        Primary = 1
    }

    /// <summary>Fluent 风格的圆角按钮：主题色实心（Primary）或描边（Secondary），带悬停/按下反馈。</summary>
    public class FluentButton : Button
    {
        private bool hovered;
        private bool pressed;
        private FluentButtonKind kind = FluentButtonKind.Secondary;

        public FluentButton()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            Cursor = Cursors.Hand;
            TabStop = true;
        }

        public FluentButtonKind Kind
        {
            get { return kind; }
            set
            {
                kind = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var parentBack = Parent != null ? Parent.BackColor : SystemColors.Control;
            g.Clear(parentBack);

            var fluent = FluentTheme.Enabled;
            Color fill;
            Color text;
            Color border;

            if (Kind == FluentButtonKind.Primary && fluent)
            {
                fill = FluentTheme.Accent;
                if (!Enabled)
                {
                    fill = FluentTheme.Blend(FluentTheme.Accent, FluentTheme.CardBack, 0.55F);
                }
                else if (pressed)
                {
                    fill = FluentTheme.Blend(fill, Color.Black, 0.18F);
                }
                else if (hovered)
                {
                    fill = FluentTheme.Blend(fill, Color.White, 0.16F);
                }

                text = Enabled ? Color.White : FluentTheme.TextSecondary;
                border = fill;
            }
            else if (fluent)
            {
                fill = FluentTheme.ControlBack;
                if (pressed)
                {
                    fill = FluentTheme.Blend(fill, FluentTheme.TextPrimary, 0.10F);
                }
                else if (hovered)
                {
                    fill = FluentTheme.Blend(fill, FluentTheme.Accent, 0.12F);
                }

                text = Enabled ? FluentTheme.TextPrimary : FluentTheme.TextSecondary;
                border = FluentTheme.ControlBorder;
            }
            else
            {
                fill = Enabled ? SystemColors.Control : SystemColors.ControlLight;
                if (pressed)
                {
                    fill = FluentTheme.Blend(fill, Color.Black, 0.06F);
                }
                else if (hovered)
                {
                    fill = FluentTheme.Blend(fill, Color.White, 0.35F);
                }

                text = Enabled ? SystemColors.ControlText : SystemColors.GrayText;
                border = SystemColors.ControlDark;
            }

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = FluentCard.CreateRoundedPath(bounds, 6))
            using (var brush = new SolidBrush(fill))
            using (var pen = new Pen(border))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            var textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, text, textFlags);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            hovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            hovered = false;
            pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                pressed = true;
                Invalidate();
            }

            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            pressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }
    }
}
