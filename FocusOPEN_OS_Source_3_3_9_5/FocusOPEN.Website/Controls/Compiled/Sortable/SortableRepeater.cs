/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class SortableRepeater : Repeater, ISortableControl
	{
		#region ISortableControl Implementation

		/// <summary>
		/// Gets or sets the default sort expression
		/// </summary>
		public string DefaultSortExpression { get; set; }

		/// <summary>
		/// Gets or sets the default sort direction
		/// </summary>
		public bool DefaultSortAscending { get; set; }

		/// <summary>
		/// Gets or sets the sort expression
		/// </summary>
		public string SortExpression
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "SortExpression", DefaultSortExpression);
			}
			set
			{
				ViewState["SortExpression"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the sort direction
		/// </summary>
		public bool SortAscending
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "SortAscending", DefaultSortAscending);
			}
			set
			{
				ViewState["SortAscending"] = value;
			}
		}

		/// <summary>
		/// Updates the sort expression
		/// </summary>
		public void UpdateSortExpression(string sortExpression)
		{
			SortExpressionParser sep = new SortExpressionParser(SortExpression, SortAscending);

			sep.UpdateSortExpression(sortExpression);

			SortExpression = sep.SortExpression;
			SortAscending = sep.SortAscending;
		}

		/// <summary>
		/// Gets the list of sort expressions
		/// </summary>
		public List<ISortExpression> GetSortExpressions()
		{
			SortExpressionParser sep = new SortExpressionParser(SortExpression, SortAscending);
			return sep.GetSortExpressions();
		}

		#endregion

		#region Constructor

		public SortableRepeater()
		{
			DefaultSortExpression = string.Empty;
			DefaultSortAscending = true;
		}

		#endregion

		/// <summary>
		/// Gets or sets the number of columns in the grid that have sorting information
		/// </summary>
		public int ColumnCount { get; set; }

		protected override void OnItemDataBound(RepeaterItemEventArgs e)
		{
			base.OnItemDataBound(e);

			switch (e.Item.ItemType)
			{
				case ListItemType.Header:

					for (int i = 1; i < ColumnCount+1; i++)
					{
						HtmlTableCell cell = (HtmlTableCell) e.Item.FindControl("HeaderCell" + i);
						LinkButton lb = (LinkButton) cell.FindControl("LinkButton" + i);

						Image existingImage = SiteUtils.FindControlRecursive(cell, "ArrowImage") as Image;
						Literal lit = SiteUtils.FindControlRecursive(cell, "ArrowImageSpacer") as Literal;

						if (existingImage != null)
							cell.Controls.Remove(existingImage);

						if (lit != null)
							cell.Controls.Remove(lit);

						if (lb.CommandArgument == SortExpression)
						{
							Image image = new Image
							              	{
							              		ID = "ArrowImage",
							              		ImageUrl = (SortAscending) ? SiteUtils.GetIconPath("arrowU.gif") : SiteUtils.GetIconPath("arrowD.gif"),
							              		AlternateText = (SortAscending) ? "[Ascending]" : "[Descending]"
							              	};

							lit = new Literal
							      	{
							      		ID = "ArrowImageSpacer",
							      		Text = "&nbsp;"
							      	};

							cell.Controls.Add(lit);
							cell.Controls.Add(image);
						}
					}

					break;
			}
		}
	}
}