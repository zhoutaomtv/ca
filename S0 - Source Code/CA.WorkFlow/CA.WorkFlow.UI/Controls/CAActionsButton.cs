namespace CA.WorkFlow.UI
{
    using System.Web.UI.WebControls;
    using QuickFlow.UI.Controls;

    public class CAActionsButton : ActionsButton, IButtonControl
    {
        public bool CausesValidation { get; set; }

        public string CommandArgument { get; set; }

        public string CommandName { get; set; }

        public string PostBackUrl { get; set; }

        public string Text { get; set; }

        public string ValidationGroup { get; set; }

        public event CommandEventHandler Command;
    }
}