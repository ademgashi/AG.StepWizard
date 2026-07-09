using System.Drawing;
using System.Windows.Forms;

namespace AG.StepWizard.Sample
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components;
        private StepWizardControl wizard;
        private ComboBox appearanceCombo;
        private TextBox nameTextBox;
        private CheckBox requirementsCheckBox;
        private ListBox summaryList;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            wizard = new StepWizardControl();
            appearanceCombo = new ComboBox();
            nameTextBox = new TextBox();
            requirementsCheckBox = new CheckBox();
            summaryList = new ListBox();

            Label appearanceLabel = new Label();
            FlowLayoutPanel toolbar = new FlowLayoutPanel();
            StepWizardPage welcomePage = new StepWizardPage();
            StepWizardPage detailsPage = new StepWizardPage();
            StepWizardPage requirementsPage = new StepWizardPage();
            StepWizardPage reviewPage = new StepWizardPage();
            Label welcomeText = new Label();
            TableLayoutPanel detailsLayout = new TableLayoutPanel();
            Label nameLabel = new Label();
            Label ownerLabel = new Label();
            TextBox ownerTextBox = new TextBox();
            Label requirementsText = new Label();

            SuspendLayout();

            appearanceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            appearanceCombo.Width = 260;

            appearanceLabel.AutoSize = true;
            appearanceLabel.Margin = new Padding(0, 7, 8, 0);
            appearanceLabel.Text = "Appearance";

            toolbar.Dock = DockStyle.Top;
            toolbar.FlowDirection = FlowDirection.LeftToRight;
            toolbar.Height = 48;
            toolbar.Padding = new Padding(16, 10, 16, 8);
            toolbar.WrapContents = false;
            toolbar.Controls.Add(appearanceLabel);
            toolbar.Controls.Add(appearanceCombo);

            wizard.Appearance = StepWizardAppearance.System;
            wizard.Dock = DockStyle.Fill;
            wizard.HeaderSubtitle = "Complete the details below to configure a project workspace.";
            wizard.HeaderTitle = "New Workspace Setup";
            wizard.Name = "wizard";
            wizard.ThemePageControls = true;

            welcomePage.Title = "Welcome";
            welcomePage.Subtitle = "This sample demonstrates the built-in AG.StepWizard appearance catalog.";
            welcomeText.AutoSize = false;
            welcomeText.Dock = DockStyle.Top;
            welcomeText.Height = 96;
            welcomeText.Text = "Use the Appearance dropdown above to switch between System, Light, Dark, OLED Black, Catppuccin, Monokai, Solarized, GitHub, Visual Studio, Fluent, and other built-in themes at runtime. The wizard repaints its header, step list, page area, footer, buttons, borders, selected step, completed indicators, and page child controls.";
            welcomePage.Controls.Add(welcomeText);

            detailsPage.Title = "Project Details";
            detailsPage.Subtitle = "Enter a project name before continuing.";
            detailsLayout.AutoSize = true;
            detailsLayout.ColumnCount = 2;
            detailsLayout.Dock = DockStyle.Top;
            detailsLayout.Margin = new Padding(0);
            detailsLayout.Padding = new Padding(0);
            detailsLayout.RowCount = 2;
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

            nameLabel.AutoSize = true;
            nameLabel.Text = "Project name";
            nameTextBox.Width = 320;

            ownerLabel.AutoSize = true;
            ownerLabel.Text = "Owner";
            ownerTextBox.Text = "Adem Gashi";
            ownerTextBox.Width = 320;

            detailsLayout.Controls.Add(nameLabel, 0, 0);
            detailsLayout.Controls.Add(nameTextBox, 1, 0);
            detailsLayout.Controls.Add(ownerLabel, 0, 1);
            detailsLayout.Controls.Add(ownerTextBox, 1, 1);
            detailsPage.Controls.Add(detailsLayout);

            requirementsPage.Title = "Requirements";
            requirementsPage.Subtitle = "Confirm that required inputs are ready.";
            requirementsText.AutoSize = false;
            requirementsText.Dock = DockStyle.Top;
            requirementsText.Height = 72;
            requirementsText.Text = "A real setup wizard can validate local settings, credentials, or prerequisites before allowing Next.";
            requirementsCheckBox.AutoSize = true;
            requirementsCheckBox.Left = 24;
            requirementsCheckBox.Text = "I confirmed the deployment requirements and support contact.";
            requirementsCheckBox.Top = 80;
            requirementsPage.Controls.Add(requirementsCheckBox);
            requirementsPage.Controls.Add(requirementsText);

            reviewPage.IsFinishPage = true;
            reviewPage.Title = "Review & Finish";
            reviewPage.Subtitle = "Review the captured settings and finish the wizard.";
            summaryList.Dock = DockStyle.Fill;
            summaryList.Items.AddRange(new object[]
            {
                "Project name: not entered yet",
                "Owner: Adem Gashi",
                "Requirements confirmed: no",
                "Appearance: System"
            });
            reviewPage.Controls.Add(summaryList);

            wizard.Pages.Add(welcomePage);
            wizard.Pages.Add(detailsPage);
            wizard.Pages.Add(requirementsPage);
            wizard.Pages.Add(reviewPage);

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 700);
            Controls.Add(wizard);
            Controls.Add(toolbar);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(900, 650);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AG.StepWizard Sample";

            ResumeLayout(false);
        }
    }
}
