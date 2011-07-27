using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CA.Web.TimeControl
{
	/// <summary>
	/// TimeList 的摘要说明。
	/// </summary>
	public class TimePicker : DropDownList
	{

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

			this.EnsureChildControls();
		}

		
		protected override void CreateChildControls()
		{
			base.CreateChildControls ();

			if( this.ChildControlsCreated ) return ; 

			 CreateItems();

			base.ChildControlsCreated = true ;

		}

		private void CreateItems()
		{
			for( int i = 0 ; i < 24 ; i ++ )
			{
				ListItem i1 = new ListItem( );
				i1.Text = i.ToString() + ":00" ;
				i1.Value = i.ToString() + ":00" ;
				this.Items.Add( i1 );

				ListItem i2 = new ListItem( );
				i2.Text = i.ToString() + ":30" ;
				i2.Value = i.ToString() + ":30" ;
				this.Items.Add( i2 );
			}
		}

		public override string SelectedValue
		{
			get
			{
				return base.SelectedValue;
			}
			set
			{
				foreach( ListItem i in this.Items  )
				{
					if( i.Value == value )
					{
						i.Selected = true ;
						base.SelectedValue = value;
						return ;
					}
				}

				ListItem ii = new ListItem( value , value ) ;
				ii.Selected = true ;
				this.Items.Insert( 0 , ii );

				base.SelectedValue = value;

			}
		}


		public string SelectedTime
		{
			get
			{
				return this.SelectedValue ;
			}
			set
			{
				this.SelectedValue = value ;
			}
		}

		public void SetTime( DateTime dt )
		{
			string hour = dt.Hour.ToString() ;

			string minute = "00";

			if( dt.Minute != 0 )
				minute = "30";

			this.SelectedTime = hour + ":" + minute ;
			
		}

		



	}
}
