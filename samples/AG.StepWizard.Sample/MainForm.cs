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
        }

        private void AppearanceComboSelectedIndexChanged(object sender, EventArgs e)
        {
            StepWizardAppearance appearance;
            if (Enum.TryParse(appearanceCombo.SelectedItem.ToString(), out appearance))
            {
                wizard.Appearance = appearance;
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

        private void UpdateSummary()
        {
            summaryList.Items.Clear();
            summaryList.Items.Add("Project name: " + (string.IsNullOrWhiteSpace(nameTextBox.Text) ? "not entered yet" : nameTextBox.Text));
            summaryList.Items.Add("Owner: Adem Gashi");
            summaryList.Items.Add("Requirements confirmed: " + (requirementsCheckBox.Checked ? "yes" : "no"));
            summaryList.Items.Add("Appearance: " + wizard.Appearance);
        }
    }
}
