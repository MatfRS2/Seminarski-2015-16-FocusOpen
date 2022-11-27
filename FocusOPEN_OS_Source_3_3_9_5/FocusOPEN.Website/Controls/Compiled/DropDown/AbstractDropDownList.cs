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

namespace FocusOPEN.Website.Controls
{
	public abstract class AbstractDropDownList : DropDownList
	{
		#region Private Variables

		private string m_BlankText = "Please select";
		private string m_BlankValue = "0";
		private bool m_OmitBlankItem = false;
		private bool m_BindOnInit = true;

		#endregion

		#region Accessors

		public string BlankText
		{
			get
			{
				return m_BlankText;
			}
			set
			{
				m_BlankText = value;
			}
		}

		public virtual string BlankValue
		{
			get
			{
				return m_BlankValue;
			}
			set
			{
				m_BlankValue = value;
			}
		}

		public bool OmitBlankItem
		{
			get
			{
				return m_OmitBlankItem;
			}
			set
			{
				m_OmitBlankItem = value;
			}
		}

		public bool ContainsBlankItem
		{
			get
			{
				return Items.Contains(GetBlankListItem());
			}
		}

		/// <summary>
		/// Boolean value specifying whether the dropdown should be
		/// automatically bound in the OnInit event.  If this false,
		/// you should manually call the RefreshFromDataSource() method
		/// on your page (usually in Page_Init, or Page_Load events).
		/// </summary>
		public bool BindOnInit
		{
			get
			{
				return m_BindOnInit;
			}
			set
			{
				m_BindOnInit = value;
			}
		}

		public virtual int SelectedId
		{
			get
			{
				try
				{
					return Convert.ToInt32(SelectedItem.Value);
				}
				catch (Exception)
				{
					return 0;
				}
			}
		}

		#endregion

		#region Overrides

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (Items.Count == 0)
			{
				if (BindOnInit)
				{
					BindList();
				}
				else
				{
					InsertBlankListItem();
				}
			}
		}

		#endregion

		#region Abstract Methods

		public new abstract object GetDataSource();

		#endregion

		#region Virtual Methods

		public virtual string GetDataTextField()
		{
			return null;
		}

		public virtual string GetDataValueField()
		{
			return null;
		}

		protected virtual void BindList()
		{
			DataSource = GetDataSource();

			if (GetDataTextField() != null)
				DataTextField = GetDataTextField();

			if (GetDataValueField() != null)
				DataValueField = GetDataValueField();

			Items.Clear();
			
			DataBind();

			if (BlankItemNeeded())
				InsertBlankListItem();
		}

		protected void InsertBlankListItem()
		{
			Items.Insert(0, GetBlankListItem());
		}

		protected ListItem GetBlankListItem()
		{
			return new ListItem(BlankText, BlankValue);
		}

		#endregion

		#region Private Methods

		private bool BlankItemNeeded()
		{
			if (Items.Count < 1)
				return true;

			if (OmitBlankItem)
				return false;

			return (Items[0].Value != BlankValue.ToString());
		}

		#endregion

		#region Public Methods

		public void RefreshFromDataSource()
		{
			BindList();
		}

		/// <summary>
		/// Selects the item with the specified text.
		/// Throws error if item does not exist
		/// </summary>
		public void SelectItem(string text)
		{
			SelectedIndex = -1;

			ListItem item = Items.FindByText(text);
			item.Selected = true;
		}

		/// <summary>
		/// Selects the item with the specified text.
		/// If it doesn't exist, the item with 'selectIfNotFound' text is selected.
		/// Throws an error if both don't exist
		/// </summary>
		public void SelectItem(string text, string selectIfNotFound)
		{
			SelectedIndex = -1;

			ListItem item = Items.FindByText(text);

			if (item == null)
			{
				SelectItem(selectIfNotFound);
				return;
			}

			item.Selected = true;
		}

		public void SelectItem(int value, int selectIfNotFound)
		{
			SelectedIndex = -1;

			ListItem item = Items.FindByValue(value.ToString());

			if (item == null)
			{
				ListItem backup = Items.FindByValue(selectIfNotFound.ToString());
				backup.Selected = true;
				return;
			}

			item.Selected = true;
		}

		/// <summary>
		/// Selects the item with the specified value if found.
		/// No error is thrown if not found
		/// </summary>
		public void SafeSelectValue(object val)
		{
			if (val == null)
				return;

			string s = val.ToString();

			if (Items.FindByValue(s) != null)
				SelectedValue = s;
		}

		#endregion
	}
}