using System.Drawing.Drawing2D;

namespace Acr.WindowsForms.Controls.Class
{
    public static class AcrGraphics
    {
        public static GraphicsPath CreateRoundedRectPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
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
    
        /// <summary>
        /// Desenha a moldura "Outlined" usada pelos campos Acr (TextBox, NumericUpDown, DatePicker):
        /// fundo arredondado, borda e, com foco/erro, um anel suave em volta.
        /// </summary>
        public static void DrawFieldFrame(Graphics g, Rectangle bounds, Color fill, Color border, bool highlighted, Color accent, int radius, int ringWidth)
        {
            var oldMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var box = new Rectangle(bounds.X + ringWidth, bounds.Y + ringWidth, bounds.Width - ringWidth * 2 - 1, bounds.Height - ringWidth * 2 - 1);
            radius = Math.Min(radius, box.Height / 2);

            if (highlighted)
            {
                var ring = Rectangle.Inflate(box, ringWidth - 1, ringWidth - 1);
                using var ringPath = CreateRoundedRectPath(ring, radius + ringWidth - 1);
                using var ringBrush = new SolidBrush(Color.FromArgb(55, accent));
                g.FillPath(ringBrush, ringPath);
            }

            using (var path = CreateRoundedRectPath(box, radius))
            {
                using (var brush = new SolidBrush(fill))
                    g.FillPath(brush, path);
                using var pen = new Pen(border, highlighted ? 1.6f : 1f);
                g.DrawPath(pen, path);
            }

            g.SmoothingMode = oldMode;
        }
    }
}
