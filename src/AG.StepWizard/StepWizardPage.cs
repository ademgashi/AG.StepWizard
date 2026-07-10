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

        public override string ToString()
        {
            return string.IsNullOrEmpty(Title) ? base.ToString() : Title;
        }
    }
}
