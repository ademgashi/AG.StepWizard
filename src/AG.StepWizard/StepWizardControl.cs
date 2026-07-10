using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AG.StepWizard
{
    /// <summary>
    /// A modern themed WinForms step wizard control.
    /// </summary>
    [DefaultEvent("SelectedPageChanged")]
    [DefaultProperty("Pages")]
    [Designer(typeof(StepWizardControlDesigner))]
    [ToolboxItem(true)]
    public class StepWizardControl : UserControl
    {
        private readonly StepWizardPageCollection pages;
        private readonly WizardHeader header;
        private readonly StepListView stepList;
        private readonly Panel pageHost;
        private readonly Panel footer;
        private readonly ThemedWizardButton backButton;
        private readonly ThemedWizardButton nextButton;
        private readonly ThemedWizardButton finishButton;
        private readonly ThemedWizardButton cancelButton;
        private StepWizardAppearance appearance = StepWizardAppearance.System;
        private StepWizardTheme theme;
        private bool customThemeAssigned;
        private bool useTheme = true;
        private int selectedPageIndex = -1;
        private int stepListWidth = 220;
        private string headerTitle = "Step Wizard";
        private string headerSubtitle = string.Empty;
        private bool showCancelButton = true;
        private bool showFinishButton = true;
        private bool themePageControls = true;

        public StepWizardControl()
        {
            pages = new StepWizardPageCollection(this);
            theme = ResolveTheme();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);

            header = new WizardHeader { Dock = DockStyle.Top };
            stepList = new StepListView(this) { Dock = DockStyle.Left };
            pageHost = new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty };
            footer = new Panel { Dock = DockStyle.Bottom };
            backButton = CreateButton("< Back", BackButtonClickCore);
            nextButton = CreateButton("Next >", NextButtonClickCore);
            finishButton = CreateButton("Finish", FinishButtonClickCore);
            cancelButton = CreateButton("Cancel", CancelButtonClickCore);

            Controls.Add(pageHost);
            Controls.Add(stepList);
            Controls.Add(footer);
            Controls.Add(header);

            footer.Controls.Add(cancelButton);
            footer.Controls.Add(finishButton);
            footer.Controls.Add(nextButton);
            footer.Controls.Add(backButton);

            Font = new Font("Segoe UI", 9F);
            Size = new Size(760, 520);
            ApplyLayoutMetrics();
            ApplyTheme();
            UpdateButtons();
        }

        /// <summary>Occurs before the selected page changes.</summary>
        [Category("Behavior")]
        public event EventHandler<StepWizardPageChangingEventArgs> SelectedPageChanging;

        /// <summary>Occurs after the selected page changes.</summary>
        [Category("Behavior")]
        public event EventHandler<StepWizardPageChangedEventArgs> SelectedPageChanged;

        /// <summary>Occurs when the Next button is clicked.</summary>
        [Category("Action")]
        public event EventHandler NextButtonClick;

        /// <summary>Occurs when the Back button is clicked.</summary>
        [Category("Action")]
        public event EventHandler BackButtonClick;

        /// <summary>Occurs when the Finish button is clicked.</summary>
        [Category("Action")]
        public event EventHandler FinishButtonClick;

        /// <summary>Occurs when the Cancel button is clicked.</summary>
        [Category("Action")]
        public event EventHandler CancelButtonClick;

        /// <summary>Occurs before Next or Finish navigation completes.</summary>
        [Category("Behavior")]
        public event EventHandler<StepWizardPageValidatingEventArgs> PageValidating;

        /// <summary>Gets the pages hosted by the wizard.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Behavior")]
        [Description("The pages hosted by the wizard.")]
        [MergableProperty(false)]
        public StepWizardPageCollection Pages
        {
            get { return pages; }
        }

        /// <summary>Gets or sets the selected page.</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StepWizardPage SelectedPage
        {
            get
            {
                return selectedPageIndex >= 0 && selectedPageIndex < Pages.Count ? Pages[selectedPageIndex] : null;
            }
            set
            {
                SelectedPageIndex = value == null ? -1 : Pages.IndexOf(value);
            }
        }

        /// <summary>Gets or sets the selected page index.</summary>
        [DefaultValue(-1)]
        [Category("Behavior")]
        public int SelectedPageIndex
        {
            get { return selectedPageIndex; }
            set { NavigateTo(value, false); }
        }

        /// <summary>Gets whether Back navigation is available.</summary>
        [Browsable(false)]
        public bool CanGoBack
        {
            get { return GetPreviousPageIndex(selectedPageIndex) >= 0; }
        }

        /// <summary>Gets whether Next navigation is available.</summary>
        [Browsable(false)]
        public bool CanGoNext
        {
            get { return GetNextPageIndex(selectedPageIndex) >= 0; }
        }

        /// <summary>Gets whether the selected page is the finish page.</summary>
        [Browsable(false)]
        public bool IsFinishPage
        {
            get
            {
                StepWizardPage page = SelectedPage;
                return page != null && (page.IsFinishPage || GetNextPageIndex(selectedPageIndex) < 0);
            }
        }

        /// <summary>Gets or sets whether the Cancel button is visible.</summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool ShowCancelButton
        {
            get { return showCancelButton; }
            set
            {
                showCancelButton = value;
                UpdateButtons();
            }
        }

        /// <summary>Gets or sets whether the Finish button may be shown.</summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool ShowFinishButton
        {
            get { return showFinishButton; }
            set
            {
                showFinishButton = value;
                UpdateButtons();
            }
        }

        /// <summary>Gets or sets the left step list width.</summary>
        [DefaultValue(220)]
        [Category("Layout")]
        public int StepListWidth
        {
            get { return stepListWidth; }
            set
            {
                stepListWidth = Math.Max(140, value);
                ApplyLayoutMetrics();
            }
        }

        /// <summary>Gets or sets the fallback header title.</summary>
        [DefaultValue("Step Wizard")]
        [Category("Appearance")]
        public string HeaderTitle
        {
            get { return headerTitle; }
            set
            {
                headerTitle = value ?? string.Empty;
                UpdateHeader();
            }
        }

        /// <summary>Gets or sets the fallback header subtitle.</summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public string HeaderSubtitle
        {
            get { return headerSubtitle; }
            set
            {
                headerSubtitle = value ?? string.Empty;
                UpdateHeader();
            }
        }

        /// <summary>Gets or sets whether themed rendering is enabled.</summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool UseTheme
        {
            get { return useTheme; }
            set
            {
                useTheme = value;
                ApplyTheme();
            }
        }

        /// <summary>Gets or sets a custom theme. Assigning a custom theme overrides the built-in appearance theme.</summary>
        [Category("Appearance")]
        [Description("Custom theme tokens. Assigning Appearance later switches back to the built-in theme pipeline.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public StepWizardTheme Theme
        {
            get { return theme; }
            set
            {
                theme = value ?? ResolveTheme();
                customThemeAssigned = value != null;
                ApplyTheme();
            }
        }

        /// <summary>Gets or sets the built-in appearance mode.</summary>
        [DefaultValue(StepWizardAppearance.System)]
        [Category("Appearance")]
        public StepWizardAppearance Appearance
        {
            get { return appearance; }
            set
            {
                appearance = value;
                customThemeAssigned = false;
                theme = ResolveTheme();
                ApplyTheme();
            }
        }

        /// <summary>
        /// Gets or sets whether controls hosted inside wizard pages inherit the active theme colors.
        /// </summary>
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ThemePageControls
        {
            get { return themePageControls; }
            set
            {
                themePageControls = value;
                ApplyTheme();
            }
        }

        /// <summary>Moves to the previous page.</summary>
        public bool GoBack()
        {
            int previousIndex = GetPreviousPageIndex(selectedPageIndex);
            return previousIndex >= 0 && NavigateTo(previousIndex, false);
        }

        /// <summary>Moves to the next page after validation.</summary>
        public bool GoNext()
        {
            if (!CanGoNext || !ValidateSelectedPage())
            {
                return false;
            }

            int nextIndex = GetNextPageIndex(selectedPageIndex);
            return nextIndex >= 0 && NavigateTo(nextIndex, false);
        }

        internal void AttachPage(StepWizardPage page)
        {
            if (page == null)
            {
                return;
            }

            page.Visible = false;
            page.Dock = DockStyle.Fill;
            page.Owner = this;
            pageHost.Controls.Add(page);
            RegisterThemePropagation(page);
            ApplyThemeToPage(page, CurrentTheme);

            if (selectedPageIndex == -1)
            {
                NavigateTo(GetInitialPageIndex(), true);
            }
            else
            {
                UpdateButtons();
                stepList.Invalidate();
            }
        }

        internal Button DesignBackButton
        {
            get { return backButton; }
        }

        internal Button DesignNextButton
        {
            get { return nextButton; }
        }

        internal Button DesignFinishButton
        {
            get { return finishButton; }
        }

        internal Button DesignCancelButton
        {
            get { return cancelButton; }
        }

        internal void DetachPage(StepWizardPage page)
        {
            if (page != null)
            {
                UnregisterThemePropagation(page);
                page.Owner = null;
                pageHost.Controls.Remove(page);
            }

            if (Pages.Count == 0)
            {
                selectedPageIndex = -1;
            }
            else if (selectedPageIndex >= Pages.Count)
            {
                selectedPageIndex = GetLastPageIndex();
            }
            else if (!IsInDesignMode && IsPageSuppressed(selectedPageIndex))
            {
                selectedPageIndex = GetNearestVisiblePageIndex(selectedPageIndex);
            }

            ShowSelectedPage();
            UpdateButtons();
            stepList.Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ApplyLayoutMetrics();
            header.Invalidate();
            stepList.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            StepWizardTheme activeTheme = CurrentTheme;
            using (Pen borderPen = new Pen(activeTheme.Border))
            {
                Rectangle border = ClientRectangle;
                border.Width -= 1;
                border.Height -= 1;
                e.Graphics.DrawRectangle(borderPen, border);
            }
        }

        private bool IsInDesignMode
        {
            get { return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime || (Site != null && Site.DesignMode); }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            if (!customThemeAssigned && appearance == StepWizardAppearance.System)
            {
                theme = ResolveTheme();
                ApplyTheme();
            }
        }

        private StepWizardTheme CurrentTheme
        {
            get { return useTheme ? theme : StepWizardTheme.Light; }
        }

        private int GetInitialPageIndex()
        {
            return IsInDesignMode ? 0 : GetNextPageIndex(-1);
        }

        private int GetLastPageIndex()
        {
            return IsInDesignMode ? Pages.Count - 1 : GetPreviousPageIndex(Pages.Count);
        }

        private int GetNearestVisiblePageIndex(int index)
        {
            int nextIndex = GetNextPageIndex(index);
            if (nextIndex >= 0)
            {
                return nextIndex;
            }

            return GetPreviousPageIndex(index);
        }

        private int GetPreviousPageIndex(int index)
        {
            for (int i = Math.Min(index - 1, Pages.Count - 1); i >= 0; i--)
            {
                if (!IsPageSuppressed(i))
                {
                    return i;
                }
            }

            return -1;
        }

        private int GetNextPageIndex(int index)
        {
            for (int i = Math.Max(index + 1, 0); i < Pages.Count; i++)
            {
                if (!IsPageSuppressed(i))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IsPageSuppressed(int index)
        {
            return index >= 0 && index < Pages.Count && Pages[index].Suppress;
        }

        private ThemedWizardButton CreateButton(string text, EventHandler clickHandler)
        {
            ThemedWizardButton button = new ThemedWizardButton
            {
                Text = text,
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                TabStop = true
            };
            button.Click += clickHandler;
            return button;
        }

        private bool NavigateTo(int index, bool force)
        {
            if (index < -1 || index >= Pages.Count || (!force && index == selectedPageIndex))
            {
                return false;
            }

            if (!IsInDesignMode && IsPageSuppressed(index))
            {
                index = GetNearestVisiblePageIndex(index);

                if (index < 0)
                {
                    return false;
                }
            }

            StepWizardPage currentPage = SelectedPage;
            StepWizardPage nextPage = index >= 0 ? Pages[index] : null;
            StepWizardPageChangingEventArgs changing = new StepWizardPageChangingEventArgs(currentPage, nextPage, selectedPageIndex, index);
            OnSelectedPageChanging(changing);
            if (changing.Cancel)
            {
                return false;
            }

            selectedPageIndex = index;
            ShowSelectedPage();
            UpdateHeader();
            UpdateButtons();
            stepList.Invalidate();
            OnSelectedPageChanged(new StepWizardPageChangedEventArgs(SelectedPage, selectedPageIndex));
            return true;
        }

        internal void OnPageFlowChanged(StepWizardPage page)
        {
            if (page == null || !Pages.Contains(page))
            {
                return;
            }

            if (!IsInDesignMode && page.Suppress && Pages.IndexOf(page) == selectedPageIndex)
            {
                int nextIndex = GetNextPageIndex(selectedPageIndex);
                NavigateTo(nextIndex >= 0 ? nextIndex : GetPreviousPageIndex(selectedPageIndex), true);
            }
            else
            {
                UpdateHeader();
                UpdateButtons();
                stepList.Invalidate();
            }
        }

        private void ShowSelectedPage()
        {
            for (int i = 0; i < pageHost.Controls.Count; i++)
            {
                pageHost.Controls[i].Visible = false;
            }

            StepWizardPage selected = SelectedPage;
            if (selected != null)
            {
                selected.Visible = true;
                selected.BringToFront();
                selected.Focus();
            }
        }

        private bool ValidateSelectedPage()
        {
            StepWizardPage page = SelectedPage;
            if (page == null)
            {
                return true;
            }

            StepWizardPageValidatingEventArgs args = new StepWizardPageValidatingEventArgs(page, selectedPageIndex);
            OnPageValidating(args);
            return !args.Cancel;
        }

        private void ApplyLayoutMetrics()
        {
            int scale = ScaleValue(1);
            Padding outerPadding = new Padding(ScaleValue(1));
            Padding footerPadding = new Padding(ScaleValue(16), ScaleValue(10), ScaleValue(16), ScaleValue(10));
            int headerHeight = ScaleValue(92);
            int footerHeight = ScaleValue(64);
            int buttonWidth = ScaleValue(92);
            int buttonHeight = ScaleValue(36);
            int gap = ScaleValue(8);
            int right = footerPadding.Right;

            Padding = outerPadding;
            header.Height = headerHeight;
            footer.Height = footerHeight;
            footer.Padding = footerPadding;
            stepList.Width = stepListWidth;
            stepList.Padding = new Padding(ScaleValue(14), ScaleValue(18), ScaleValue(12), ScaleValue(18));

            ThemedWizardButton[] buttons = { cancelButton, finishButton, nextButton, backButton };
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Size = new Size(buttonWidth, buttonHeight);
                buttons[i].Location = new Point(footer.Width - right - ((i + 1) * buttonWidth) - (i * gap), footerPadding.Top);
            }

            footer.Resize -= FooterResize;
            footer.Resize += FooterResize;
            FooterResize(this, EventArgs.Empty);
            pageHost.Padding = new Padding(0, 0, scale, 0);
        }

        private void FooterResize(object sender, EventArgs e)
        {
            int buttonWidth = ScaleValue(92);
            int gap = ScaleValue(8);
            int top = Math.Max(ScaleValue(8), (footer.Height - ScaleValue(36)) / 2);
            int right = ScaleValue(16);
            ThemedWizardButton[] buttons = { cancelButton, finishButton, nextButton, backButton };
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Location = new Point(footer.Width - right - ((i + 1) * buttonWidth) - (i * gap), top);
            }
        }

        private int ScaleValue(int value)
        {
            using (Graphics graphics = CreateGraphics())
            {
                return (int)Math.Round(value * graphics.DpiX / 96F);
            }
        }

        private void ApplyTheme()
        {
            if (!customThemeAssigned)
            {
                theme = ResolveTheme();
            }

            StepWizardTheme activeTheme = CurrentTheme;
            BackColor = activeTheme.WindowBack;
            ForeColor = activeTheme.Text;
            header.SetTheme(activeTheme);
            header.Title = ResolveHeaderTitle();
            header.Subtitle = ResolveHeaderSubtitle();
            stepList.SetTheme(activeTheme);
            pageHost.BackColor = activeTheme.ContentBack;
            footer.BackColor = activeTheme.CardBack;
            foreach (StepWizardPage page in Pages)
            {
                ApplyThemeToPage(page, activeTheme);
            }

            backButton.SetTheme(activeTheme);
            nextButton.SetTheme(activeTheme);
            finishButton.SetTheme(activeTheme);
            cancelButton.SetTheme(activeTheme);
            Invalidate(true);
        }

        private void ApplyThemeToPage(StepWizardPage page, StepWizardTheme activeTheme)
        {
            if (page == null)
            {
                return;
            }

            page.BackColor = activeTheme.ContentBack;
            page.ForeColor = activeTheme.Text;

            if (themePageControls)
            {
                ApplyThemeToChildren(page, activeTheme);
            }

            page.Invalidate(true);
        }

        private void ApplyThemeToChildren(Control parent, StepWizardTheme activeTheme)
        {
            foreach (Control child in parent.Controls)
            {
                ApplyThemeToControl(child, activeTheme);
                ApplyThemeToChildren(child, activeTheme);
            }
        }

        private void ApplyThemeToControl(Control control, StepWizardTheme activeTheme)
        {
            if (control == null)
            {
                return;
            }

            IStepWizardThemeAware themeAware = control as IStepWizardThemeAware;
            if (themeAware != null)
            {
                themeAware.ApplyTheme(activeTheme);
                return;
            }

            control.ForeColor = control.Enabled ? activeTheme.Text : activeTheme.DisabledText;

            if (control is TextBoxBase || control is ListBox || control is ComboBox || control is ListView || control is TreeView)
            {
                control.BackColor = activeTheme.CardBack;
                return;
            }

            if (control is Button)
            {
                Button button = (Button)control;
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = activeTheme.CardBack;
                button.ForeColor = button.Enabled ? activeTheme.Text : activeTheme.DisabledText;
                button.FlatAppearance.BorderColor = activeTheme.Border;
                button.FlatAppearance.MouseOverBackColor = activeTheme.HoverBack;
                button.FlatAppearance.MouseDownBackColor = activeTheme.SelectedBack;
                return;
            }

            if (control is LinkLabel)
            {
                LinkLabel linkLabel = (LinkLabel)control;
                linkLabel.BackColor = activeTheme.ContentBack;
                linkLabel.LinkColor = activeTheme.Accent;
                linkLabel.ActiveLinkColor = activeTheme.Warning;
                linkLabel.VisitedLinkColor = activeTheme.MutedText;
                return;
            }

            if (control is Label || control is CheckBox || control is RadioButton)
            {
                control.BackColor = Color.Transparent;
                return;
            }

            if (control is GroupBox)
            {
                control.BackColor = activeTheme.ContentBack;
                return;
            }

            if (control is Panel || control is TableLayoutPanel || control is FlowLayoutPanel || control is TabControl || control is TabPage)
            {
                control.BackColor = activeTheme.ContentBack;
                return;
            }

            control.BackColor = activeTheme.ContentBack;
        }

        private void RegisterThemePropagation(Control control)
        {
            if (control == null)
            {
                return;
            }

            control.ControlAdded -= PageControlAdded;
            control.ControlAdded += PageControlAdded;
            control.ControlRemoved -= PageControlRemoved;
            control.ControlRemoved += PageControlRemoved;

            foreach (Control child in control.Controls)
            {
                RegisterThemePropagation(child);
            }
        }

        private void UnregisterThemePropagation(Control control)
        {
            if (control == null)
            {
                return;
            }

            control.ControlAdded -= PageControlAdded;
            control.ControlRemoved -= PageControlRemoved;

            foreach (Control child in control.Controls)
            {
                UnregisterThemePropagation(child);
            }
        }

        private void PageControlAdded(object sender, ControlEventArgs e)
        {
            RegisterThemePropagation(e.Control);

            if (themePageControls)
            {
                ApplyThemeToControl(e.Control, CurrentTheme);
                ApplyThemeToChildren(e.Control, CurrentTheme);
            }
        }

        private void PageControlRemoved(object sender, ControlEventArgs e)
        {
            UnregisterThemePropagation(e.Control);
        }

        internal void DesignAddPage(bool insertBeforeCurrent)
        {
            StepWizardPage page = CreateDesignPage();
            string propertyName = "Pages";
            RaiseDesignChanging(propertyName);

            if (insertBeforeCurrent && selectedPageIndex >= 0 && selectedPageIndex < Pages.Count)
            {
                Pages.Insert(selectedPageIndex, page);
            }
            else
            {
                Pages.Add(page);
                selectedPageIndex = Pages.Count - 1;
            }

            SelectedPage = page;
            RaiseDesignChanged(propertyName);
        }

        private StepWizardPage CreateDesignPage()
        {
            IDesignerHost host = Site == null ? null : Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
            StepWizardPage page = host == null ? new StepWizardPage() : host.CreateComponent(typeof(StepWizardPage)) as StepWizardPage;
            if (page == null)
            {
                page = new StepWizardPage();
            }

            page.Title = "Step " + (Pages.Count + 1);
            page.Subtitle = "Describe this step.";
            return page;
        }

        internal void DesignRemoveSelectedPage()
        {
            StepWizardPage page = SelectedPage;
            if (page == null)
            {
                return;
            }

            string propertyName = "Pages";
            RaiseDesignChanging(propertyName);
            Pages.Remove(page);

            IDesignerHost host = Site == null ? null : Site.GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (host != null && page.Site != null)
            {
                host.DestroyComponent(page);
            }
            else
            {
                page.Dispose();
            }

            RaiseDesignChanged(propertyName);
        }

        internal void DesignFirstPage()
        {
            if (Pages.Count > 0)
            {
                SetDesignSelectedPageIndex(0);
            }
        }

        internal void DesignPreviousPage()
        {
            if (selectedPageIndex > 0)
            {
                SetDesignSelectedPageIndex(selectedPageIndex - 1);
            }
        }

        internal void DesignNextPage()
        {
            if (selectedPageIndex >= 0 && selectedPageIndex < Pages.Count - 1)
            {
                SetDesignSelectedPageIndex(selectedPageIndex + 1);
            }
        }

        internal void DesignLastPage()
        {
            if (Pages.Count > 0)
            {
                SetDesignSelectedPageIndex(Pages.Count - 1);
            }
        }

        internal void SetDesignSelectedPageIndex(int index)
        {
            RaiseDesignChanging("SelectedPageIndex");
            SelectedPageIndex = index;
            RaiseDesignChanged("SelectedPageIndex");
        }

        private void RaiseDesignChanging(string propertyName)
        {
            IComponentChangeService service = Site == null ? null : Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (service != null)
            {
                service.OnComponentChanging(this, TypeDescriptor.GetProperties(this)[propertyName]);
            }
        }

        private void RaiseDesignChanged(string propertyName)
        {
            IComponentChangeService service = Site == null ? null : Site.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (service != null)
            {
                service.OnComponentChanged(this, TypeDescriptor.GetProperties(this)[propertyName], null, null);
            }
        }

        private StepWizardTheme ResolveTheme()
        {
            StepWizardAppearance resolved = appearance;
            if (resolved == StepWizardAppearance.System)
            {
                if (SystemInformation.HighContrast)
                {
                    resolved = StepWizardAppearance.HighContrast;
                }
                else
                {
                    resolved = IsWindowsAppsDarkMode() ? StepWizardAppearance.Dark : StepWizardAppearance.Light;
                }
            }

            switch (resolved)
            {
                case StepWizardAppearance.Dark:
                    return StepWizardTheme.Dark;
                case StepWizardAppearance.OLEDBlack:
                    return StepWizardTheme.OLEDBlack;
                case StepWizardAppearance.BlueDark:
                    return StepWizardTheme.BlueDark;
                case StepWizardAppearance.CatppuccinLatte:
                    return StepWizardTheme.CatppuccinLatte;
                case StepWizardAppearance.CatppuccinFrappe:
                    return StepWizardTheme.CatppuccinFrappe;
                case StepWizardAppearance.CatppuccinMacchiato:
                    return StepWizardTheme.CatppuccinMacchiato;
                case StepWizardAppearance.CatppuccinMocha:
                    return StepWizardTheme.CatppuccinMocha;
                case StepWizardAppearance.Monokai:
                    return StepWizardTheme.Monokai;
                case StepWizardAppearance.SolarizedLight:
                    return StepWizardTheme.SolarizedLight;
                case StepWizardAppearance.SolarizedDark:
                    return StepWizardTheme.SolarizedDark;
                case StepWizardAppearance.Linear:
                    return StepWizardTheme.Linear;
                case StepWizardAppearance.Notion:
                    return StepWizardTheme.Notion;
                case StepWizardAppearance.OpenClaw:
                    return StepWizardTheme.OpenClaw;
                case StepWizardAppearance.Matrix:
                    return StepWizardTheme.Matrix;
                case StepWizardAppearance.OneDark:
                    return StepWizardTheme.OneDark;
                case StepWizardAppearance.Dracula:
                    return StepWizardTheme.Dracula;
                case StepWizardAppearance.Nord:
                    return StepWizardTheme.Nord;
                case StepWizardAppearance.GruvboxDark:
                    return StepWizardTheme.GruvboxDark;
                case StepWizardAppearance.GruvboxLight:
                    return StepWizardTheme.GruvboxLight;
                case StepWizardAppearance.TokyoNight:
                    return StepWizardTheme.TokyoNight;
                case StepWizardAppearance.GitHubLight:
                    return StepWizardTheme.GitHubLight;
                case StepWizardAppearance.GitHubDark:
                    return StepWizardTheme.GitHubDark;
                case StepWizardAppearance.VSCodeDarkPlus:
                    return StepWizardTheme.VSCodeDarkPlus;
                case StepWizardAppearance.VisualStudioBlue:
                    return StepWizardTheme.VisualStudioBlue;
                case StepWizardAppearance.VisualStudioDark:
                    return StepWizardTheme.VisualStudioDark;
                case StepWizardAppearance.FluentLight:
                    return StepWizardTheme.FluentLight;
                case StepWizardAppearance.FluentDark:
                    return StepWizardTheme.FluentDark;
                case StepWizardAppearance.WindowsClassic:
                    return StepWizardTheme.WindowsClassic;
                case StepWizardAppearance.HighContrast:
                    return StepWizardTheme.HighContrast;
                default:
                    return StepWizardTheme.Light;
            }
        }

        private static bool IsWindowsAppsDarkMode()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    object value = key == null ? null : key.GetValue("AppsUseLightTheme");
                    return value is int && (int)value == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        private string ResolveHeaderTitle()
        {
            StepWizardPage page = SelectedPage;
            if (page != null && !string.IsNullOrWhiteSpace(page.Title))
            {
                return page.Title;
            }

            return headerTitle;
        }

        private string ResolveHeaderSubtitle()
        {
            StepWizardPage page = SelectedPage;
            if (page != null && !string.IsNullOrWhiteSpace(page.Subtitle))
            {
                return page.Subtitle;
            }

            return headerSubtitle;
        }

        private void UpdateHeader()
        {
            header.Title = ResolveHeaderTitle();
            header.Subtitle = ResolveHeaderSubtitle();
            header.Invalidate();
        }

        private void UpdateButtons()
        {
            backButton.Enabled = CanGoBack;
            nextButton.Enabled = CanGoNext;
            nextButton.Visible = !IsFinishPage;
            finishButton.Visible = showFinishButton && IsFinishPage;
            finishButton.Enabled = SelectedPage != null;
            cancelButton.Visible = showCancelButton;
        }

        private void BackButtonClickCore(object sender, EventArgs e)
        {
            if (IsInDesignMode)
            {
                DesignPreviousPage();
                return;
            }

            OnBackButtonClick(EventArgs.Empty);
            GoBack();
        }

        private void NextButtonClickCore(object sender, EventArgs e)
        {
            if (IsInDesignMode)
            {
                DesignNextPage();
                return;
            }

            OnNextButtonClick(EventArgs.Empty);
            GoNext();
        }

        private void FinishButtonClickCore(object sender, EventArgs e)
        {
            if (IsInDesignMode)
            {
                DesignLastPage();
                return;
            }

            if (ValidateSelectedPage())
            {
                OnFinishButtonClick(EventArgs.Empty);
            }
        }

        private void CancelButtonClickCore(object sender, EventArgs e)
        {
            OnCancelButtonClick(EventArgs.Empty);
        }

        protected virtual void OnSelectedPageChanging(StepWizardPageChangingEventArgs e)
        {
            EventHandler<StepWizardPageChangingEventArgs> handler = SelectedPageChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelectedPageChanged(StepWizardPageChangedEventArgs e)
        {
            EventHandler<StepWizardPageChangedEventArgs> handler = SelectedPageChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnNextButtonClick(EventArgs e)
        {
            EventHandler handler = NextButtonClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnBackButtonClick(EventArgs e)
        {
            EventHandler handler = BackButtonClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnFinishButtonClick(EventArgs e)
        {
            EventHandler handler = FinishButtonClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCancelButtonClick(EventArgs e)
        {
            EventHandler handler = CancelButtonClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPageValidating(StepWizardPageValidatingEventArgs e)
        {
            EventHandler<StepWizardPageValidatingEventArgs> handler = PageValidating;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private sealed class WizardHeader : Control
        {
            private StepWizardTheme theme = StepWizardTheme.Light;

            public string Title { get; set; }
            public string Subtitle { get; set; }

            public WizardHeader()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
                Title = string.Empty;
                Subtitle = string.Empty;
            }

            public void SetTheme(StepWizardTheme value)
            {
                theme = value ?? StepWizardTheme.Light;
                BackColor = theme.HeaderBack;
                ForeColor = theme.Text;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.Clear(theme.HeaderBack);
                Rectangle textBounds = new Rectangle(24, 18, Width - 48, Height - 28);
                using (Font titleFont = new Font(Font.FontFamily, Font.Size + 5F, FontStyle.Bold))
                using (Font subtitleFont = new Font(Font.FontFamily, Font.Size + 1F, FontStyle.Regular))
                using (Pen borderPen = new Pen(theme.Border))
                {
                    TextRenderer.DrawText(e.Graphics, Title, titleFont, textBounds, theme.Text, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
                    Rectangle subtitleBounds = new Rectangle(textBounds.Left, textBounds.Top + 34, textBounds.Width, 26);
                    TextRenderer.DrawText(e.Graphics, Subtitle, subtitleFont, subtitleBounds, theme.MutedText, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
                    e.Graphics.DrawLine(borderPen, 0, Height - 1, Width, Height - 1);
                }
            }
        }

        private sealed class StepListView : Control
        {
            private readonly StepWizardControl owner;
            private StepWizardTheme theme = StepWizardTheme.Light;

            public StepListView(StepWizardControl owner)
            {
                this.owner = owner;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            }

            public void SetTheme(StepWizardTheme value)
            {
                theme = value ?? StepWizardTheme.Light;
                BackColor = theme.SidebarBack;
                ForeColor = theme.Text;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.Clear(theme.SidebarBack);

                int itemHeight = ScaleValue(58);
                int circleSize = ScaleValue(28);
                int x = Padding.Left;
                int y = Padding.Top;
                int textX = x + circleSize + ScaleValue(12);

                using (Pen borderPen = new Pen(theme.Border))
                {
                    e.Graphics.DrawLine(borderPen, Width - 1, 0, Width - 1, Height);
                }

                int visibleStepNumber = 1;
                for (int i = 0; i < owner.Pages.Count; i++)
                {
                    StepWizardPage page = owner.Pages[i];
                    if (page.Suppress)
                    {
                        continue;
                    }

                    Rectangle itemBounds = new Rectangle(Padding.Left / 2, y - ScaleValue(6), Width - Padding.Horizontal / 2 - ScaleValue(8), itemHeight);
                    bool selected = i == owner.selectedPageIndex;
                    bool completed = owner.selectedPageIndex > i;

                    if (selected)
                    {
                        using (GraphicsPath selectedPath = RoundedRectangle(itemBounds, ScaleValue(8)))
                        using (SolidBrush selectedBrush = new SolidBrush(theme.SelectedBack))
                        {
                            e.Graphics.FillPath(selectedBrush, selectedPath);
                        }
                    }

                    Rectangle circleBounds = new Rectangle(x, y + ScaleValue(8), circleSize, circleSize);
                    DrawCircle(e.Graphics, circleBounds, visibleStepNumber, completed, selected);

                    Color titleColor = selected ? theme.AccentText : theme.Text;
                    Color mutedColor = selected ? theme.AccentText : theme.MutedText;
                    string title = string.IsNullOrWhiteSpace(page.Title) ? "Step " + visibleStepNumber : page.Title;
                    string subtitle = completed ? "Completed" : (selected ? "Current step" : "Pending");
                    Rectangle titleBounds = new Rectangle(textX, y + ScaleValue(6), Width - textX - ScaleValue(12), ScaleValue(22));
                    Rectangle subtitleBounds = new Rectangle(textX, y + ScaleValue(30), Width - textX - ScaleValue(12), ScaleValue(20));

                    using (Font titleFont = new Font(Font, selected ? FontStyle.Bold : FontStyle.Regular))
                    {
                        TextRenderer.DrawText(e.Graphics, title, titleFont, titleBounds, titleColor, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
                    }

                    TextRenderer.DrawText(e.Graphics, subtitle, Font, subtitleBounds, mutedColor, TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
                    y += itemHeight;
                    visibleStepNumber++;
                }
            }

            private void DrawCircle(Graphics graphics, Rectangle bounds, int number, bool completed, bool selected)
            {
                Color back = completed ? theme.Success : (selected ? theme.Accent : theme.CardBack);
                Color fore = completed ? theme.AccentText : (selected ? theme.AccentText : theme.Text);
                Color border = selected ? theme.Accent : theme.Border;

                using (SolidBrush backBrush = new SolidBrush(back))
                using (Pen borderPen = new Pen(border, ScaleValue(2)))
                {
                    graphics.FillEllipse(backBrush, bounds);
                    graphics.DrawEllipse(borderPen, bounds);
                }

                if (completed)
                {
                    DrawCheck(graphics, bounds, fore);
                    return;
                }

                string text = number.ToString();
                using (Font circleFont = new Font(Font.FontFamily, Font.Size, FontStyle.Bold))
                {
                    TextRenderer.DrawText(graphics, text, circleFont, bounds, fore, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

            private void DrawCheck(Graphics graphics, Rectangle bounds, Color color)
            {
                Point p1 = new Point(bounds.Left + bounds.Width / 4, bounds.Top + bounds.Height / 2);
                Point p2 = new Point(bounds.Left + bounds.Width / 2 - ScaleValue(1), bounds.Bottom - bounds.Height / 4);
                Point p3 = new Point(bounds.Right - bounds.Width / 4, bounds.Top + bounds.Height / 3);
                using (Pen pen = new Pen(color, ScaleValue(3)))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    graphics.DrawLines(pen, new[] { p1, p2, p3 });
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
