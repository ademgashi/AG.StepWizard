using System.ComponentModel;
using System.Drawing;

namespace AG.StepWizard
{
    /// <summary>
    /// Defines semantic theme tokens used by every rendered wizard surface.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WizardTheme
    {
        [Category("Identity")]
        public string Name { get; set; }

        [Category("Identity")]
        public bool IsDark { get; set; }

        [Category("Surfaces")]
        public Color WindowBack { get; set; }

        [Category("Surfaces")]
        public Color ContentBack { get; set; }

        [Category("Surfaces")]
        public Color HeaderBack { get; set; }

        [Category("Surfaces")]
        public Color SidebarBack { get; set; }

        [Category("Surfaces")]
        public Color CardBack { get; set; }

        [Category("Borders")]
        public Color Border { get; set; }

        [Category("Text")]
        public Color Text { get; set; }

        [Category("Text")]
        public Color MutedText { get; set; }

        [Category("Accent")]
        public Color Accent { get; set; }

        [Category("Accent")]
        public Color AccentText { get; set; }

        [Category("States")]
        public Color HoverBack { get; set; }

        [Category("States")]
        public Color SelectedBack { get; set; }

        [Category("States")]
        public Color DisabledText { get; set; }

        [Category("Semantic")]
        public Color Success { get; set; }

        [Category("Semantic")]
        public Color Warning { get; set; }

        [Category("Semantic")]
        public Color Error { get; set; }

        public WizardTheme()
        {
            Name = string.Empty;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? base.ToString() : Name;
        }
    }
}
