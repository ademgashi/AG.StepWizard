using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AG.StepWizard
{
    internal sealed class ThemedWizardButton : Button
    {
        private bool isHovered;
        private bool isPressed;
        private StepWizardTheme theme = StepWizardTheme.Light;

        public ThemedWizardButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        public void SetTheme(StepWizardTheme value)
        {
            theme = value ?? StepWizardTheme.Light;
            Invalidate();
        }

        protected override void OnEnabledChanged(System.EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            isHovered = false;
            isPressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isPressed = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color backColor = Enabled ? theme.CardBack : theme.WindowBack;
            Color foreColor = Enabled ? theme.Text : theme.DisabledText;
            Color borderColor = theme.Border;

            if (Enabled && isPressed)
            {
                backColor = theme.SelectedBack;
            }
            else if (Enabled && isHovered)
            {
                backColor = theme.HoverBack;
            }

            Rectangle bounds = ClientRectangle;
            bounds.Width -= 1;
            bounds.Height -= 1;

            using (GraphicsPath path = RoundedRectangle(bounds, 6))
            using (SolidBrush backBrush = new SolidBrush(backColor))
            using (Pen borderPen = new Pen(borderColor))
            {
                e.Graphics.FillPath(backBrush, path);
                e.Graphics.DrawPath(borderPen, path);
            }

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, foreColor, flags);

            if (Focused && ShowFocusCues)
            {
                Rectangle focus = ClientRectangle;
                focus.Inflate(-4, -4);
                ControlPaint.DrawFocusRectangle(e.Graphics, focus, foreColor, backColor);
            }
        }

        private static GraphicsPath RoundedRectangle(Rectangle rectangle, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
