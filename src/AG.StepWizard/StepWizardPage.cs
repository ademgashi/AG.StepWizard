using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AG.StepWizard
{
    /// <summary>
    /// Represents a page hosted by <see cref="StepWizardControl"/>.
    /// </summary>
    [DefaultProperty("Title")]
    [Designer(typeof(StepWizardPageDesigner))]
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class StepWizardPage : Panel
    {
        private bool allowBack = true;
        private bool allowNext = true;
        private bool allowCancel = true;
        private bool allowFinish = true;
        private StepWizardPage nextPage;
        private bool suppress;

        public StepWizardPage()
        {
            AutoScroll = true;
            Margin = Padding.Empty;
            Padding = new Padding(24);
        }

        /// <summary>Gets or sets the text shown in the wizard header and step list.</summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public string Title { get; set; } = string.Empty;

        /// <summary>Gets or sets the subtitle shown when this page is selected.</summary>
        [DefaultValue("")]
        [Category("Appearance")]
        public string Subtitle { get; set; } = string.Empty;

        /// <summary>Gets or sets whether this page should show Finish instead of Next.</summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool IsFinishPage { get; set; }

        /// <summary>Gets or sets whether Back navigation is enabled while this page is selected.</summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool AllowBack
        {
            get { return allowBack; }
            set
            {
                if (allowBack == value)
                {
                    return;
                }

                allowBack = value;
                NotifyOwnerBehaviorChanged();
            }
        }

        /// <summary>Gets or sets whether Next navigation is enabled while this page is selected.</summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool AllowNext
        {
            get { return allowNext; }
            set
            {
                if (allowNext == value)
                {
                    return;
                }

                allowNext = value;
                NotifyOwnerBehaviorChanged();
            }
        }

        /// <summary>Gets or sets whether Cancel is enabled while this page is selected.</summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool AllowCancel
        {
            get { return allowCancel; }
            set
            {
                if (allowCancel == value)
                {
                    return;
                }

                allowCancel = value;
                NotifyOwnerBehaviorChanged();
            }
        }

        /// <summary>Gets or sets whether Finish is enabled while this page is selected.</summary>
        [DefaultValue(true)]
        [Category("Behavior")]
        public bool AllowFinish
        {
            get { return allowFinish; }
            set
            {
                if (allowFinish == value)
                {
                    return;
                }

                allowFinish = value;
                NotifyOwnerBehaviorChanged();
            }
        }

        /// <summary>Gets or sets the explicit next page for this page.</summary>
        [DefaultValue(null)]
        [Category("Behavior")]
        public StepWizardPage NextPage
        {
            get { return nextPage; }
            set
            {
                if (ReferenceEquals(nextPage, value))
                {
                    return;
                }

                nextPage = value;
                NotifyOwnerBehaviorChanged();
            }
        }

        /// <summary>Gets or sets whether this page is hidden from the wizard navigation flow.</summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("When true, Back and Next skip this page and the step list hides it. The page remains in Pages for code and designer editing.")]
        public bool Suppress
        {
            get { return suppress; }
            set
            {
                if (suppress == value)
                {
                    return;
                }

                suppress = value;
                if (Owner != null)
                {
                    Owner.OnPageFlowChanged(this);
                }
            }
        }

        internal StepWizardControl Owner { get; set; }

        /// <summary>Occurs when this page is shown.</summary>
        [Category("Behavior")]
        public event System.EventHandler<StepWizardPageInitEventArgs> Initialize;

        /// <summary>Occurs before this page is committed during Next or Finish navigation.</summary>
        [Category("Behavior")]
        public event System.EventHandler<StepWizardPageConfirmEventArgs> Commit;

        internal void OnInitialize(StepWizardPageInitEventArgs e)
        {
            System.EventHandler<StepWizardPageInitEventArgs> handler = Initialize;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void OnCommit(StepWizardPageConfirmEventArgs e)
        {
            System.EventHandler<StepWizardPageConfirmEventArgs> handler = Commit;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void NotifyOwnerBehaviorChanged()
        {
            if (Owner != null)
            {
                Owner.OnPageBehaviorChanged(this);
            }
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Title) ? base.ToString() : Title;
        }
    }
}
