using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AG.StepWizard
{
    internal sealed class StepWizardControlDesigner : ParentControlDesigner, IToolboxUser
    {
        private DesignerVerbCollection verbs;
        private DesignerActionListCollection actionLists;
        private ISelectionService selectionService;
        private IComponentChangeService componentChangeService;

        private StepWizardControl Wizard
        {
            get { return Control as StepWizardControl; }
        }

        public override ICollection AssociatedComponents
        {
            get { return Wizard == null ? base.AssociatedComponents : Wizard.Pages; }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new StepWizardControlActionList(this));
                }

                return actionLists;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                if (verbs == null)
                {
                    verbs = new DesignerVerbCollection
                    {
                        new DesignerVerb("Add page", AddPage),
                        new DesignerVerb("Insert page before current", InsertPage),
                        new DesignerVerb("Remove current page", RemovePage),
                        new DesignerVerb("First page", FirstPage),
                        new DesignerVerb("Previous page", PreviousPage),
                        new DesignerVerb("Next page", NextPage),
                        new DesignerVerb("Last page", LastPage)
                    };
                }

                UpdateVerbState();
                return verbs;
            }
        }

        public override SelectionRules SelectionRules
        {
            get { return SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable; }
        }

        public override bool CanParent(Control control)
        {
            return control is StepWizardPage && Wizard != null && !Wizard.Pages.Contains((StepWizardPage)control);
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            AutoResizeHandles = true;
            selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
            componentChangeService = GetService(typeof(IComponentChangeService)) as IComponentChangeService;
            if (componentChangeService != null)
            {
                componentChangeService.ComponentChanged += ComponentChangeServiceComponentChanged;
            }

            if (Wizard != null)
            {
                Wizard.SelectedPageChanged += WizardSelectedPageChanged;
            }
        }

        bool IToolboxUser.GetToolSupported(ToolboxItem tool)
        {
            StepWizardControl wizard = Wizard;
            if (wizard == null)
            {
                return false;
            }

            return tool.TypeName == typeof(StepWizardPage).FullName || wizard.SelectedPage != null;
        }

        void IToolboxUser.ToolPicked(ToolboxItem tool)
        {
            if (tool.TypeName == typeof(StepWizardPage).FullName)
            {
                InsertPageIntoWizard(false);
                return;
            }

            AddControlToSelectedPage(tool);
        }

        protected override IComponent[] CreateToolCore(ToolboxItem tool, int x, int y, int width, int height, bool hasLocation, bool hasSize)
        {
            StepWizardPageDesigner pageDesigner = GetSelectedPageDesigner();
            if (pageDesigner != null)
            {
                InvokeCreateTool(pageDesigner, tool);
            }

            return null;
        }

        protected override bool GetHitTest(Point point)
        {
            StepWizardControl wizard = Wizard;
            if (wizard == null)
            {
                return base.GetHitTest(point);
            }

            return HitTestButton(wizard.DesignBackButton, point)
                || HitTestButton(wizard.DesignNextButton, point)
                || HitTestButton(wizard.DesignFinishButton, point)
                || HitTestButton(wizard.DesignCancelButton, point);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Wizard != null)
            {
                Wizard.SelectedPageChanged -= WizardSelectedPageChanged;
            }

            if (disposing && componentChangeService != null)
            {
                componentChangeService.ComponentChanged -= ComponentChangeServiceComponentChanged;
            }

            base.Dispose(disposing);
        }

        private void ComponentChangeServiceComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            UpdateVerbState();
        }

        internal void InsertPageIntoWizard(bool insertBeforeCurrent)
        {
            StepWizardControl wizard = Wizard;
            IDesignerHost host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (wizard == null || host == null)
            {
                return;
            }

            using (DesignerTransaction transaction = host.CreateTransaction(insertBeforeCurrent ? "Insert Step Wizard Page" : "Add Step Wizard Page"))
            {
                wizard.DesignAddPage(insertBeforeCurrent);
                transaction.Commit();
            }

            SelectComponent(wizard.SelectedPage);
            RefreshSmartTags();
        }

        internal void RemoveSelectedPage()
        {
            StepWizardControl wizard = Wizard;
            IDesignerHost host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            if (wizard == null || wizard.SelectedPage == null || host == null)
            {
                return;
            }

            using (DesignerTransaction transaction = host.CreateTransaction("Remove Step Wizard Page"))
            {
                wizard.DesignRemoveSelectedPage();
                transaction.Commit();
            }

            SelectComponent(wizard.SelectedPage);
            RefreshSmartTags();
        }

        internal void SelectPage(int index)
        {
            StepWizardControl wizard = Wizard;
            if (wizard == null || index < 0 || index >= wizard.Pages.Count)
            {
                return;
            }

            wizard.SetDesignSelectedPageIndex(index);
            SelectComponent(wizard.SelectedPage);
            RefreshSmartTags();
        }

        internal void FirstPage()
        {
            StepWizardControl wizard = Wizard;
            if (wizard != null && wizard.Pages.Count > 0)
            {
                SelectPage(0);
            }
        }

        internal void PreviousPage()
        {
            StepWizardControl wizard = Wizard;
            if (wizard != null && wizard.CanGoBack)
            {
                SelectPage(wizard.SelectedPageIndex - 1);
            }
        }

        internal void NextPage()
        {
            StepWizardControl wizard = Wizard;
            if (wizard != null && wizard.CanGoNext)
            {
                SelectPage(wizard.SelectedPageIndex + 1);
            }
        }

        internal void LastPage()
        {
            StepWizardControl wizard = Wizard;
            if (wizard != null && wizard.Pages.Count > 0)
            {
                SelectPage(wizard.Pages.Count - 1);
            }
        }

        private void AddControlToSelectedPage(ToolboxItem tool)
        {
            StepWizardPageDesigner pageDesigner = GetSelectedPageDesigner();
            if (pageDesigner != null)
            {
                InvokeCreateTool(pageDesigner, tool);
            }
        }

        private void AddPage(object sender, EventArgs e)
        {
            InsertPageIntoWizard(false);
        }

        private void InsertPage(object sender, EventArgs e)
        {
            InsertPageIntoWizard(true);
        }

        private void RemovePage(object sender, EventArgs e)
        {
            RemoveSelectedPage();
        }

        private void FirstPage(object sender, EventArgs e)
        {
            FirstPage();
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            PreviousPage();
        }

        private void NextPage(object sender, EventArgs e)
        {
            NextPage();
        }

        private void LastPage(object sender, EventArgs e)
        {
            LastPage();
        }

        private StepWizardPageDesigner GetSelectedPageDesigner()
        {
            StepWizardControl wizard = Wizard;
            IDesignerHost host = GetService(typeof(IDesignerHost)) as IDesignerHost;
            return wizard == null || wizard.SelectedPage == null || host == null
                ? null
                : host.GetDesigner(wizard.SelectedPage) as StepWizardPageDesigner;
        }

        private bool HitTestButton(Control button, Point screenPoint)
        {
            return button != null && button.Visible && button.Enabled && button.ClientRectangle.Contains(button.PointToClient(screenPoint));
        }

        private void RefreshSmartTags()
        {
            DesignerActionUIService service = GetService(typeof(DesignerActionUIService)) as DesignerActionUIService;
            if (service != null && Wizard != null)
            {
                service.Refresh(Wizard);
            }
        }

        private void SelectComponent(IComponent component)
        {
            if (selectionService == null || component == null || component.Site == null)
            {
                return;
            }

            selectionService.SetSelectedComponents(new object[] { component }, SelectionTypes.Primary);
        }

        private void UpdateVerbState()
        {
            if (verbs == null || Wizard == null)
            {
                return;
            }

            bool hasPages = Wizard.Pages.Count > 0;
            verbs[1].Enabled = hasPages;
            verbs[2].Enabled = Wizard.SelectedPage != null;
            verbs[3].Enabled = hasPages;
            verbs[4].Enabled = Wizard.CanGoBack;
            verbs[5].Enabled = Wizard.CanGoNext;
            verbs[6].Enabled = hasPages;
        }

        private void WizardSelectedPageChanged(object sender, StepWizardPageChangedEventArgs e)
        {
            SelectComponent(e.SelectedPage);
            RefreshSmartTags();
        }
    }

    internal sealed class StepWizardControlActionList : DesignerActionList
    {
        private readonly StepWizardControlDesigner designer;

        public StepWizardControlActionList(StepWizardControlDesigner designer)
            : base(designer.Component)
        {
            this.designer = designer;
            AutoShow = true;
        }

        public StepWizardPage GoToPage
        {
            get { return Wizard.SelectedPage; }
            set
            {
                if (value != null)
                {
                    designer.SelectPage(Wizard.Pages.IndexOf(value));
                }
            }
        }

        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public StepWizardPageCollection Pages
        {
            get { return Wizard.Pages; }
        }

        public void AddPage()
        {
            designer.InsertPageIntoWizard(false);
        }

        public void InsertPage()
        {
            designer.InsertPageIntoWizard(true);
        }

        public void RemoveCurrentPage()
        {
            designer.RemoveSelectedPage();
        }

        public void PreviousPage()
        {
            designer.PreviousPage();
        }

        public void NextPage()
        {
            designer.NextPage();
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            DesignerActionItemCollection items = new DesignerActionItemCollection
            {
                new DesignerActionHeaderItem("Pages"),
                new DesignerActionMethodItem(this, "AddPage", "Add page", "Pages", "Add a new page to the end of the wizard.", true),
                new DesignerActionMethodItem(this, "InsertPage", "Insert page before current", "Pages", "Insert a page before the selected page.", true)
            };

            if (Wizard.SelectedPage != null)
            {
                items.Add(new DesignerActionMethodItem(this, "RemoveCurrentPage", "Remove current page", "Pages", "Remove the selected page.", true));
            }

            if (Wizard.Pages.Count > 0)
            {
                items.Add(new DesignerActionPropertyItem("GoToPage", "Go to page", "Pages", "Select the page shown in the designer."));
                items.Add(new DesignerActionPropertyItem("Pages", "Edit pages...", "Pages", "Open the page collection editor."));
            }

            if (Wizard.CanGoBack || Wizard.CanGoNext)
            {
                items.Add(new DesignerActionHeaderItem("Navigation"));
            }

            if (Wizard.CanGoBack)
            {
                items.Add(new DesignerActionMethodItem(this, "PreviousPage", "Previous page", "Navigation", "Show the previous page.", false));
            }

            if (Wizard.CanGoNext)
            {
                items.Add(new DesignerActionMethodItem(this, "NextPage", "Next page", "Navigation", "Show the next page.", false));
            }

            return items;
        }

        private StepWizardControl Wizard
        {
            get { return (StepWizardControl)Component; }
        }
    }

    internal sealed class StepWizardPageDesigner : ParentControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get { return SelectionRules.Visible | SelectionRules.Locked; }
        }

        public override bool CanBeParentedTo(IDesigner parentDesigner)
        {
            return parentDesigner is StepWizardControlDesigner;
        }

        protected override void OnPaintAdornments(PaintEventArgs pe)
        {
            Rectangle rectangle = Control.ClientRectangle;
            rectangle.Width--;
            rectangle.Height--;
            ControlPaint.DrawFocusRectangle(pe.Graphics, rectangle);
            base.OnPaintAdornments(pe);
        }
    }
}
