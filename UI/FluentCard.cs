using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ranant.UI
{
    /// <summary>带圆角与描边的卡片容器，用作设置页的分组面板。</summary>
    public class FluentCard : Panel
    {
        private int radius = 8;
        private Color borderColor = Color.FromArgb(0xE5, 0xE5, 0xE5);

        public FluentCard()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);
            BackColor = Color.White;
        }

        /// <summary>圆角半径，0 表示直角。</summary>
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value < 0 ? 0 : value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // 背景在 OnPaint 里统一绘制，避免直角底色盖住圆角。
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var parentBack = Parent != null ? Parent.BackColor : SystemColors.Control;
            g.Clear(parentBack);

            var bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CreateRoundedPath(bounds, Radius))
            using (var brush = new SolidBrush(BackColor))
            using (var pen = new Pen(borderColor))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }
        }

        /// <summary>生成圆角矩形路径。</summary>
        public static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                return path;
            }

            var limit = Math.Min(bounds.Width, bounds.Height) / 2;
            if (radius > limit)
            {
                radius = limit;
            }

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            var diameter = radius * 2;
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
