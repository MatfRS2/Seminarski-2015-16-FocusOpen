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
using System.Web.UI.WebControls;
using FocusOPEN.Shared;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class UserDetailsForm : BaseUserControl
	{
		public enum Modes
		{
			Registration,
			EditProfile
		}

		private Modes m_Mode = Modes.Registration;

		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				IsStaffUserRadioButtonList.SelectedValue = "1";
				CompanyNameTextBox.Text = WebsiteBrandManager.GetBrand().OrganisationName;

                //Sets the default state for the form.
                IsStaffUserCheck();

                BindCompanyDropDownList();
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (Mode == Modes.Registration)
			{
				RegistrationOnlyOptions.Visible = true;
				BrandSelectorRow.Visible = (WebsiteBrandManager.GetBrand().IsBrandSelectionAllowed && BrandCache.Instance.GetList().Count > 1);
			}
			else
			{
				RegistrationOnlyOptions.Visible = false;
			}
		}

		protected void IsStaffUserRadioButtonList_SelectedIndexChanged(object sender, EventArgs e)
		{
			IsStaffUserCheck();
		}

		#endregion

		#region Accessors

		public Modes Mode
		{
			get
			{
				return m_Mode;
			}
			set
			{
				m_Mode = value;
			}
		}

		public string FirstName
		{
			get
			{
				return FirstNameTextBox.Text.Trim();
			}
			set
			{
				FirstNameTextBox.Text = value;
			}
		}

		public string LastName
		{
			get
			{
				return LastNameTextBox.Text.Trim();
			}
			set
			{
				LastNameTextBox.Text = value;
			}
		}

		public string Email
		{
			get
			{
				return EmailTextBox.Text.Trim();
			}
			set
			{
				EmailTextBox.Text = value;
			}
		}

		public string Password
		{
			get
			{
				return PasswordTextBox.Text.Trim();
			}
			set
			{
				PasswordTextBox.Text = value;
			}
		}

		public string ConfirmPassword
		{
			get
			{
				return ConfirmPasswordTextBox.Text.Trim();
			}
			set
			{
				ConfirmPasswordTextBox.Text = value;
			}
		}

		public bool IsStaffUser
		{
			get
			{
				return (IsStaffUserRadioButtonList.SelectedValue == "1");
			}
			set
			{
				IsStaffUserRadioButtonList.SelectedValue = (value) ? "1" : "0";
				IsStaffUserCheck();
			}
		}

		public string CompanyName
		{
			get
			{
                if (IsStaffUser)
                    return CompanyDropDownList.SelectedValue;

				return CompanyNameTextBox.Text.Trim();
			}
			set
			{
                //Although this value is set in EditProfile.aspx it is only visible for Registration. 
                ListItem companyListItem = CompanyDropDownList.Items.FindByValue(value);

                if (companyListItem != null)
                {
                    companyListItem.Selected = true;
                }
                else
                { 
                    CompanyNameTextBox.Text = value;
                }
			}
		}

		public int BrandId
		{
			get
			{
				if (BrandSelectorRow.Visible)
					return BrandDropDownList.SelectedId;

				return WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault();
			}
			set
			{
				BrandDropDownList.SafeSelectValue(value);
			}
		}

		public int? CountryId
		{
			get
			{
				if (CountryDropDownList1.SelectedId <= 0)
					return null;

				return CountryDropDownList1.SelectedId;
			}
			set
			{
				CountryDropDownList1.SafeSelectValue(value);
			}
		}

		public string PhoneNumber
		{
			get
			{
				return PhoneNumberTextBox.Text.Trim();
			}
			set
			{
				PhoneNumberTextBox.Text = value;
			}
		}

		public string MobileNumber
		{
			get
			{
				return MobileNumberTextBox.Text.Trim();
			}
			set
			{
				MobileNumberTextBox.Text = value;
			}
		}

		#endregion

		#region Private Methods

		private void IsStaffUserCheck()
		{
			bool isStaffUser = IsStaffUserRadioButtonList.SelectedValue == "1";

            //Set the visibility of the controls.
            CompanyNameTextBox.Visible = !isStaffUser;
            CompanyDropDownList.Visible = isStaffUser;

			if (!isStaffUser)
			{
				CompanyNameTextBox.Enabled = true;
				Company ac = UserSecurityManager.GetCompanyByDomain(EmailTextBox.Text.Trim());
				CompanyNameTextBox.Text = ac.Name;
			}
		}

        private void BindCompanyDropDownList()
        {
            List<Company> list = new List<Company>();
            list.AddRange(CompanyCache.Instance.GetList()
                .Where(c => (c.IsInternal) && (BrandManager.IsSingleBrandMode || c.BrandList.Any(b => b.BrandId == WebsiteBrandManager.GetBrand().BrandId.GetValueOrDefault()))));

            CompanyDropDownList.DataSource = list;
            CompanyDropDownList.DataTextField = Company.Columns.Name.ToString();
            CompanyDropDownList.DataValueField = Company.Columns.Name.ToString();
            CompanyDropDownList.DataBind();
        }

		#endregion
	}
}