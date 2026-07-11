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

    public enum StepWizardActionButtonState
    {
        Idle,
        Running,
        Success,
        Error,
        Warning
    }

    public class StepWizardActionButton : Button, IStepWizardThemeAware
    {
        private readonly Timer animationTimer;
        private bool isHovered;
        private bool isPressed;
        private int animationFrame;
        private StepWizardActionButtonState state;
        private StepWizardTheme theme = StepWizardTheme.Light;
        private string idleText = string.Empty;
        private string runningText = "Working...";
        private string successText = "Succeeded";
        private string errorText = "Failed";
        private string warningText = "Warning";
        private bool disableWhileRunning = true;

        public StepWizardActionButton()
        {
            animationTimer = new Timer { Interval = 100 };
            animationTimer.Tick += AnimationTimerTick;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(180, 36);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [DefaultValue(StepWizardActionButtonState.Idle)]
        [Category("Behavior")]
        public StepWizardActionButtonState State
        {
            get { return state; }
            set
            {
                if (state == value)
                {
                    return;
                }

                state = value;
                animationFrame = 0;
                animationTimer.Enabled = state == StepWizardActionButtonState.Running && IsHandleCreated;
                Invalidate();
            }
        }

        [DefaultValue("")]
        [Category("Appearance")]
        public string IdleText
        {
            get { return idleText; }
            set { idleText = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue("Working...")]
        [Category("Appearance")]
        public string RunningText
        {
            get { return runningText; }
            set { runningText = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue("Succeeded")]
        [Category("Appearance")]
        public string SuccessText
        {
            get { return successText; }
            set { successText = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue("Failed")]
        [Category("Appearance")]
        public string ErrorText
        {
            get { return errorText; }
            set { errorText = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue("Warning")]
        [Category("Appearance")]
        public string WarningText
        {
            get { return warningText; }
            set { warningText = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        public bool DisableWhileRunning
        {
            get { return disableWhileRunning; }
            set { disableWhileRunning = value; Invalidate(); }
        }

        public bool IsRunning
        {
            get { return state == StepWizardActionButtonState.Running; }
        }

        public void BeginOperation()
        {
            State = StepWizardActionButtonState.Running;
        }

        public void SetSuccess()
        {
            State = StepWizardActionButtonState.Success;
        }

        public void SetError()
        {
            State = StepWizardActionButtonState.Error;
        }

        public void SetWarning()
        {
            State = StepWizardActionButtonState.Warning;
        }

        public void ResetState()
        {
            State = StepWizardActionButtonState.Idle;
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer.Tick -= AnimationTimerTick;
                animationTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            animationTimer.Enabled = state == StepWizardActionButtonState.Running;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            animationTimer.Enabled = false;
            base.OnHandleDestroyed(e);
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
            if (state == StepWizardActionButtonState.Running && disableWhileRunning)
            {
                return;
            }

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

        protected override void OnClick(EventArgs e)
        {
            if (state == StepWizardActionButtonState.Running && disableWhileRunning)
            {
                return;
            }

            base.OnClick(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Parent == null ? theme.WindowBack : Parent.BackColor);

            bool effectiveEnabled = Enabled && !(state == StepWizardActionButtonState.Running && disableWhileRunning);
            Color stateColor = GetStateColor();
            Color back = effectiveEnabled ? theme.CardBack : theme.WindowBack;
            if (effectiveEnabled && isPressed)
            {
                back = theme.SelectedBack;
            }
            else if (effectiveEnabled && isHovered)
            {
                back = theme.HoverBack;
            }

            Rectangle bounds = new Rectangle(1, 1, Width - 3, Height - 3);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, 6))
            using (SolidBrush brush = new SolidBrush(back))
            using (Pen pen = new Pen(state == StepWizardActionButtonState.Idle ? theme.Border : stateColor))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }

            Rectangle glyphBounds = new Rectangle(ScaleValue(12), Math.Max(ScaleValue(8), (Height - ScaleValue(18)) / 2), ScaleValue(18), ScaleValue(18));
            PaintStateGlyph(e.Graphics, glyphBounds, stateColor, effectiveEnabled);

            Rectangle textBounds = new Rectangle(glyphBounds.Right + ScaleValue(8), 0, Width - glyphBounds.Right - ScaleValue(16), Height);
            TextRenderer.DrawText(e.Graphics, GetDisplayText(), Font, textBounds, Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void AnimationTimerTick(object sender, EventArgs e)
        {
            animationFrame = (animationFrame + 1) % 12;
            Invalidate();
        }

        private string GetDisplayText()
        {
            switch (state)
            {
                case StepWizardActionButtonState.Running:
                    return runningText;
                case StepWizardActionButtonState.Success:
                    return successText;
                case StepWizardActionButtonState.Error:
                    return errorText;
                case StepWizardActionButtonState.Warning:
                    return warningText;
                default:
                    return string.IsNullOrEmpty(idleText) ? Text : idleText;
            }
        }

        private Color GetStateColor()
        {
            switch (state)
            {
                case StepWizardActionButtonState.Running:
                    return theme.Accent;
                case StepWizardActionButtonState.Success:
                    return theme.Success;
                case StepWizardActionButtonState.Error:
                    return theme.Error;
                case StepWizardActionButtonState.Warning:
                    return theme.Warning;
                default:
                    return theme.MutedText;
            }
        }

        private void PaintStateGlyph(Graphics graphics, Rectangle bounds, Color color, bool effectiveEnabled)
        {
            Color glyphColor = effectiveEnabled || state != StepWizardActionButtonState.Idle ? color : theme.DisabledText;
            if (state == StepWizardActionButtonState.Running)
            {
                using (Pen basePen = new Pen(Color.FromArgb(60, glyphColor), 2.4F))
                using (Pen activePen = new Pen(glyphColor, 2.4F))
                {
                    graphics.DrawEllipse(basePen, bounds);
                    graphics.DrawArc(activePen, bounds, animationFrame * 30, 95);
                }
                return;
            }

            if (state == StepWizardActionButtonState.Success)
            {
                StepWizardPaint.DrawCheckMark(graphics, bounds, glyphColor, 2.3F);
                return;
            }

            if (state == StepWizardActionButtonState.Error)
            {
                float inset = bounds.Width * 0.27F;
                using (Pen pen = new Pen(glyphColor, 2.3F))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, bounds.Left + inset, bounds.Top + inset, bounds.Right - inset, bounds.Bottom - inset);
                    graphics.DrawLine(pen, bounds.Right - inset, bounds.Top + inset, bounds.Left + inset, bounds.Bottom - inset);
                }
                return;
            }

            if (state == StepWizardActionButtonState.Warning)
            {
                using (Pen pen = new Pen(glyphColor, 2.2F))
                using (SolidBrush brush = new SolidBrush(glyphColor))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    float centerX = bounds.Left + bounds.Width / 2F;
                    graphics.DrawLine(pen, centerX, bounds.Top + bounds.Height * 0.22F, centerX, bounds.Top + bounds.Height * 0.62F);
                    graphics.FillEllipse(brush, centerX - 1.6F, bounds.Bottom - bounds.Height * 0.22F, 3.2F, 3.2F);
                }
                return;
            }

            using (Pen pen = new Pen(glyphColor, 1.8F))
            {
                graphics.DrawEllipse(pen, bounds);
            }
        }

        private int ScaleValue(int value)
        {
            using (Graphics graphics = CreateGraphics())
            {
                return (int)Math.Round(value * graphics.DpiX / 96F);
            }
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
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Opaque, true);
            AutoSize = false;
            Size = new Size(160, 24);
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.ContentBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            StepWizardPaint.PaintCheckRadio(e.Graphics, ClientRectangle, Font, Text, Checked, Enabled, false, theme);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size textSize = TextRenderer.MeasureText(Text, Font);
            return new Size(textSize.Width + 28, Math.Max(22, textSize.Height + 4));
        }
    }

    public class StepWizardRadioButton : RadioButton, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardRadioButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Opaque, true);
            AutoSize = false;
            Size = new Size(180, 24);
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.ContentBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            StepWizardPaint.PaintCheckRadio(e.Graphics, ClientRectangle, Font, Text, Checked, Enabled, true, theme);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size textSize = TextRenderer.MeasureText(Text, Font);
            return new Size(textSize.Width + 28, Math.Max(22, textSize.Height + 4));
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
            FlatStyle = FlatStyle.Flat;
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
            if (e.Index < 0)
            {
                using (SolidBrush brush = new SolidBrush(theme.CardBack))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x000F)
            {
                using (Graphics graphics = CreateGraphics())
                using (Pen pen = new Pen(theme.Border))
                {
                    Rectangle border = new Rectangle(0, 0, Width - 1, Height - 1);
                    graphics.DrawRectangle(pen, border);
                }
            }
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

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
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

    public class StepWizardCard : Panel, IStepWizardThemeAware
    {
        private StepWizardTheme theme = StepWizardTheme.Light;

        public StepWizardCard()
        {
            Padding = new Padding(14);
            Size = new Size(280, 96);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        public virtual void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            ApplyThemeToChildren(this);
            Invalidate();
        }

        protected StepWizardTheme CurrentTheme
        {
            get { return theme; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, 8))
            using (SolidBrush brush = new SolidBrush(theme.CardBack))
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }
        }

        private void ApplyThemeToChildren(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                IStepWizardThemeAware aware = control as IStepWizardThemeAware;
                if (aware != null)
                {
                    aware.ApplyTheme(theme);
                }
                else
                {
                    control.BackColor = theme.CardBack;
                    control.ForeColor = control.Enabled ? theme.Text : theme.DisabledText;
                }

                ApplyThemeToChildren(control);
            }
        }
    }

    public class StepWizardOptionCard : Control, IStepWizardThemeAware
    {
        private bool selected;
        private bool hovered;
        private StepWizardTheme theme = StepWizardTheme.Light;
        private string subtitle = string.Empty;

        public StepWizardOptionCard()
        {
            Size = new Size(220, 78);
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [DefaultValue(false)]
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected == value)
                {
                    return;
                }

                selected = value;
                Invalidate();
            }
        }

        [DefaultValue("")]
        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value ?? string.Empty; Invalidate(); }
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
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
            Selected = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color back = selected ? theme.SelectedBack : hovered ? theme.HoverBack : theme.CardBack;
            Color border = selected ? theme.Accent : theme.Border;
            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, 8))
            using (SolidBrush brush = new SolidBrush(back))
            using (Pen pen = new Pen(border, selected ? 2F : 1F))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }

            Rectangle glyph = new Rectangle(14, 15, 22, 22);
            StepWizardPaint.PaintRadioGlyph(e.Graphics, glyph, selected, Enabled, theme);
            using (Font titleFont = new Font(Font, FontStyle.Bold))
            {
                TextRenderer.DrawText(e.Graphics, Text, titleFont, new Rectangle(46, 12, Width - 58, 24), Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
            }

            TextRenderer.DrawText(e.Graphics, subtitle, Font, new Rectangle(46, 36, Width - 58, 34), Enabled ? theme.MutedText : theme.DisabledText, TextFormatFlags.EndEllipsis | TextFormatFlags.WordBreak);
        }
    }

    public class StepWizardStatusCard : Control, IStepWizardThemeAware
    {
        private StepWizardTaskStatus status = StepWizardTaskStatus.Pending;
        private StepWizardTheme theme = StepWizardTheme.Light;
        private string subtitle = string.Empty;

        public StepWizardStatusCard()
        {
            Size = new Size(220, 72);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [DefaultValue(StepWizardTaskStatus.Pending)]
        public StepWizardTaskStatus Status
        {
            get { return status; }
            set { status = value; Invalidate(); }
        }

        [DefaultValue("")]
        public string Subtitle
        {
            get { return subtitle; }
            set { subtitle = value ?? string.Empty; Invalidate(); }
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
            using (GraphicsPath path = StepWizardPaint.RoundedRectangle(bounds, 8))
            using (SolidBrush brush = new SolidBrush(theme.CardBack))
            using (Pen pen = new Pen(theme.Border))
            {
                e.Graphics.FillPath(brush, path);
                e.Graphics.DrawPath(pen, path);
            }

            Rectangle badge = new Rectangle(14, 18, 24, 24);
            PaintBadge(e.Graphics, badge);
            using (Font titleFont = new Font(Font, FontStyle.Bold))
            {
                TextRenderer.DrawText(e.Graphics, Text, titleFont, new Rectangle(50, 12, Width - 62, 24), Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
            }

            TextRenderer.DrawText(e.Graphics, subtitle, Font, new Rectangle(50, 36, Width - 62, 26), Enabled ? theme.MutedText : theme.DisabledText, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
        }

        private void PaintBadge(Graphics graphics, Rectangle bounds)
        {
            Color color = GetStatusColor();
            using (SolidBrush brush = new SolidBrush(color))
            {
                graphics.FillEllipse(brush, bounds);
            }

            if (status == StepWizardTaskStatus.Completed)
            {
                StepWizardPaint.DrawCheckMark(graphics, bounds, theme.AccentText, 2.4F);
                return;
            }

            string marker = status == StepWizardTaskStatus.Error ? "x" : status == StepWizardTaskStatus.Warning ? "!" : status == StepWizardTaskStatus.Running ? "..." : string.Empty;
            if (!string.IsNullOrEmpty(marker))
            {
                TextRenderer.DrawText(graphics, marker, Font, bounds, theme.AccentText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private Color GetStatusColor()
        {
            switch (status)
            {
                case StepWizardTaskStatus.Completed:
                    return theme.Success;
                case StepWizardTaskStatus.Error:
                    return theme.Error;
                case StepWizardTaskStatus.Warning:
                    return theme.Warning;
                case StepWizardTaskStatus.Running:
                    return theme.Accent;
                default:
                    return theme.MutedText;
            }
        }
    }

    public enum StepWizardTaskStatus
    {
        Pending,
        Running,
        Completed,
        Error,
        Warning
    }

    public class StepWizardTaskItemControl : Control, IStepWizardThemeAware
    {
        private readonly Timer animationTimer;
        private StepWizardTheme theme = StepWizardTheme.Light;
        private string progressText = string.Empty;
        private string installText = "Check to Install";
        private bool installChecked;
        private bool showInstallCheck;
        private int animationFrame;
        private StepWizardTaskStatus status = StepWizardTaskStatus.Pending;

        public StepWizardTaskItemControl()
        {
            animationTimer = new Timer { Interval = 120 };
            animationTimer.Tick += (sender, e) =>
            {
                animationFrame = (animationFrame + 1) % 12;
                Invalidate();
            };
            Size = new Size(350, 76);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        public event EventHandler InstallCheckedChanged;

        [DefaultValue("")]
        public string Subtitle
        {
            get { return progressText; }
            set { ProgressText = value; }
        }

        [DefaultValue("")]
        public string ProgressText
        {
            get { return progressText; }
            set { progressText = value ?? string.Empty; Invalidate(); }
        }

        [DefaultValue(false)]
        public bool Completed
        {
            get { return status == StepWizardTaskStatus.Completed; }
            set { Status = value ? StepWizardTaskStatus.Completed : StepWizardTaskStatus.Pending; }
        }

        [DefaultValue(StepWizardTaskStatus.Pending)]
        public StepWizardTaskStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                animationFrame = 0;
                animationTimer.Enabled = status == StepWizardTaskStatus.Running && IsHandleCreated;
                Invalidate();
            }
        }

        [DefaultValue(false)]
        public bool ShowInstallCheck
        {
            get { return showInstallCheck; }
            set { showInstallCheck = value; Invalidate(); }
        }

        [DefaultValue(false)]
        public bool InstallChecked
        {
            get { return installChecked; }
            set
            {
                if (installChecked == value)
                {
                    return;
                }

                installChecked = value;
                OnInstallCheckedChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        [DefaultValue("Check to Install")]
        public string InstallText
        {
            get { return installText; }
            set { installText = value ?? string.Empty; Invalidate(); }
        }

        public void ApplyTheme(StepWizardTheme theme)
        {
            this.theme = theme ?? StepWizardTheme.Light;
            BackColor = this.theme.CardBack;
            ForeColor = Enabled ? this.theme.Text : this.theme.DisabledText;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            animationTimer.Enabled = status == StepWizardTaskStatus.Running;
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            animationTimer.Enabled = false;
            base.OnHandleDestroyed(e);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (showInstallCheck && GetInstallCheckBounds().Contains(PointToClient(MousePosition)))
            {
                InstallChecked = !installChecked;
            }
        }

        protected virtual void OnInstallCheckedChanged(EventArgs e)
        {
            EventHandler handler = InstallCheckedChanged;
            if (handler != null)
            {
                handler(this, e);
            }
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

            Rectangle statusBounds = new Rectangle(12, 10, 22, 22);
            PaintStatus(e.Graphics, statusBounds);

            using (Font titleFont = new Font(Font, FontStyle.Bold))
            {
                TextRenderer.DrawText(e.Graphics, Text, titleFont, new Rectangle(44, 6, Width - 58, 22), theme.Text, TextFormatFlags.EndEllipsis);
            }

            TextRenderer.DrawText(e.Graphics, progressText, Font, new Rectangle(44, 27, Width - 58, 18), theme.MutedText, TextFormatFlags.EndEllipsis);

            if (showInstallCheck)
            {
                Rectangle checkBounds = GetInstallCheckBounds();
                StepWizardPaint.PaintCheckBoxGlyph(e.Graphics, new Rectangle(checkBounds.Left, checkBounds.Top + 2, 14, 14), installChecked, Enabled, theme);
                TextRenderer.DrawText(e.Graphics, installText, Font, new Rectangle(checkBounds.Left + 22, checkBounds.Top, Width - checkBounds.Left - 28, 20), Enabled ? theme.Text : theme.DisabledText, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        private Rectangle GetInstallCheckBounds()
        {
            return new Rectangle(44, 46, Math.Max(120, Width - 56), 20);
        }

        private void PaintStatus(Graphics graphics, Rectangle bounds)
        {
            Color color = GetStatusColor();
            if (status == StepWizardTaskStatus.Running)
            {
                using (Pen basePen = new Pen(Color.FromArgb(60, color), 3F))
                using (Pen activePen = new Pen(color, 3F))
                {
                    graphics.DrawEllipse(basePen, bounds);
                    graphics.DrawArc(activePen, bounds, animationFrame * 30, 92);
                }
                return;
            }

            using (SolidBrush brush = new SolidBrush(color))
            {
                graphics.FillEllipse(brush, bounds);
            }

            if (status == StepWizardTaskStatus.Completed)
            {
                StepWizardPaint.DrawCheckMark(graphics, bounds, theme.AccentText, 2.4F);
                return;
            }

            string marker = status == StepWizardTaskStatus.Error ? "x" : status == StepWizardTaskStatus.Warning ? "!" : string.Empty;
            if (!string.IsNullOrEmpty(marker))
            {
                TextRenderer.DrawText(graphics, marker, Font, bounds, theme.AccentText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private Color GetStatusColor()
        {
            switch (status)
            {
                case StepWizardTaskStatus.Completed:
                    return theme.Success;
                case StepWizardTaskStatus.Error:
                    return theme.Error;
                case StepWizardTaskStatus.Warning:
                    return theme.Warning;
                case StepWizardTaskStatus.Running:
                    return theme.Accent;
                default:
                    return theme.MutedText;
            }
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
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color backColor = isChecked && enabled ? theme.Accent : enabled ? theme.CardBack : theme.WindowBack;
            Color borderColor = isChecked && enabled ? theme.Accent : theme.Border;

            using (GraphicsPath path = RoundedRectangle(bounds, 3))
            using (SolidBrush brush = new SolidBrush(backColor))
            using (Pen pen = new Pen(borderColor, 1.4F))
            {
                graphics.FillPath(brush, path);
                graphics.DrawPath(pen, path);
            }

            if (isChecked && enabled)
            {
                DrawCheckMark(graphics, bounds, theme.AccentText, 2.1F);
            }
            else if (isChecked)
            {
                DrawCheckMark(graphics, bounds, theme.DisabledText, 2.1F);
            }
        }

        public static void DrawCheckMark(Graphics graphics, Rectangle bounds, Color color, float width)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            PointF start = new PointF(bounds.Left + bounds.Width * 0.26F, bounds.Top + bounds.Height * 0.54F);
            PointF middle = new PointF(bounds.Left + bounds.Width * 0.43F, bounds.Top + bounds.Height * 0.70F);
            PointF end = new PointF(bounds.Left + bounds.Width * 0.76F, bounds.Top + bounds.Height * 0.32F);

            using (Pen pen = new Pen(color, width))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;
                graphics.DrawLines(pen, new[] { start, middle, end });
            }
        }

        public static void PaintRadioGlyph(Graphics graphics, Rectangle bounds, bool isChecked, bool enabled, StepWizardTheme theme)
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
