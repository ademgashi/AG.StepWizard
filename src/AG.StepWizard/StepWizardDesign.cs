using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace AG.StepWizard
{
    public sealed class StepWizardControlDesigner : ParentControlDesigner
    {
        private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection
                    {
                        new StepWizardActionList((StepWizardControl)Control)
                    };
                }

                return actionLists;
            }
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                return new DesignerVerbCollection
                {
                    new DesignerVerb("Add wizard page", AddPage),
                    new DesignerVerb("Remove selected wizard page", RemoveSelectedPage),
                    new DesignerVerb("Previous page", PreviousPage),
                    new DesignerVerb("Next page", NextPage)
                };
            }
        }

        private void AddPage(object sender, EventArgs e)
        {
            StepWizardActionList.AddPage((StepWizardControl)Control);
        }

        private void RemoveSelectedPage(object sender, EventArgs e)
        {
            StepWizardActionList.RemoveSelectedPage((StepWizardControl)Control);
        }

        private void PreviousPage(object sender, EventArgs e)
        {
            StepWizardControl wizard = (StepWizardControl)Control;
            if (wizard.CanGoBack)
            {
                SetProperty(wizard, "SelectedPageIndex", wizard.SelectedPageIndex - 1);
            }
        }

        private void NextPage(object sender, EventArgs e)
        {
            StepWizardControl wizard = (StepWizardControl)Control;
            if (wizard.CanGoNext)
            {
                SetProperty(wizard, "SelectedPageIndex", wizard.SelectedPageIndex + 1);
            }
        }

        internal static void SetProperty(object component, string propertyName, object value)
        {
            PropertyDescriptor property = TypeDescriptor.GetProperties(component)[propertyName];
            if (property != null)
            {
                property.SetValue(component, value);
            }
        }
    }

    public sealed class StepWizardActionList : DesignerActionList
    {
        private readonly StepWizardControl wizard;

        public StepWizardActionList(StepWizardControl wizard)
            : base(wizard)
        {
            this.wizard = wizard;
        }

        public StepWizardAppearance Appearance
        {
            get { return wizard.Appearance; }
            set { SetProperty("Appearance", value); }
        }

        public int SelectedPageIndex
        {
            get { return wizard.SelectedPageIndex; }
            set { SetProperty("SelectedPageIndex", value); }
        }

        public int StepListWidth
        {
            get { return wizard.StepListWidth; }
            set { SetProperty("StepListWidth", value); }
        }

        public bool ShowCancelButton
        {
            get { return wizard.ShowCancelButton; }
            set { SetProperty("ShowCancelButton", value); }
        }

        public bool ShowFinishButton
        {
            get { return wizard.ShowFinishButton; }
            set { SetProperty("ShowFinishButton", value); }
        }

        public bool ThemePageControls
        {
            get { return wizard.ThemePageControls; }
            set { SetProperty("ThemePageControls", value); }
        }

        public void AddPage()
        {
            AddPage(wizard);
        }

        public void RemoveSelectedPage()
        {
            RemoveSelectedPage(wizard);
        }

        public void PreviousPage()
        {
            if (wizard.CanGoBack)
            {
                SetProperty("SelectedPageIndex", wizard.SelectedPageIndex - 1);
            }
        }

        public void NextPage()
        {
            if (wizard.CanGoNext)
            {
                SetProperty("SelectedPageIndex", wizard.SelectedPageIndex + 1);
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            return new DesignerActionItemCollection
            {
                new DesignerActionHeaderItem("Pages"),
                new DesignerActionMethodItem(this, "AddPage", "Add page", "Pages", "Adds a new wizard page.", true),
                new DesignerActionMethodItem(this, "RemoveSelectedPage", "Remove selected page", "Pages", "Removes the current wizard page.", true),
                new DesignerActionMethodItem(this, "PreviousPage", "Previous page", "Pages", "Shows the previous page at design time.", true),
                new DesignerActionMethodItem(this, "NextPage", "Next page", "Pages", "Shows the next page at design time.", true),
                new DesignerActionPropertyItem("SelectedPageIndex", "Selected page index", "Pages", "The page shown in the designer."),
                new DesignerActionHeaderItem("Appearance"),
                new DesignerActionPropertyItem("Appearance", "Appearance", "Appearance", "The built-in appearance previewed by the designer."),
                new DesignerActionPropertyItem("StepListWidth", "Step list width", "Appearance", "The width of the left step list."),
                new DesignerActionPropertyItem("ThemePageControls", "Theme page controls", "Appearance", "Themes controls hosted inside wizard pages."),
                new DesignerActionHeaderItem("Buttons"),
                new DesignerActionPropertyItem("ShowCancelButton", "Show cancel button", "Buttons", "Shows or hides Cancel."),
                new DesignerActionPropertyItem("ShowFinishButton", "Show finish button", "Buttons", "Shows or hides Finish.")
            };
        }

        internal static void AddPage(StepWizardControl wizard)
        {
            if (wizard == null)
            {
                return;
            }

            IDesignerHost host = (IDesignerHost)wizard.Site?.GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host == null ? null : host.CreateTransaction("Add wizard page");
            bool committed = false;

            try
            {
                StepWizardPage page = host == null
                    ? new StepWizardPage()
                    : (StepWizardPage)host.CreateComponent(typeof(StepWizardPage));
                page.Title = "Step " + (wizard.Pages.Count + 1);
                page.Subtitle = "Describe this step.";

                RaiseChanging(wizard, "Pages");
                wizard.Pages.Add(page);
                StepWizardControlDesigner.SetProperty(wizard, "SelectedPageIndex", wizard.Pages.Count - 1);
                RaiseChanged(wizard, "Pages");
                transaction?.Commit();
                committed = true;
            }
            finally
            {
                if (!committed && transaction != null)
                {
                    transaction.Cancel();
                }
            }
        }

        internal static void RemoveSelectedPage(StepWizardControl wizard)
        {
            if (wizard == null || wizard.SelectedPageIndex < 0 || wizard.SelectedPageIndex >= wizard.Pages.Count)
            {
                return;
            }

            IDesignerHost host = (IDesignerHost)wizard.Site?.GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host == null ? null : host.CreateTransaction("Remove wizard page");
            bool committed = false;

            try
            {
                StepWizardPage page = wizard.SelectedPage;
                RaiseChanging(wizard, "Pages");
                wizard.Pages.Remove(page);
                if (host != null && page != null)
                {
                    host.DestroyComponent(page);
                }

                RaiseChanged(wizard, "Pages");
                transaction?.Commit();
                committed = true;
            }
            finally
            {
                if (!committed && transaction != null)
                {
                    transaction.Cancel();
                }
            }
        }

        private void SetProperty(string propertyName, object value)
        {
            StepWizardControlDesigner.SetProperty(wizard, propertyName, value);
        }

        private static void RaiseChanging(IComponent component, string propertyName)
        {
            IComponentChangeService service = (IComponentChangeService)component.Site?.GetService(typeof(IComponentChangeService));
            service?.OnComponentChanging(component, TypeDescriptor.GetProperties(component)[propertyName]);
        }

        private static void RaiseChanged(IComponent component, string propertyName)
        {
            IComponentChangeService service = (IComponentChangeService)component.Site?.GetService(typeof(IComponentChangeService));
            service?.OnComponentChanged(component, TypeDescriptor.GetProperties(component)[propertyName], null, null);
        }
    }

    public sealed class StepWizardPageCollectionEditor : CollectionEditor
    {
        public StepWizardPageCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(StepWizardPage);
        }

        protected override object CreateInstance(Type itemType)
        {
            StepWizardPage page = (StepWizardPage)base.CreateInstance(itemType);
            page.Title = "Step";
            page.Subtitle = "Describe this step.";
            return page;
        }

        protected override string GetDisplayText(object value)
        {
            StepWizardPage page = value as StepWizardPage;
            return page == null ? base.GetDisplayText(value) : page.ToString();
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }
    }
}
