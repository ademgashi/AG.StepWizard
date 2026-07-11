using System;
using System.ComponentModel;

namespace AG.StepWizard
{
    /// <summary>Provides data for selected page navigation events.</summary>
    public class StepWizardPageChangingEventArgs : CancelEventArgs
    {
        public StepWizardPageChangingEventArgs(StepWizardPage currentPage, StepWizardPage nextPage, int currentIndex, int nextIndex)
        {
            CurrentPage = currentPage;
            NextPage = nextPage;
            CurrentIndex = currentIndex;
            NextIndex = nextIndex;
        }

        public StepWizardPage CurrentPage { get; private set; }
        public StepWizardPage NextPage { get; set; }
        public int CurrentIndex { get; private set; }
        public int NextIndex { get; private set; }
    }

    /// <summary>Provides data after the selected page changes.</summary>
    public class StepWizardPageChangedEventArgs : EventArgs
    {
        public StepWizardPageChangedEventArgs(StepWizardPage selectedPage, int selectedIndex)
        {
            SelectedPage = selectedPage;
            SelectedIndex = selectedIndex;
        }

        public StepWizardPage SelectedPage { get; private set; }
        public int SelectedIndex { get; private set; }
    }

    /// <summary>Provides data for page validation before navigation.</summary>
    public class StepWizardPageValidatingEventArgs : CancelEventArgs
    {
        public StepWizardPageValidatingEventArgs(StepWizardPage page, int pageIndex)
        {
            Page = page;
            PageIndex = pageIndex;
        }

        public StepWizardPage Page { get; private set; }
        public int PageIndex { get; private set; }
    }

    /// <summary>Provides data when a page is shown.</summary>
    public class StepWizardPageInitEventArgs : EventArgs
    {
        public StepWizardPageInitEventArgs(StepWizardPage page, int pageIndex)
        {
            Page = page;
            PageIndex = pageIndex;
        }

        public StepWizardPage Page { get; private set; }
        public int PageIndex { get; private set; }
    }

    /// <summary>Provides data when a page is being committed before navigation.</summary>
    public class StepWizardPageConfirmEventArgs : CancelEventArgs
    {
        public StepWizardPageConfirmEventArgs(StepWizardPage page, int pageIndex)
        {
            Page = page;
            PageIndex = pageIndex;
        }

        public StepWizardPage Page { get; private set; }
        public int PageIndex { get; private set; }
    }
}
