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
using System.Web.UI.WebControls;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website.Admin
{
	public partial class ManageIpAddresses : BaseAdminPageableLookupPage
	{
		#region BaseAdminPageableLookupPage Implementation

		protected override SortableDataGrid Grid
		{
			get
			{
				return EntityDataGrid;
			}
		}

		protected override void BindGrid(int page)
		{
			IpAddressFinder finder = new IpAddressFinder();
			finder.SortExpressions.AddRange(Grid.GetSortExpressions());
			EntityList<IpAddress> data = IpAddress.FindMany(finder, page - 1, PageSize);

			Grid.DataSource = data;
			Grid.DataBind();

			Grid.Visible = (data.Count > 0);
			NoResultsPanel.Visible = (data.Count == 0);

			UpdatePagingControls(data.PagingInfo);
		}

		protected override void Grid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					IpAddress ip = (IpAddress)e.Item.DataItem;

					Label EntityLabel = (Label)e.Item.FindControl("EntityLabel");
					EntityLabel.Text = ip.IpAddressValue;

					LinkButton DeleteLb = (LinkButton)e.Item.Cells[e.Item.Cells.Count - 1].FindControl("DeleteLb");
					DeleteLb.CommandArgument = ip.IpAddressId.ToString();

					break;

				case ListItemType.EditItem:

					ip = (IpAddress)e.Item.DataItem;

					TextBox EntityTextBox = (TextBox)e.Item.FindControl("EntityTextBox");
					EntityTextBox.Text = ip.IpAddressValue;

					LinkButton lb = (LinkButton)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0];
					lb.CommandArgument = ip.IpAddressId.ToString();

					break;
			}
		}

		protected override void Grid_ItemCommand(object source, DataGridCommandEventArgs e)
		{
			if (e.CommandName == "add")
			{
				TextBox NewEntityTextBox = (TextBox)e.Item.FindControl("NewEntityTextBox");
				string ipAddress = NewEntityTextBox.Text.Trim();

				try
				{
					IpAddress ipa = IpAddressManager.Add(ipAddress);
					AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.AddIpAddress, string.Format("Added Ip Address: {0}, IpAddressId: {1}", ipa.IpAddressValue, ipa.IpAddressId));

					ResetGridState();
					MessageLabel1.SetSuccessMessage("IP Address added successfully");
				}
				catch (InvalidIpAddressException ex)
				{
					MessageLabel1.SetErrorMessage(ex.Message);
				}
			}
		}

		protected override void Grid_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			int id = Convert.ToInt32(e.CommandArgument);
			
			IpAddress.Delete(id);
			AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.DeleteIpAddress, string.Format("Deleted Ip Address with IpAddressId: {0}", id));

			BindGrid(CurrentPage);
			MessageLabel1.SetSuccessMessage("IP Address deleted successfully");
		}

		protected override void Grid_UpdateCommand(object source, DataGridCommandEventArgs e)
		{
			TextBox EntityTextBox = (TextBox)e.Item.FindControl("EntityTextBox");
			int id = Convert.ToInt32(e.CommandArgument);

			try
			{
				string ipAddress = EntityTextBox.Text;

				IpAddressManager.Update(id, ipAddress);
				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyIpAddress, string.Format("Modified Ip Address: {0}, IpAddressId: {1}", id, ipAddress));

				ResetGridState();
				MessageLabel1.SetSuccessMessage("IP Address updated successfully");
			}
			catch (InvalidIpAddressException ex)
			{
				MessageLabel1.SetErrorMessage(ex.Message);
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Settings.IpAddressRestrictionEnabled)
			{
				MessageLabel1.SetErrorMessage("IP Address restrictions are currently disabled. Changes to IP addresses will not take effect until this is re-enabled. This can be done by editing the configuration file.");
				MessageLabel1.Pinned = true;
			}
		}
	}
}