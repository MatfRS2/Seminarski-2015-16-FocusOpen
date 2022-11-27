using System.Web.UI.WebControls;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class MultiBrandPlaceHolder : PlaceHolder
	{
		public override bool Visible
		{
			get
			{
				return (SessionInfo.Current.User.Brands.Count > 1);
			}
		}
	}
}