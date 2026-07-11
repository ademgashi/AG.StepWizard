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
        private StepWizardCheckBox showControlsStepCheckBox;
        private StepWizardPage controlsPage;
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
            showControlsStepCheckBox = new StepWizardCheckBox();
            summaryList = new StepWizardListView();
            themedToolTip = new StepWizardToolTip();
            testComboBox = new StepWizardComboBox();
            testProgressBar = new StepWizardProgressBar();

            StepWizardLabel appearanceLabel = new StepWizardLabel();
            FlowLayoutPanel toolbar = new FlowLayoutPanel();
            StepWizardPage welcomePage = new StepWizardPage();
            StepWizardPage detailsPage = new StepWizardPage();
            StepWizardPage requirementsPage = new StepWizardPage();
            controlsPage = new StepWizardPage();
            StepWizardPage reviewPage = new StepWizardPage();
            StepWizardLabel welcomeText = new StepWizardLabel();
            FlowLayoutPanel welcomeLayout = new FlowLayoutPanel();
            StepWizardCard packageCard = new StepWizardCard();
            StepWizardLabel packageTitle = new StepWizardLabel();
            StepWizardLabel packageSummary = new StepWizardLabel();
            FlowLayoutPanel deploymentCards = new FlowLayoutPanel();
            StepWizardOptionCard singleServerCard = new StepWizardOptionCard();
            StepWizardOptionCard threeServerCard = new StepWizardOptionCard();
            FlowLayoutPanel operationCards = new FlowLayoutPanel();
            StepWizardOptionCard installCard = new StepWizardOptionCard();
            StepWizardOptionCard upgradeCard = new StepWizardOptionCard();
            StepWizardOptionCard uninstallCard = new StepWizardOptionCard();
            FlowLayoutPanel statusCards = new FlowLayoutPanel();
            StepWizardStatusCard processorStatusCard = new StepWizardStatusCard();
            StepWizardStatusCard memoryStatusCard = new StepWizardStatusCard();
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
            ListViewItem listViewDemoItem1 = new ListViewItem("StepWizardListView");
            ListViewItem listViewDemoItem2 = new ListViewItem("Owner draw rows");
            ListViewItem summaryProjectNameItem = new ListViewItem("Project name");
            ListViewItem summaryOwnerItem = new ListViewItem("Owner");
            ListViewItem summaryRequirementsItem = new ListViewItem("Requirements confirmed");
            ListViewItem summaryAppearanceItem = new ListViewItem("Appearance");
            ListViewItem summaryControlsStepItem = new ListViewItem("Themed controls step");

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
            wizard.ChromeStyle = StepWizardChromeStyle.Enterprise;
            wizard.Dock = DockStyle.Fill;
            wizard.HeaderSubtitle = "Complete the details below to configure a project workspace.";
            wizard.HeaderTitle = "New Workspace Setup";
            wizard.Name = "wizard";
            wizard.StepStatusTextMode = StepWizardStepStatusTextMode.Hidden;
            wizard.ThemePageControls = true;

            welcomePage.Title = "Welcome";
            welcomePage.Subtitle = "Enterprise chrome, compact steps, option cards, and status cards.";
            welcomeLayout.Dock = DockStyle.Fill;
            welcomeLayout.FlowDirection = FlowDirection.TopDown;
            welcomeLayout.Padding = new Padding(0);
            welcomeLayout.WrapContents = false;
            welcomeText.AutoSize = false;
            welcomeText.Dock = DockStyle.Top;
            welcomeText.Height = 46;
            welcomeText.Margin = new Padding(0, 0, 0, 8);
            welcomeText.Text = "Use the Appearance dropdown above to switch themes at runtime. The wizard shell now uses enterprise spacing, compact header chrome, and primary navigation.";

            packageCard.Width = 780;
            packageCard.Height = 102;
            packageCard.Margin = new Padding(0, 0, 0, 12);
            packageTitle.AutoSize = false;
            packageTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            packageTitle.Location = new Point(16, 14);
            packageTitle.Size = new Size(720, 22);
            packageTitle.Text = "Package summary";
            packageSummary.AutoSize = false;
            packageSummary.Location = new Point(16, 40);
            packageSummary.Size = new Size(720, 48);
            packageSummary.Text = "Infrastructure: MongoDB, RabbitMQ, OpenSearch, PostgreSQL   |   Applications: Admin, Search, API, Automation   |   Framework: Symphony Core, SDP Framework";
            packageCard.Controls.Add(packageTitle);
            packageCard.Controls.Add(packageSummary);

            deploymentCards.AutoSize = true;
            deploymentCards.Margin = new Padding(0, 0, 0, 12);
            deploymentCards.WrapContents = false;
            singleServerCard.Text = "Single Server";
            singleServerCard.Subtitle = "Install all services on this machine.";
            singleServerCard.Selected = true;
            singleServerCard.Margin = new Padding(0, 0, 12, 0);
            threeServerCard.Text = "Three Server";
            threeServerCard.Subtitle = "Distribute roles across dedicated servers.";
            threeServerCard.Margin = new Padding(0);
            deploymentCards.Controls.Add(singleServerCard);
            deploymentCards.Controls.Add(threeServerCard);

            operationCards.AutoSize = true;
            operationCards.Margin = new Padding(0, 0, 0, 12);
            operationCards.WrapContents = false;
            installCard.Text = "Install";
            installCard.Subtitle = "Fresh Lomond installation.";
            installCard.Selected = true;
            installCard.Margin = new Padding(0, 0, 12, 0);
            upgradeCard.Text = "Upgrade";
            upgradeCard.Subtitle = "Move an existing system forward.";
            upgradeCard.Margin = new Padding(0, 0, 12, 0);
            uninstallCard.Text = "Uninstall";
            uninstallCard.Subtitle = "Remove selected components.";
            uninstallCard.Margin = new Padding(0);
            operationCards.Controls.Add(installCard);
            operationCards.Controls.Add(upgradeCard);
            operationCards.Controls.Add(uninstallCard);

            statusCards.AutoSize = true;
            statusCards.Margin = new Padding(0);
            statusCards.WrapContents = false;
            processorStatusCard.Text = "Processor";
            processorStatusCard.Subtitle = "12 cores available";
            processorStatusCard.Status = StepWizardTaskStatus.Completed;
            processorStatusCard.Width = 220;
            processorStatusCard.Margin = new Padding(0, 0, 12, 0);
            memoryStatusCard.Text = "Memory";
            memoryStatusCard.Subtitle = "24 GB available, 64 GB recommended";
            memoryStatusCard.Status = StepWizardTaskStatus.Warning;
            memoryStatusCard.Width = 300;
            memoryStatusCard.Margin = new Padding(0);
            statusCards.Controls.Add(processorStatusCard);
            statusCards.Controls.Add(memoryStatusCard);

            welcomeLayout.Controls.Add(welcomeText);
            welcomeLayout.Controls.Add(packageCard);
            welcomeLayout.Controls.Add(deploymentCards);
            welcomeLayout.Controls.Add(operationCards);
            welcomeLayout.Controls.Add(statusCards);
            welcomePage.Controls.Add(welcomeLayout);

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
            showControlsStepCheckBox.AutoSize = true;
            showControlsStepCheckBox.Checked = true;
            showControlsStepCheckBox.Left = 24;
            showControlsStepCheckBox.Text = "Show the optional themed controls step.";
            showControlsStepCheckBox.Top = 112;
            showControlsStepCheckBox.CheckedChanged += ShowControlsStepCheckBoxCheckedChanged;
            requirementsPage.Controls.Add(showControlsStepCheckBox);
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
            controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 104F));
            controlsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 172F));

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
            listViewDemoItem1.SubItems.Add("Ready");
            listViewDemoItem2.SubItems.Add("Themed");
            listViewDemo.Items.Add(listViewDemoItem1);
            listViewDemo.Items.Add(listViewDemoItem2);
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

            pendingTask.Width = 210;
            pendingTask.Height = 72;
            pendingTask.Margin = new Padding(0, 0, 8, 8);
            runningTask.Width = 210;
            runningTask.Height = 72;
            runningTask.Margin = new Padding(0, 0, 8, 8);
            completedTask.Width = 210;
            completedTask.Height = 72;
            completedTask.Margin = new Padding(0, 0, 8, 8);
            errorTask.Width = 210;
            errorTask.Height = 72;
            errorTask.Margin = new Padding(0, 0, 8, 8);
            warningTask.Width = 210;
            warningTask.Height = 72;
            warningTask.Margin = new Padding(0, 0, 8, 8);
            taskStatesPanel.Controls.Add(pendingTask);
            taskStatesPanel.Controls.Add(runningTask);
            taskStatesPanel.Controls.Add(completedTask);
            taskStatesPanel.Controls.Add(errorTask);
            taskStatesPanel.Controls.Add(warningTask);

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
            summaryProjectNameItem.SubItems.Add("not entered yet");
            summaryOwnerItem.SubItems.Add("Adem Gashi");
            summaryRequirementsItem.SubItems.Add("no");
            summaryAppearanceItem.SubItems.Add("System");
            summaryControlsStepItem.SubItems.Add("shown");
            summaryList.Items.Add(summaryProjectNameItem);
            summaryList.Items.Add(summaryOwnerItem);
            summaryList.Items.Add(summaryRequirementsItem);
            summaryList.Items.Add(summaryAppearanceItem);
            summaryList.Items.Add(summaryControlsStepItem);
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
