using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Web.UI.Design.WebControls;

namespace CA.Web
{
    internal class ParameterListTypeEditor : CollectionEditor // : UITypeEditor
    {
        public ParameterListTypeEditor(Type type)
            : base(type)
        {
        }


        protected override Type[] CreateNewItemTypes()
        {
            Type[] types = new Type[4];
            types[0] = typeof(ControlParameter);
            types[1] = typeof(FormParameter);
            types[2] = typeof(QueryStringParameter);
            //types[3] = typeof(UserParameter);

            return types ;
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

 


        //public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        //{
        //    ObjectMapper component = context.Instance as ObjectMapper ;
        //    if (component == null)
        //    {
        //        return null ;
        //    }

        //    IDesignerHost host = (IDesignerHost)provider.GetService(typeof(IDesignerHost));
        //    DataBoundControlDesigner controlDesigner = (DataBoundControlDesigner)host.GetDesigner(component);

        //    IComponentChangeService service = (IComponentChangeService)provider.GetService(typeof(IComponentChangeService));

        //    //DataControlFieldsEditor form = new DataControlFieldsEditor(controlDesigner);

        //    //if ((UIServiceHelper.ShowDialog(provider, form) == DialogResult.OK) && (service != null))
        //    //{
        //    //    service.OnComponentChanged(component, null, null, null);
        //    //}

        //    //System.Windows.Forms.MessageBox.Show(context.Instance.ToString(), "");

        //    //System.Windows.Forms.MessageBox.Show(context.Container.ToString(), "");

        //    component.Parameters.Add(new FormParameter());

        //    System.Windows.Forms.MessageBox.Show(component.Parameters.Count.ToString(), "");

            

        //    service.OnComponentChanged(component, null, null, null);

        //    return value;
        //}
        //public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        //{
        //    return UITypeEditorEditStyle.Modal;
        //}

 



    }
}
