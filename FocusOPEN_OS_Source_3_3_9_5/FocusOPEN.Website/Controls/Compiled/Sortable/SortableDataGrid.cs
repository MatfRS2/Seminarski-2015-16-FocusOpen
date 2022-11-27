/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public sealed class SortableDataGrid : DataGrid, ISortableControl
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

		#region Accessors

		public string DeletePrompt { get; set; }

		public bool ShowAddButtonInFooterEditColumn { get; set;}

		public bool AllowDelete { get; set;}

		#endregion

		#region Constructor

		public SortableDataGrid()
		{
			DefaultSortExpression = string.Empty;
			DefaultSortAscending = true;
			
			ShowFooter = true;
			DeletePrompt = "Are you sure you want to delete this?";
		}

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			AllowSorting = true;
			AutoGenerateColumns = false;
			Width = 910;
			CellSpacing = 0;
			CellPadding = 2;
			CssClass = "TblOutline AppTblMrg";

			HeaderStyle.CssClass = "TblCell1 Bold BodyTxt";
			ItemStyle.CssClass = "TblCell2";
			AlternatingItemStyle.CssClass = "TblCell1";
			FooterStyle.CssClass = "TblCell3";
			EditItemStyle.CssClass = "TblCell3";
		}

		public override void DataBind()
		{
			UpdateColumnHeaders();
			base.DataBind();
		}

		protected override void OnSortCommand(DataGridSortCommandEventArgs e)
		{
			UpdateSortExpression(e.SortExpression);
			base.OnSortCommand(e);
		}

		protected override void OnItemCreated(DataGridItemEventArgs e)
		{
			base.OnItemCreated(e);

			switch (e.Item.ItemType)
			{
				case ListItemType.Header:

					TableCell cell;
					LinkButton lb;

					for (int i = 0; i < e.Item.Cells.Count; i++)
					{
						cell = e.Item.Cells[i];

						if (cell.Controls.Count > 0)
						{
							lb = cell.Controls[0] as LinkButton;

							if (lb != null)
								lb.CssClass = "BodyTxt";
						}
					}

					break;

				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					cell = e.Item.Cells[e.Item.Cells.Count - 1];

					if (ContainsEditCommandColumn())
					{
						cell.CssClass = (AllowDelete) ? "W100" : "W75";
						cell.CssClass += " alignCentre";
					}

					// Edit button
					if (cell.Controls.Count > 0)
					{
						lb = cell.Controls[0] as LinkButton;
						if (lb != null)
							lb.CssClass = "BodyTxt";
					}

					if (AllowDelete)
					{
						Literal lit = new Literal {Text = "&nbsp;<span class=\"BodyTxt\">|</span>&nbsp;"};
						cell.Controls.Add(lit);

						using (PromptLinkButton deleteLb = new PromptLinkButton())
						{
							deleteLb.ID = "DeleteLb";
							deleteLb.Text = "delete";
							deleteLb.CommandName = "Delete";
							deleteLb.CssClass = "BodyTxt";
							deleteLb.Prompt = DeletePrompt;
							cell.Controls.Add(deleteLb);
						}
					}

					break;

				case ListItemType.EditItem:

					cell = e.Item.Cells[e.Item.Cells.Count - 1];
					cell.CssClass = (AllowDelete) ? "W100" : "W75";
					cell.CssClass += " alignCentre White BodyTxt";

					// Update button
					lb = (LinkButton) cell.Controls[0];
					lb.CssClass = "BodyTxt White";

					// Cancel button
					lb = (LinkButton) cell.Controls[2];
					lb.CssClass = "BodyTxt White";

					Literal litA = new Literal {Text = "&nbsp;|"};
					cell.Controls.AddAt(1, litA);

					break;

				case ListItemType.Footer:

					if (ShowAddButtonInFooterEditColumn)
					{
						Button btn = new Button {ID = "AddLinkButton", CommandName = "add", Text = "add", CssClass = "button W50"};

						cell = e.Item.Cells[e.Item.Cells.Count - 1];
						cell.CssClass = "alignCentre";
						cell.Controls.Add(btn);
					}

					break;
			}
		}

		#endregion

		#region Helper Methods

		private bool ContainsEditCommandColumn()
		{
			return Columns.Cast<DataGridColumn>().Any(c => c.GetType() == typeof(EditCommandColumn));
		}

		private void UpdateColumnHeaders()
		{
			foreach (DataGridColumn c in Columns)
			{
				if (c.SortExpression != string.Empty)
				{
					c.HeaderText = Regex.Replace(c.HeaderText, "\\s<.*>", string.Empty);

					if (c.SortExpression == SortExpression)
					{
						if (SortAscending)
						{
							c.HeaderText += " <img src=\"" + ResolveUrl(SiteUtils.GetIconPath("arrowU.gif")) + "\" border=\"0\">";
						}
						else
						{
							c.HeaderText += " <img src=\"" + ResolveUrl(SiteUtils.GetIconPath("arrowD.gif")) + "\" border=\"0\">";
						}
					}
					else
					{
						c.HeaderText += " <img src=\"" + ResolveUrl("~/images/spacer.gif") + "\" width=\"9\" height=\"5\" border=\"0\">";
					}
				}
			}
		}

		#endregion
	}
}