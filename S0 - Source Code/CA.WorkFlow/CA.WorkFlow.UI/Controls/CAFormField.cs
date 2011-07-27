namespace CA.WorkFlow.UI
{
    using Microsoft.SharePoint.WebControls;
    using FormField = QuickFlow.UI.ListForm.FormField;

    public class CAFormField : FormField
    {
        public override void UpdateFieldValueInItem()
        {
            if (this.ControlMode == SPControlMode.Display)
            {
                return;
            }

            base.UpdateFieldValueInItem();
        }
    }
}