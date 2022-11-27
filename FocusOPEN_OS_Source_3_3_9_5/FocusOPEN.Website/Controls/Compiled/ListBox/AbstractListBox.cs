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
using System.Web.UI.WebControls;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Controls
{
	public abstract class AbstractListBox : ListBox
	{
		#region Abstract Methods

		public new abstract object GetDataSource();
		public abstract string GetDataTextField();
		public abstract string GetDataValueField();

		#endregion

		/// <summary>
		/// Specifies where the blank list item should be displayed
		/// </summary>
		public enum BlankItemLocation
		{
			None,
			Top,
			Bottom
		}

		#region Private variables

		private BlankItemLocation m_BlankItemLocation = BlankItemLocation.None;
		private string m_BlankItemText = "Other";
		private int m_BlankItemValue = 0;

		#endregion

		#region Accessors

		public BlankItemLocation ShowBlankItem
		{
			get
			{
				return m_BlankItemLocation;
			}
			set
			{
				m_BlankItemLocation = value;
			}
		}

		public string BlankItemText
		{
			get
			{
				return m_BlankItemText;
			}
			set
			{
				m_BlankItemText = value;
			}
		}

		public int BlankItemValue
		{
			get
			{
				return m_BlankItemValue;
			}
			set
			{
				m_BlankItemValue = value;
			}
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (Items.Count == 0)
				BindList();

			if (!Page.ClientScript.IsClientScriptBlockRegistered("AbstractListBox"))
			{
				const string script = @"function checkAllItemSelected(e)
									{
										var sv = e.options[e.options.selectedIndex].value;
										
										if (sv == 0)
										{
											for (var i=0; i < e.options.length; i++)
											{
												e.options[i].selected = (e.options[i].value == 0);
											}
										}
									}";
				Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "AbstractListBox", script, true);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (!AutoPostBack && SelectionMode == ListSelectionMode.Multiple && ShowBlankItem != BlankItemLocation.None)
			{
				Attributes.Add("onChange", "checkAllItemSelected(this)");
			}
		}

		public void RefreshFromDataSource()
		{
			BindList();
		}

		protected void BindList()
		{
			DataSource = GetDataSource();
			DataTextField = GetDataTextField();
			DataValueField = GetDataValueField();
			DataBind();

			if (ShowBlankItem != BlankItemLocation.None)
			{
				ListItem li = new ListItem(BlankItemText, BlankItemValue.ToString());

				if (ShowBlankItem == BlankItemLocation.Top)
					Items.Insert(0, li);

				if (ShowBlankItem == BlankItemLocation.Bottom)
					Items.Add(li);
			}
		}

		public int SelectedId
		{
			get
			{
				return NumericUtils.ParseInt32(SelectedValue, 0);
			}
		}

        /// <summary>
        /// basically returns the same as SelectedIds but would include the blank one as well
        /// if available and selected OR if available and no items were selected - pretty much
        /// as a drop down
        /// </summary>
	    public List<Int32> SelectedIdsWithBlank
	    {
	        get
	        {
	            var ids = SelectedIds;

                if(ids.Count > 0 || ShowBlankItem == BlankItemLocation.None) return ids;

                return new List<int>() { BlankItemValue };
	        }
	    }
		public List<Int32> SelectedIds
		{
			get
			{
				List<Int32> list = new List<int>();

				foreach (ListItem item in Items)
				{
					if (item.Selected)
					{
						int value = Convert.ToInt32(item.Value);

						if (value != BlankItemValue)
							list.Add(value);
					}
				}

				return list;
			}
		}

		/// <summary>
		/// Selects the list item with the specified value
		/// Does not throw an error if item does not exist
		/// </summary>
		public void SafeSelectValue(int? id)
		{
			ListItem li = Items.FindByValue(id.ToString());
			if (li != null)
			{
				if (SelectionMode == ListSelectionMode.Single)
					SelectedIndex = -1;

				li.Selected = true;
			}
		}
	}
}