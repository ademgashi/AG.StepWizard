using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AG.StepWizard
{
    /// <summary>Implemented by controls that can consume a <see cref="StepWizardTheme"/> directly.</summary>
    public interface IStepWizardThemeAware
    {
        void ApplyTheme(StepWizardTheme theme);
    }

    public class StepWizardLabel : Label, IStepWizardThemeAware
    {
        public StepWizardLabel()
        {
            AutoSize = true;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            theme = theme ?? StepWizardTheme.Light;
            BackColor = Color.Transparent;
            ForeColor = Enabled ? theme.Text : theme.DisabledText;
        }
    }

    public class StepWizardButton : Button, IStepWizardThemeAware
    {
        private bool isHovered;
        private bool isPressed;
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
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
            e.Graphics.Clear(Parent == null ? theme.WindowBack : Parent.BackColor);

            Color back = Enabled ? theme.CardBack : theme.WindowBack;
            if (Enabled && isPressed)
            {
                back = theme.SelectedBack;
            }
            else if (Enabled && isHovered)
            {
                back = theme.HoverBack;
            }

            Rectangle bounds = new Rectangle(1, 1, Width - 3, Height - 3);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, 6))
            using (SolidBrush brush = new SolidBrush(back))
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }

            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    [DefaultProperty("Text")]
    public class StepWizardTextBox : UserControl, IStepWizardThemeAware
    {
        private readonly TextBox textBox;
        private bool hovered;
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardTextBox()
        {
            textBox = new TextBox { BorderStyle = BorderStyle.None, Location = new Point(8, 7), Width = Width - 16 };
            textBox.TextChanged += (sender, e) => OnTextChanged(e);
            textBox.GotFocus += (sender, e) => Invalidate();
            textBox.LostFocus += (sender, e) => Invalidate();
            Controls.Add(textBox);
            Size = new Size(180, 32);
            TabStop = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public char PasswordChar
        {
            get { return textBox.PasswordChar; }
            set { textBox.PasswordChar = value; }
        }

        public TextBox InnerTextBox
        {
            get { return textBox; }
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            textBox.BackColor = this.theme.CardBack;
            textBox.ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        public override bool Focused
        {
            get { return base.Focused || textBox.Focused; }
        }

        public new bool Focus()
        {
            return textBox.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            textBox.Location = new Point(8, Math.Max(5, (Height - textBox.Height) / 2));
            textBox.Width = Math.Max(10, Width - 16);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            hovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hovered = false;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            textBox.Focus();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            textBox.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color border = Focused ? theme.Accent : hovered ? theme.MutedText : theme.Border;
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), 6))
            using (Pen pen = new Pen(border, Focused ? 2F : 1F))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }
    }

    public class StepWizardCheckBox : CheckBox, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            AutoSize = true;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = Color.Transparent;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            StepWizardPaint.PaintCheckRadio(e.Graphics, ClientRectangle, Font, Text, Checked, Enabled, false, theme);
        }
    }

    public class StepWizardRadioButton : RadioButton, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardRadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            AutoSize = true;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = Color.Transparent;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            StepWizardPaint.PaintCheckRadio(e.Graphics, ClientRectangle, Font, Text, Checked, Enabled, true, theme);
        }
    }

    public class StepWizardGroupBox : GroupBox, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardGroupBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.ContentBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate(true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(BackColor);
            Size textSize = TextRenderer.MeasureText(Text, Font);
            Rectangle border = new Rectangle(0, textSize.Height / 2, Width - 1, Height - textSize.Height / 2 - 1);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(border, 6))
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.DrawPath(pen, path);
            }

            Rectangle textBounds = new Rectangle(10, 0, Math.Min(Width - 20, textSize.Width + 8), textSize.Height);
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(brush, textBounds);
            }
            TextRenderer.DrawText(e.Graphics, Text, Font, textBounds, Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }
    }

    public class StepWizardComboBox : ComboBox, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
            ItemHeight = 22;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0)
            {
                return;
            }

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color back = selected ? theme.SelectedBack : theme.CardBack;
            Color fore = Enabled ? theme.Text : theme.DisabledText;
            using (SolidBrush brush = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            TextRenderer.DrawText(e.Graphics, GetItemText(Items[e.Index]), Font, e.Bounds, fore, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    public class StepWizardCheckedListBox : CheckedListBox, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardCheckedListBox()
        {
            BorderStyle = BorderStyle.None;
            CheckOnClick = true;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 24;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush brush = new SolidBrush(selected ? theme.SelectedBack : theme.CardBack))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            Rectangle box = new Rectangle(e.Bounds.Left + 4, e.Bounds.Top + 4, 14, 14);
            StepWizardPaint.PaintCheckBoxGlyph(e.Graphics, box, GetItemChecked(e.Index), Enabled, theme);
            TextRenderer.DrawText(e.Graphics, GetItemText(Items[e.Index]), Font, new Rectangle(e.Bounds.Left + 24, e.Bounds.Top, e.Bounds.Width - 24, e.Bounds.Height), Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    public class StepWizardListView : ListView, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardListView()
        {
            BorderStyle = BorderStyle.None;
            FullRowSelect = true;
            OwnerDraw = true;
            View = View.Details;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(theme.HeaderBack))
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
                e.Graphics.DrawRectangle(pen, e.Bounds);
            }
            TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, theme.MutedText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            bool selected = e.Item.Selected;
            using (SolidBrush brush = new SolidBrush(selected ? theme.SelectedBack : theme.CardBack))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, Font, e.Bounds, Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    public class StepWizardProgressBar : Control, IStepWizardThemeAware
    {
        private int minimum;
        private int maximum = 100;
        private int value;
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardProgressBar()
        {
            Size = new Size(180, 18);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [DefaultValue(0)]
        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; if (this.value < minimum) this.value = minimum; Invalidate(); }
        }

        [DefaultValue(100)]
        public int Maximum
        {
            get { return maximum; }
            set { maximum = Math.Max(value, minimum + 1); if (this.value > maximum) this.value = maximum; Invalidate(); }
        }

        [DefaultValue(0)]
        public int Value
        {
            get { return value; }
            set { this.value = Math.Max(minimum, Math.Min(maximum, value)); Invalidate(); }
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, Height / 2))
            using (SolidBrush back = new SolidBrush(theme.HoverBack))
            using (Pen border = new Pen(theme.Border))
            {
                e.Graphics.FillPath(back, path);
                e.Graphics.DrawPath(border, path);
            }

            double percent = (double)(value - minimum) / (maximum - minimum);
            Rectangle fill = new Rectangle(1, 1, Math.Max(0, (int)((Width - 2) * percent)), Height - 2);
            if (fill.Width > 0)
            {
                using (GraphicsPath path = StepWizardPaint.RoundedRectangle(fill, Math.Max(1, fill.Height / 2)))
                using (SolidBrush brush = new SolidBrush(theme.Accent))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }
    }

    public class StepWizardTaskItemControl : Control, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;
        private string subtitle = string.Empty;
        private bool completed;

        public StepWizardTaskItemControl()
        {
            Size = new Size(260, 56);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [DefaultValue("")]
        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue(false)]
        public bool Completed
        {
            get { return completed; }
            set { completed = value; Invalidate(); }
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, 6))
            using (SolidBrush brush = new SolidBrush(theme.CardBack))
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }

            Rectangle circle = new Rectangle(12, 16, 24, 24);
            using (SolidBrush brush = new SolidBrush(completed ? theme.Success : theme.Accent))
            {
                e.Graphics.FillEllipse(brush, circle);
            }
            TextRenderer.DrawText(e.Graphics, completed ? "v" : "*", Font, circle, theme.AccentText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            using (Font titleFont = new Font(Font, FontStyle.Bold))
            {
                TextRenderer.DrawText(e.Graphics, Text, titleFont, new Rectangle(46, 9, Width - 58, 22), theme.Text, TextFormatFlags.EndEllipsis);
            }
            TextRenderer.DrawText(e.Graphics, subtitle, Font, new Rectangle(46, 30, Width - 58, 18), theme.MutedText, TextFormatFlags.EndEllipsis);
        }
    }

    public class StepWizardToolTip : ToolTip, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardToolTip()
        {
            OwnerDraw = true;
            Popup += StepWizardToolTipPopup;
            Draw += StepWizardToolTipDraw;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
        }

        private void StepWizardToolTipPopup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = TextRenderer.MeasureText(GetToolTip(e.AssociatedControl), SystemFonts.MessageBoxFont) + new Size(18, 12);
        }

        private void StepWizardToolTipDraw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.Clear(theme.CardBack);
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, e.Bounds.Width - 1, e.Bounds.Height - 1));
            }
            TextRenderer.DrawText(e.Graphics, e.ToolTipText, SystemFonts.MessageBoxFont, new Rectangle(9, 6, e.Bounds.Width - 18, e.Bounds.Height - 12), theme.Text, TextFormatFlags.WordBreak);
        }
    }

    internal static class StepWizardPaint
    {
        public static GraphicsPath RoundedRectangle(Rectangle rectangle, int radius)
        {
            int diameter = Math.Max(1, radius * 2);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rectangle.X, rectangle.Y, diameter, diameter, 180, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Y, diameter, diameter, 270, 90);
            path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rectangle.X, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void PaintCheckRadio(Graphics graphics, Rectangle bounds, Font font, string text, bool isChecked, bool enabled, bool radio, StepWizardTheme theme)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle glyph = new Rectangle(bounds.Left, bounds.Top + Math.Max(0, (bounds.Height - 16) / 2), 16, 16);
            if (radio)
            {
                PaintRadioGlyph(graphics, glyph, isChecked, enabled, theme);
            }
            else
            {
                PaintCheckBoxGlyph(graphics, glyph, isChecked, enabled, theme);
            }
            TextRenderer.DrawText(graphics, text, font, new Rectangle(bounds.Left + 22, bounds.Top, bounds.Width - 22, bounds.Height), enabled ? theme.Text : theme.DisabledText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        public static void PaintCheckBoxGlyph(Graphics graphics, Rectangle bounds, bool isChecked, bool enabled, StepWizardTheme theme)
        {
            using (SolidBrush brush = new SolidBrush(enabled ? theme.CardBack : theme.WindowBack))
            using (Pen pen = new Pen(isChecked ? theme.Accent : theme.Border, 1.5F))
            {
                graphics.FillRectangle(brush, bounds);
                graphics.DrawRectangle(pen, bounds);
            }
            if (isChecked)
            {
                TextRenderer.DrawText(graphics, "v", SystemFonts.MessageBoxFont, bounds, theme.Accent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private static void PaintRadioGlyph(Graphics graphics, Rectangle bounds, bool isChecked, bool enabled, StepWizardTheme theme)
        {
            using (SolidBrush brush = new SolidBrush(enabled ? theme.CardBack : theme.WindowBack))
            using (Pen pen = new Pen(isChecked ? theme.Accent : theme.Border, 1.5F))
            {
                graphics.FillEllipse(brush, bounds);
                graphics.DrawEllipse(pen, bounds);
            }
            if (isChecked)
            {
                Rectangle inner = bounds;
                inner.Inflate(-4, -4);
                using (SolidBrush brush = new SolidBrush(theme.Accent))
                {
                    graphics.FillEllipse(brush, inner);
                }
            }
        }
    }
}
