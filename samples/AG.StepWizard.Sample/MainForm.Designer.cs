using System.Drawing;
using System.Windows.Forms;

namespace AG.StepWizard.Sample
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components;
        private StepWizardControl wizard;
        private StepWizardComboBox appearanceCombo;
        private StepWizardTextBox nameTextBox;
        private StepWizardCheckBox requirementsCheckBox;
        private StepWizardListView summaryList;
        private StepWizardToolTip themedToolTip;
        private StepWizardComboBox testComboBox;
        private StepWizardProgressBar testProgressBar;

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
            appearanceCombo = new StepWizardComboBox();
            nameTextBox = new StepWizardTextBox();
            requirementsCheckBox = new StepWizardCheckBox();
            summaryList = new StepWizardListView();
            themedToolTip = new StepWizardToolTip();
            testComboBox = new StepWizardComboBox();
            testProgressBar = new StepWizardProgressBar();

            StepWizardLabel appearanceLabel = new StepWizardLabel();
            FlowLayoutPanel toolbar = new FlowLayoutPanel();
            StepWizardPage welcomePage = new StepWizardPage();
            StepWizardPage detailsPage = new StepWizardPage();
            StepWizardPage requirementsPage = new StepWizardPage();
            StepWizardPage controlsPage = new StepWizardPage();
            StepWizardPage reviewPage = new StepWizardPage();
            StepWizardLabel welcomeText = new StepWizardLabel();
            TableLayoutPanel detailsLayout = new TableLayoutPanel();
            StepWizardLabel nameLabel = new StepWizardLabel();
            StepWizardLabel ownerLabel = new StepWizardLabel();
            StepWizardTextBox ownerTextBox = new StepWizardTextBox();
            StepWizardLabel requirementsText = new StepWizardLabel();
            StepWizardGroupBox controlsGroup = new StepWizardGroupBox();
            TableLayoutPanel controlsLayout = new TableLayoutPanel();
            StepWizardLabel labelDemo = new StepWizardLabel();
            StepWizardTextBox textBoxDemo = new StepWizardTextBox();
            StepWizardCheckBox checkBoxDemo = new StepWizardCheckBox();
            StepWizardRadioButton radioButtonA = new StepWizardRadioButton();
            StepWizardRadioButton radioButtonB = new StepWizardRadioButton();
            StepWizardButton buttonDemo = new StepWizardButton();
            StepWizardCheckedListBox checkedListDemo = new StepWizardCheckedListBox();
            StepWizardListView listViewDemo = new StepWizardListView();
            FlowLayoutPanel taskStatesPanel = new FlowLayoutPanel();
            StepWizardTaskItemControl pendingTask = new StepWizardTaskItemControl();
            StepWizardTaskItemControl runningTask = new StepWizardTaskItemControl();
            StepWizardTaskItemControl completedTask = new StepWizardTaskItemControl();
            StepWizardTaskItemControl errorTask = new StepWizardTaskItemControl();
            StepWizardTaskItemControl warningTask = new StepWizardTaskItemControl();

            SuspendLayout();

            appearanceLabel.AutoSize = true;
            appearanceLabel.Margin = new Padding(0, 7, 8, 0);
            appearanceLabel.Text = "Appearance";

            appearanceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            appearanceCombo.Width = 260;

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
            welcomePage.Subtitle = "Switch appearances and inspect the themed wizard shell.";
            welcomeText.AutoSize = false;
            welcomeText.Dock = DockStyle.Top;
            welcomeText.Height = 112;
            welcomeText.Text = "Use the Appearance dropdown above to switch every built-in theme at runtime. The sample now includes a dedicated step for themed companion controls and uses StepWizardMessageBox for themed validation dialogs.";
            welcomePage.Controls.Add(welcomeText);

            detailsPage.Title = "Project Details";
            detailsPage.Subtitle = "Enter a project name before continuing.";
            detailsLayout.AutoSize = true;
            detailsLayout.ColumnCount = 2;
            detailsLayout.Dock = DockStyle.Top;
            detailsLayout.Margin = new Padding(0);
            detailsLayout.RowCount = 2;
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            nameLabel.Text = "Project name";
            ownerLabel.Text = "Owner";
            nameTextBox.Width = 320;
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

            controlsPage.Title = "Themed Controls";
            controlsPage.Subtitle = "All AG companion controls read from StepWizardTheme tokens.";
            controlsGroup.Dock = DockStyle.Fill;
            controlsGroup.Text = "Companion control coverage";
            controlsLayout.ColumnCount = 2;
            controlsLayout.Dock = DockStyle.Fill;
            controlsLayout.Padding = new Padding(16, 20, 16, 16);
            controlsLayout.RowCount = 6;
            controlsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            controlsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            for (int i = 0; i < 6; i++)
            {
                controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, i < 4 ? 48F : i == 4 ? 104F : 172F));
            }

            labelDemo.Text = "StepWizardLabel";
            labelDemo.Margin = new Padding(8, 8, 8, 8);
            textBoxDemo.Text = "Themed focus border";
            textBoxDemo.Width = 240;
            textBoxDemo.Margin = new Padding(8, 4, 8, 8);
            checkBoxDemo.Text = "StepWizardCheckBox";
            checkBoxDemo.Checked = true;
            checkBoxDemo.Size = new Size(210, 26);
            checkBoxDemo.Margin = new Padding(8, 8, 8, 8);
            radioButtonA.Text = "StepWizardRadioButton A";
            radioButtonA.Checked = true;
            radioButtonA.Size = new Size(230, 26);
            radioButtonA.Margin = new Padding(8, 8, 8, 8);
            radioButtonB.Text = "StepWizardRadioButton B";
            radioButtonB.Size = new Size(230, 26);
            radioButtonB.Margin = new Padding(8, 8, 8, 8);
            buttonDemo.Text = "StepWizardButton";
            buttonDemo.Width = 160;
            buttonDemo.Margin = new Padding(8, 6, 8, 8);
            buttonDemo.Click += TestButtonClick;

            testComboBox.Items.AddRange(new object[] { "StepWizardComboBox", "VS Code Dark+", "Catppuccin Mocha" });
            testComboBox.SelectedIndex = 0;
            testComboBox.Width = 220;
            testComboBox.Margin = new Padding(8, 6, 8, 8);

            testProgressBar.Value = 68;
            testProgressBar.Width = 240;
            testProgressBar.Margin = new Padding(8, 12, 8, 8);

            checkedListDemo.Items.AddRange(new object[] { "StepWizardCheckedListBox", "Checked item", "Another option" });
            checkedListDemo.SetItemChecked(0, true);
            checkedListDemo.SetItemChecked(1, true);
            checkedListDemo.Height = 86;
            checkedListDemo.Width = 320;
            checkedListDemo.Margin = new Padding(8, 4, 8, 8);

            listViewDemo.Columns.Add("ColumnHeader", 130);
            listViewDemo.Columns.Add("Status", 200);
            listViewDemo.Items.Add(new ListViewItem(new[] { "StepWizardListView", "Ready" }));
            listViewDemo.Items.Add(new ListViewItem(new[] { "Owner draw rows", "Themed" }));
            listViewDemo.Height = 86;
            listViewDemo.Width = 330;
            listViewDemo.Margin = new Padding(8, 4, 8, 8);

            taskStatesPanel.AutoSize = true;
            taskStatesPanel.Dock = DockStyle.Fill;
            taskStatesPanel.Margin = new Padding(8, 4, 8, 8);
            taskStatesPanel.WrapContents = true;

            pendingTask.Text = "Pending task";
            pendingTask.ProgressText = "Waiting to start";
            pendingTask.Status = StepWizardTaskStatus.Pending;

            runningTask.Text = "Running task";
            runningTask.ProgressText = "Animated themed spinner";
            runningTask.Status = StepWizardTaskStatus.Running;
            runningTask.ShowInstallCheck = true;
            runningTask.InstallChecked = true;

            completedTask.Text = "Finished task";
            completedTask.ProgressText = "Completed successfully";
            completedTask.Status = StepWizardTaskStatus.Completed;

            errorTask.Text = "Failed task";
            errorTask.ProgressText = "Install step failed";
            errorTask.Status = StepWizardTaskStatus.Error;

            warningTask.Text = "Warning task";
            warningTask.ProgressText = "Completed with warnings";
            warningTask.Status = StepWizardTaskStatus.Warning;

            StepWizardTaskItemControl[] taskItems = { pendingTask, runningTask, completedTask, errorTask, warningTask };
            for (int i = 0; i < taskItems.Length; i++)
            {
                taskItems[i].Width = 210;
                taskItems[i].Height = 72;
                taskItems[i].Margin = new Padding(0, 0, 8, 8);
                taskStatesPanel.Controls.Add(taskItems[i]);
            }

            controlsLayout.Controls.Add(labelDemo, 0, 0);
            controlsLayout.Controls.Add(textBoxDemo, 1, 0);
            controlsLayout.Controls.Add(checkBoxDemo, 0, 1);
            controlsLayout.Controls.Add(radioButtonA, 1, 1);
            controlsLayout.Controls.Add(buttonDemo, 0, 2);
            controlsLayout.Controls.Add(radioButtonB, 1, 2);
            controlsLayout.Controls.Add(testComboBox, 0, 3);
            controlsLayout.Controls.Add(testProgressBar, 1, 3);
            controlsLayout.Controls.Add(checkedListDemo, 0, 4);
            controlsLayout.Controls.Add(listViewDemo, 1, 4);
            controlsLayout.Controls.Add(taskStatesPanel, 0, 5);
            controlsLayout.SetColumnSpan(taskStatesPanel, 2);
            controlsGroup.Controls.Add(controlsLayout);
            controlsPage.Controls.Add(controlsGroup);

            themedToolTip.SetToolTip(textBoxDemo, "StepWizardToolTip uses the active theme.");
            themedToolTip.SetToolTip(buttonDemo, "Click to show a themed StepWizardMessageBox.");
            themedToolTip.SetToolTip(testProgressBar, "StepWizardProgressBar uses Accent and Border tokens.");

            reviewPage.IsFinishPage = true;
            reviewPage.Title = "Review & Finish";
            reviewPage.Subtitle = "Review the captured settings and finish the wizard.";
            summaryList.Dock = DockStyle.Fill;
            summaryList.Columns.Add("Setting", 180);
            summaryList.Columns.Add("Value", 320);
            summaryList.Items.Add(new ListViewItem(new[] { "Project name", "not entered yet" }));
            summaryList.Items.Add(new ListViewItem(new[] { "Owner", "Adem Gashi" }));
            summaryList.Items.Add(new ListViewItem(new[] { "Requirements confirmed", "no" }));
            summaryList.Items.Add(new ListViewItem(new[] { "Appearance", "System" }));
            reviewPage.Controls.Add(summaryList);

            wizard.Pages.Add(welcomePage);
            wizard.Pages.Add(detailsPage);
            wizard.Pages.Add(requirementsPage);
            wizard.Pages.Add(controlsPage);
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
