using System.Collections.ObjectModel;

namespace AG.StepWizard
{
    /// <summary>
    /// Represents the pages hosted by a <see cref="StepWizardControl"/>.
    /// </summary>
    public sealed class StepWizardPageCollection : Collection<StepWizardPage>
    {
        private readonly StepWizardControl owner;

        internal StepWizardPageCollection(StepWizardControl owner)
        {
            this.owner = owner;
        }

        protected override void InsertItem(int index, StepWizardPage item)
        {
            base.InsertItem(index, item);
            owner.AttachPage(item);
        }

        protected override void RemoveItem(int index)
        {
            StepWizardPage item = this[index];
            base.RemoveItem(index);
            owner.DetachPage(item);
        }

        protected override void SetItem(int index, StepWizardPage item)
        {
            StepWizardPage oldItem = this[index];
            base.SetItem(index, item);
            owner.DetachPage(oldItem);
            owner.AttachPage(item);
        }

        protected override void ClearItems()
        {
            StepWizardPage[] oldItems = new StepWizardPage[Count];
            CopyTo(oldItems, 0);
            base.ClearItems();
            foreach (StepWizardPage item in oldItems)
            {
                owner.DetachPage(item);
            }
        }
    }
}
