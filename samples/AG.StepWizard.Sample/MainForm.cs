using System;
using System.Windows.Forms;

namespace AG.StepWizard.Sample
{
    public sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            appearanceCombo.Items.AddRange(Enum.GetNames(typeof(StepWizardAppearance)));
            appearanceCombo.SelectedItem = StepWizardAppearance.System.ToString();
            appearanceCombo.SelectedIndexChanged += AppearanceComboSelectedIndexChanged;

            wizard.PageValidating += WizardPageValidating;
            wizard.FinishButtonClick += WizardFinishButtonClick;
            wizard.CancelButtonClick += WizardCancelButtonClick;
            wizard.SelectedPageChanged += WizardSelectedPageChanged;
            ApplyCompanionThemes();
        }

        private void AppearanceComboSelectedIndexChanged(object sender, EventArgs e)
        {
            StepWizardAppearance appearance;
            if (Enum.TryParse(appearanceCombo.SelectedItem.ToString(), out appearance))
            {
                wizard.Appearance = appearance;
                ApplyCompanionThemes();
                UpdateSummary();
            }
        }

        private void WizardPageValidating(object sender, StepWizardPageValidatingEventArgs e)
        {
            if (e.PageIndex == 1 && string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                e.Cancel = true;
                StepWizardMessageBox.Show(this, wizard.Theme, "Enter a project name before continuing.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                nameTextBox.Focus();
            }

            if (e.PageIndex == 2 && !requirementsCheckBox.Checked)
            {
                e.Cancel = true;
                StepWizardMessageBox.Show(this, wizard.Theme, "Confirm the requirements before continuing.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                requirementsCheckBox.Focus();
            }
        }

        private void WizardSelectedPageChanged(object sender, StepWizardPageChangedEventArgs e)
        {
            UpdateSummary();
        }

        private void WizardFinishButtonClick(object sender, EventArgs e)
        {
            StepWizardMessageBox.Show(this, wizard.Theme, "The sample wizard finished successfully.", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WizardCancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void TestButtonClick(object sender, EventArgs e)
        {
            StepWizardMessageBox.Show(this, wizard.Theme, "This dialog is rendered by StepWizardMessageBox and uses the current appearance tokens.", "Themed Dialog", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void ApplyCompanionThemes()
        {
            themedToolTip.ApplyTheme(wizard.Theme);
        }

        private void UpdateSummary()
        {
            SetSummaryValue(0, string.IsNullOrWhiteSpace(nameTextBox.Text) ? "not entered yet" : nameTextBox.Text);
            SetSummaryValue(1, "Adem Gashi");
            SetSummaryValue(2, requirementsCheckBox.Checked ? "yes" : "no");
            SetSummaryValue(3, wizard.Appearance.ToString());
        }

        private void SetSummaryValue(int index, string value)
        {
            if (summaryList.Items.Count <= index)
            {
                return;
            }

            if (summaryList.Items[index].SubItems.Count < 2)
            {
                summaryList.Items[index].SubItems.Add(value);
            }
            else
            {
                summaryList.Items[index].SubItems[1].Text = value;
            }
        }
    }
}
