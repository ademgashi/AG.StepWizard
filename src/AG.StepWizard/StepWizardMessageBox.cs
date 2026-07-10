using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AG.StepWizard
{
    /// <summary>
    /// Displays a themed modal dialog with MessageBox-style buttons, icons, and return values.
    /// </summary>
    public static class StepWizardMessageBox
    {
        public static DialogResult Show(string text)
        {
            return Show(null, StepWizardTheme.Light, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(string text, string caption)
        {
            return Show(null, StepWizardTheme.Light, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show(null, StepWizardTheme.Light, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, StepWizardTheme.Light, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(null, StepWizardTheme.Light, text, caption, buttons, icon, defaultButton, 0);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return Show(null, StepWizardTheme.Light, text, caption, buttons, icon, defaultButton, options);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Show(owner, ResolveOwnerTheme(owner), text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return Show(owner, ResolveOwnerTheme(owner), text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(owner, ResolveOwnerTheme(owner), text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, ResolveOwnerTheme(owner), text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(owner, ResolveOwnerTheme(owner), text, caption, buttons, icon, defaultButton, 0);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return Show(owner, ResolveOwnerTheme(owner), text, caption, buttons, icon, defaultButton, options);
        }

        public static DialogResult Show(StepWizardTheme theme, string text)
        {
            return Show(null, theme, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(StepWizardTheme theme, string text, string caption)
        {
            return Show(null, theme, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(null, theme, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, theme, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(null, theme, text, caption, buttons, icon, defaultButton, 0);
        }

        public static DialogResult Show(StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return Show(null, theme, text, caption, buttons, icon, defaultButton, options);
        }

        public static DialogResult Show(IWin32Window owner, StepWizardTheme theme, string text)
        {
            return Show(owner, theme, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, StepWizardTheme theme, string text, string caption)
        {
            return Show(owner, theme, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(owner, theme, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, theme, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(IWin32Window owner, StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(owner, theme, text, caption, buttons, icon, defaultButton, 0);
        }

        public static DialogResult Show(IWin32Window owner, StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            using (ThemedMessageBoxForm form = new ThemedMessageBoxForm(theme ?? StepWizardTheme.Light, text, caption, buttons, icon, defaultButton, options))
            {
                return owner == null ? form.ShowDialog() : form.ShowDialog(owner);
            }
        }

        public static DialogResult Show(StepWizardControl owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, owner == null ? StepWizardTheme.Light : owner.Theme, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, 0);
        }

        public static DialogResult Show(StepWizardControl owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(owner, owner == null ? StepWizardTheme.Light : owner.Theme, text, caption, buttons, icon, defaultButton, 0);
        }

        public static DialogResult Show(StepWizardControl owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return Show(owner, owner == null ? StepWizardTheme.Light : owner.Theme, text, caption, buttons, icon, defaultButton, options);
        }

        private static StepWizardTheme ResolveOwnerTheme(IWin32Window owner)
        {
            Control control = owner as Control;
            while (control != null)
            {
                StepWizardControl wizard = control as StepWizardControl;
                if (wizard != null)
                {
                    return wizard.Theme;
                }

                control = control.Parent;
            }

            return StepWizardTheme.Light;
        }

        private sealed class ThemedMessageBoxForm : Form
        {
            private readonly StepWizardTheme theme;
            private readonly MessageBoxIcon icon;
            private readonly MessageBoxOptions options;
            private readonly FlowLayoutPanel buttonPanel;
            private readonly MessageContentView messageContent;

            public ThemedMessageBoxForm(StepWizardTheme theme, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
            {
                this.theme = theme;
                this.icon = icon;
                this.options = options;

                AutoScaleMode = AutoScaleMode.Dpi;
                BackColor = theme.WindowBack;
                ClientSize = new Size(480, 190);
                Font = new Font("Segoe UI", 9F);
                ForeColor = theme.Text;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowIcon = false;
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterParent;
                Text = caption ?? string.Empty;

                if ((options & MessageBoxOptions.RightAlign) == MessageBoxOptions.RightAlign)
                {
                    RightToLeft = RightToLeft.Yes;
                }

                if ((options & MessageBoxOptions.RtlReading) == MessageBoxOptions.RtlReading)
                {
                    RightToLeft = RightToLeft.Yes;
                    RightToLeftLayout = true;
                }

                Panel header = new Panel
                {
                    BackColor = theme.HeaderBack,
                    Dock = DockStyle.Top,
                    Height = ScaleValue(44)
                };

                Label titleLabel = new Label
                {
                    AutoEllipsis = true,
                    BackColor = Color.Transparent,
                    Dock = DockStyle.Fill,
                    Font = new Font(Font, FontStyle.Bold),
                    ForeColor = theme.Text,
                    Padding = new Padding(ScaleValue(16), 0, ScaleValue(16), 0),
                    Text = string.IsNullOrEmpty(caption) ? "AG.StepWizard" : caption,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                header.Controls.Add(titleLabel);

                Panel footer = new Panel
                {
                    BackColor = theme.WindowBack,
                    Dock = DockStyle.Bottom,
                    Height = ScaleValue(64),
                    Padding = new Padding(ScaleValue(16), ScaleValue(12), ScaleValue(16), ScaleValue(12))
                };

                buttonPanel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Dock = DockStyle.Right,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = false
                };
                footer.Controls.Add(buttonPanel);

                Panel body = new Panel
                {
                    BackColor = theme.ContentBack,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(ScaleValue(18), ScaleValue(18), ScaleValue(18), ScaleValue(18))
                };

                messageContent = new MessageContentView(theme, icon, text ?? string.Empty, options)
                {
                    Dock = DockStyle.Fill,
                    Font = Font
                };
                body.Controls.Add(messageContent);

                Controls.Add(body);
                Controls.Add(footer);
                Controls.Add(header);

                CreateButtons(buttons, defaultButton);
                LayoutMessage();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                using (Pen pen = new Pen(theme.Border))
                {
                    Rectangle border = ClientRectangle;
                    border.Width--;
                    border.Height--;
                    e.Graphics.DrawRectangle(pen, border);
                }
            }

            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);
                Button defaultButton = AcceptButton as Button;
                if (defaultButton != null)
                {
                    defaultButton.Focus();
                }
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                LayoutMessage();
            }

            private void CreateButtons(MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
            {
                List<ButtonSpec> specs = ButtonSpec.Get(buttons);
                for (int i = 0; i < specs.Count; i++)
                {
                    ThemedWizardButton button = new ThemedWizardButton
                    {
                        DialogResult = specs[i].Result,
                        Margin = new Padding(ScaleValue(4), 0, 0, 0),
                        Size = new Size(ScaleValue(92), ScaleValue(36)),
                        Text = specs[i].Text
                    };
                    button.SetTheme(theme);
                    buttonPanel.Controls.Add(button);

                    if (i == GetDefaultButtonIndex(defaultButton, specs.Count))
                    {
                        AcceptButton = button;
                    }

                    if (specs[i].Result == DialogResult.Cancel || specs[i].Result == DialogResult.No)
                    {
                        CancelButton = button;
                    }
                }

                if (AcceptButton == null && buttonPanel.Controls.Count > 0)
                {
                    AcceptButton = buttonPanel.Controls[0] as IButtonControl;
                }

                if (CancelButton == null)
                {
                    for (int i = buttonPanel.Controls.Count - 1; i >= 0; i--)
                    {
                        ThemedWizardButton button = buttonPanel.Controls[i] as ThemedWizardButton;
                        if (button != null)
                        {
                            CancelButton = button;
                            break;
                        }
                    }
                }
            }

            private int GetDefaultButtonIndex(MessageBoxDefaultButton defaultButton, int buttonCount)
            {
                int index;
                switch (defaultButton)
                {
                    case MessageBoxDefaultButton.Button2:
                        index = 1;
                        break;
                    case MessageBoxDefaultButton.Button3:
                        index = 2;
                        break;
                    default:
                        index = 0;
                        break;
                }

                return Math.Min(index, Math.Max(0, buttonCount - 1));
            }

            private void LayoutMessage()
            {
                if (messageContent == null)
                {
                    return;
                }

                int padding = ScaleValue(20);
                Size preferred = messageContent.GetPreferredContentSize(ScaleValue(460));
                int desiredWidth = Math.Max(ScaleValue(460), Math.Min(ScaleValue(820), preferred.Width + (padding * 2)));
                int availableContentWidth = desiredWidth - (padding * 2);
                preferred = messageContent.GetPreferredContentSize(availableContentWidth);
                int rowHeight = preferred.Height;
                int bodyHeight = Math.Max(ScaleValue(108), rowHeight + (padding * 2));
                Size desiredSize = new Size(desiredWidth, ScaleValue(44) + ScaleValue(64) + bodyHeight);
                if (ClientSize != desiredSize)
                {
                    ClientSize = desiredSize;
                }
                messageContent.Invalidate();
            }

            private int ScaleValue(int value)
            {
                return (int)Math.Round(value * DeviceDpi / 96.0);
            }
        }

        private sealed class MessageContentView : Control
        {
            private readonly StepWizardTheme theme;
            private readonly MessageBoxIcon icon;
            private readonly string message;
            private readonly MessageBoxOptions options;

            public MessageContentView(StepWizardTheme theme, MessageBoxIcon icon, string message, MessageBoxOptions options)
            {
                this.theme = theme;
                this.icon = icon;
                this.message = message ?? string.Empty;
                this.options = options;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            }

            public Size GetPreferredContentSize(int availableWidth)
            {
                int iconSize = HasVisibleIcon ? ScaleValue(42) : 0;
                int gap = iconSize > 0 ? ScaleValue(16) : 0;
                int messageWidth = Math.Max(ScaleValue(140), availableWidth - iconSize - gap);
                Size textSize = TextRenderer.MeasureText(message, Font, new Size(messageWidth, 0), TextFormatFlags.WordBreak);
                return new Size(iconSize + gap + textSize.Width, Math.Max(iconSize, textSize.Height));
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(theme.ContentBack);

                int padding = ScaleValue(20);
                int iconSize = HasVisibleIcon ? ScaleValue(42) : 0;
                int gap = iconSize > 0 ? ScaleValue(16) : 0;
                int messageWidth = Math.Max(ScaleValue(120), ClientSize.Width - (padding * 2) - iconSize - gap);
                Size textSize = TextRenderer.MeasureText(message, Font, new Size(messageWidth, 0), TextFormatFlags.WordBreak);
                int rowHeight = Math.Max(iconSize, textSize.Height);
                int rowTop = Math.Max(padding, (ClientSize.Height - rowHeight) / 2);
                int left = padding;

                if (HasVisibleIcon)
                {
                    Rectangle iconBounds = new Rectangle(left, rowTop + ((rowHeight - iconSize) / 2), iconSize, iconSize);
                    DrawIcon(e.Graphics, iconBounds);
                    left += iconSize + gap;
                }

                Rectangle textBounds = new Rectangle(left, rowTop + ((rowHeight - textSize.Height) / 2), messageWidth, Math.Max(textSize.Height, ScaleValue(24)));
                TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter;
                flags |= (options & MessageBoxOptions.RightAlign) == MessageBoxOptions.RightAlign ? TextFormatFlags.Right : TextFormatFlags.Left;
                TextRenderer.DrawText(e.Graphics, message, Font, textBounds, theme.Text, flags);
            }

            private bool HasVisibleIcon
            {
                get { return icon != MessageBoxIcon.None; }
            }

            private void DrawIcon(Graphics graphics, Rectangle bounds)
            {
                Color color = GetIconColor();
                Rectangle iconBounds = new Rectangle(bounds.Left + 2, bounds.Top + 2, bounds.Width - 5, bounds.Height - 5);
                switch (icon)
                {
                    case MessageBoxIcon.Hand:
                        DrawError(graphics, iconBounds, color);
                        break;
                    case MessageBoxIcon.Exclamation:
                        DrawWarning(graphics, iconBounds, color);
                        break;
                    case MessageBoxIcon.Question:
                        DrawQuestion(graphics, iconBounds, color);
                        break;
                    case MessageBoxIcon.Asterisk:
                        DrawInformation(graphics, iconBounds, color);
                        break;
                    default:
                        DrawInformation(graphics, iconBounds, color);
                        break;
                }
            }

            private int ScaleValue(int value)
            {
                using (Graphics graphics = CreateGraphics())
                {
                    return (int)Math.Round(value * graphics.DpiX / 96F);
                }
            }

            private Color GetIconColor()
            {
                switch (icon)
                {
                    case MessageBoxIcon.Hand:
                        return theme.Error;
                    case MessageBoxIcon.Exclamation:
                        return theme.Warning;
                    case MessageBoxIcon.Question:
                        return theme.Accent;
                    case MessageBoxIcon.Asterisk:
                        return theme.Accent;
                    default:
                        return theme.Accent;
                }
            }

            private void DrawInformation(Graphics graphics, Rectangle bounds, Color color)
            {
                DrawBadge(graphics, bounds, color, true);

                float centerX = bounds.Left + bounds.Width / 2F;
                float dotSize = Math.Max(3F, bounds.Width * 0.1F);
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(2F, bounds.Width * 0.08F)))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.FillEllipse(brush, centerX - dotSize / 2F, bounds.Top + bounds.Height * 0.25F, dotSize, dotSize);
                    graphics.DrawLine(pen, centerX, bounds.Top + bounds.Height * 0.43F, centerX, bounds.Bottom - bounds.Height * 0.22F);
                }
            }

            private void DrawQuestion(Graphics graphics, Rectangle bounds, Color color)
            {
                DrawBadge(graphics, bounds, color, true);

                float dotSize = Math.Max(3F, bounds.Width * 0.1F);
                Rectangle markBounds = new Rectangle(bounds.Left, bounds.Top + (int)(bounds.Height * 0.03F), bounds.Width, (int)(bounds.Height * 0.66F));
                using (Font font = new Font(Font.FontFamily, Math.Max(16F, bounds.Height * 0.56F), FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(color))
                using (StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    graphics.DrawString("?", font, brush, markBounds, format);
                    graphics.FillEllipse(brush, bounds.Left + (bounds.Width - dotSize) / 2F, bounds.Bottom - bounds.Height * 0.22F, dotSize, dotSize);
                }
            }

            private void DrawWarning(Graphics graphics, Rectangle bounds, Color color)
            {
                PointF top = new PointF(bounds.Left + bounds.Width / 2F, bounds.Top + bounds.Height * 0.09F);
                PointF left = new PointF(bounds.Left + bounds.Width * 0.09F, bounds.Bottom - bounds.Height * 0.1F);
                PointF right = new PointF(bounds.Right - bounds.Width * 0.09F, bounds.Bottom - bounds.Height * 0.1F);
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddPolygon(new[] { top, right, left });
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(34, color)))
                    using (Pen pen = new Pen(color, Math.Max(2F, bounds.Width * 0.05F)))
                    {
                        pen.LineJoin = LineJoin.Round;
                        graphics.FillPath(brush, path);
                        graphics.DrawPath(pen, path);
                    }
                }

                float centerX = bounds.Left + bounds.Width / 2F;
                float dotSize = Math.Max(3F, bounds.Width * 0.09F);
                using (SolidBrush brush = new SolidBrush(color))
                using (Pen pen = new Pen(color, Math.Max(2F, bounds.Width * 0.07F)))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, centerX, bounds.Top + bounds.Height * 0.35F, centerX, bounds.Top + bounds.Height * 0.62F);
                    graphics.FillEllipse(brush, centerX - dotSize / 2F, bounds.Bottom - bounds.Height * 0.25F, dotSize, dotSize);
                }
            }

            private void DrawError(Graphics graphics, Rectangle bounds, Color color)
            {
                DrawBadge(graphics, bounds, color, true);

                float inset = bounds.Width * 0.32F;
                using (Pen pen = new Pen(color, Math.Max(3F, bounds.Width * 0.08F)))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLine(pen, bounds.Left + inset, bounds.Top + inset, bounds.Right - inset, bounds.Bottom - inset);
                    graphics.DrawLine(pen, bounds.Right - inset, bounds.Top + inset, bounds.Left + inset, bounds.Bottom - inset);
                }
            }

            private void DrawBadge(Graphics graphics, Rectangle bounds, Color color, bool circular)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(28, color)))
                using (Pen pen = new Pen(color, Math.Max(2F, bounds.Width * 0.05F)))
                {
                    if (circular)
                    {
                        graphics.FillEllipse(brush, bounds);
                        graphics.DrawEllipse(pen, bounds);
                    }
                    else
                    {
                        graphics.FillRectangle(brush, bounds);
                        graphics.DrawRectangle(pen, bounds);
                    }
                }
            }
        }

        private sealed class ButtonSpec
        {
            private ButtonSpec(string text, DialogResult result)
            {
                Text = text;
                Result = result;
            }

            public string Text { get; private set; }
            public DialogResult Result { get; private set; }

            public static List<ButtonSpec> Get(MessageBoxButtons buttons)
            {
                switch (buttons)
                {
                    case MessageBoxButtons.OKCancel:
                        return new List<ButtonSpec> { new ButtonSpec("OK", DialogResult.OK), new ButtonSpec("Cancel", DialogResult.Cancel) };
                    case MessageBoxButtons.AbortRetryIgnore:
                        return new List<ButtonSpec> { new ButtonSpec("Abort", DialogResult.Abort), new ButtonSpec("Retry", DialogResult.Retry), new ButtonSpec("Ignore", DialogResult.Ignore) };
                    case MessageBoxButtons.YesNoCancel:
                        return new List<ButtonSpec> { new ButtonSpec("Yes", DialogResult.Yes), new ButtonSpec("No", DialogResult.No), new ButtonSpec("Cancel", DialogResult.Cancel) };
                    case MessageBoxButtons.YesNo:
                        return new List<ButtonSpec> { new ButtonSpec("Yes", DialogResult.Yes), new ButtonSpec("No", DialogResult.No) };
                    case MessageBoxButtons.RetryCancel:
                        return new List<ButtonSpec> { new ButtonSpec("Retry", DialogResult.Retry), new ButtonSpec("Cancel", DialogResult.Cancel) };
                    default:
                        return new List<ButtonSpec> { new ButtonSpec("OK", DialogResult.OK) };
                }
            }
        }
    }
}
