using System;
using System.Drawing;
using System.Windows.Forms;

namespace AG.StepWizard.Sample
{
    public sealed partial class MainForm : Form
    {
        private StepWizardLabel messageBoxResultLabel;

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
            CreateMessageBoxTestButtons();
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
            ShowThemedMessageBox("Info", MessageBoxIcon.Information, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button1);
        }

        private void ShowThemedMessageBox(string name, MessageBoxIcon icon, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            DialogResult result = StepWizardMessageBox.Show(
                this,
                wizard.Theme,
                name + " message using " + buttons + ".",
                name + " Dialog",
                buttons,
                icon,
                defaultButton);

            if (messageBoxResultLabel != null)
            {
                messageBoxResultLabel.Text = "Last dialog result: " + result;
            }
        }

        private void CreateMessageBoxTestButtons()
        {
            StepWizardGroupBox messageBoxGroup = new StepWizardGroupBox
            {
                Dock = DockStyle.Bottom,
                Height = 104,
                Text = "StepWizardMessageBox test buttons",
                Padding = new Padding(12, 20, 12, 10)
            };

            FlowLayoutPanel buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                WrapContents = true,
                AutoScroll = false
            };

            messageBoxResultLabel = new StepWizardLabel
            {
                AutoSize = true,
                Margin = new Padding(8, 8, 12, 8),
                Text = "Last dialog result: none"
            };

            buttonPanel.Controls.Add(CreateMessageBoxButton("Info OK", MessageBoxIcon.Information, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1));
            buttonPanel.Controls.Add(CreateMessageBoxButton("Warning OKCancel", MessageBoxIcon.Warning, MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2));
            buttonPanel.Controls.Add(CreateMessageBoxButton("Error RetryCancel", MessageBoxIcon.Error, MessageBoxButtons.RetryCancel, MessageBoxDefaultButton.Button1));
            buttonPanel.Controls.Add(CreateMessageBoxButton("Question YesNo", MessageBoxIcon.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2));
            buttonPanel.Controls.Add(CreateMessageBoxButton("Question YesNoCancel", MessageBoxIcon.Question, MessageBoxButtons.YesNoCancel, MessageBoxDefaultButton.Button3));
            buttonPanel.Controls.Add(CreateMessageBoxButton("AbortRetryIgnore", MessageBoxIcon.Warning, MessageBoxButtons.AbortRetryIgnore, MessageBoxDefaultButton.Button2));
            buttonPanel.Controls.Add(messageBoxResultLabel);

            messageBoxGroup.Controls.Add(buttonPanel);
            controlsPage.Controls.Add(messageBoxGroup);
            messageBoxGroup.BringToFront();
        }

        private StepWizardButton CreateMessageBoxButton(string text, MessageBoxIcon icon, MessageBoxButtons buttons, MessageBoxDefaultButton defaultButton)
        {
            StepWizardButton button = new StepWizardButton
            {
                Text = text,
                Width = 150,
                Height = 32,
                Margin = new Padding(4)
            };

            button.Click += delegate
            {
                ShowThemedMessageBox(text, icon, buttons, defaultButton);
            };

            return button;
        }

        private void ShowControlsStepCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            controlsPage.Suppress = !showControlsStepCheckBox.Checked;
            UpdateSummary();
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
            SetSummaryValue(4, showControlsStepCheckBox.Checked ? "shown" : "suppressed");
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
