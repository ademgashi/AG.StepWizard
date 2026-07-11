using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AG.StepWizard
{
    internal enum ThemedWizardButtonRole
    {
        Secondary,
        Primary
    }

    internal sealed class ThemedWizardButton : Button
    {
        private bool isHovered;
        private bool isPressed;
        private StepWizardTheme theme = StepWizardTheme.Light;
        private ThemedWizardButtonRole role;

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

        public ThemedWizardButtonRole Role
        {
            get { return role; }
            set
            {
                role = value;
                Invalidate();
            }
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

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            bool primary = role == ThemedWizardButtonRole.Primary;
            Color backColor = Enabled ? (primary ? theme.Accent : theme.CardBack) : theme.WindowBack;
            Color foreColor = Enabled ? (primary ? theme.AccentText : theme.Text) : theme.DisabledText;
            Color borderColor = Enabled && primary ? theme.Accent : theme.Border;

            if (Enabled && isPressed)
            {
                backColor = primary ? ControlPaint.Dark(theme.Accent, 0.08F) : theme.SelectedBack;
            }
            else if (Enabled && isHovered)
            {
                backColor = primary ? ControlPaint.Light(theme.Accent, 0.08F) : theme.HoverBack;
            }

            Color parentBackColor = Parent == null ? theme.CardBack : Parent.BackColor;
            e.Graphics.Clear(parentBackColor);

            Rectangle bounds = new Rectangle(1, 1, Width - 3, Height - 3);

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
