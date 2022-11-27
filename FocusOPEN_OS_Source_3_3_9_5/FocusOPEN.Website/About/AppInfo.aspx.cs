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
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.About
{
	public partial class AppInfo : Page
	{
		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				string folder = Server.MapPath("~/bin");
				string[] files = Directory.GetFiles(folder, "*.dll");

				repeater1.DataSource = files;
				repeater1.DataBind();

				Assembly currentAssembly = Assembly.GetExecutingAssembly();
				AssemblyInfo currentAssemblyInfo = new AssemblyInfo(currentAssembly);

				DisplayAssemblyInformation(currentAssemblyInfo);

				CurrentAssemblyNameLabel.Text = currentAssembly.GetName().Name;
				CurrentAssemblyLastModifiedLabel.Text = currentAssemblyInfo.LastModifiedDate.ToString("dd MMMM yyyy HH:mm");
				CurrentAssemblyBuildNumberLabel.Text = currentAssemblyInfo.Version;

				TimeSpan ts = DateTime.Now - currentAssemblyInfo.LastModifiedDate;
				CurrentAssemblyLastModifiedLabel.Text += " (" + ts.Days + " days ago)";
			}
		}

		protected void repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:

					string path = e.Item.DataItem.ToString();

					LinkButton FileNameLinkButton = (LinkButton) e.Item.FindControl("FileNameLinkButton");
					FileNameLinkButton.Text = Path.GetFileName(path);
					FileNameLinkButton.CommandArgument = path;

					break;
			}
		}

		protected void FileNameLinkButton_Command(object sender, CommandEventArgs e)
		{
			string path = e.CommandArgument.ToString();
			DisplayAssemblyInformation(path);
		}

		#endregion

		#region Private Methods

		private void DisplayAssemblyInformation(string path)
		{
			AssemblyInfo assemblyInfo = new AssemblyInfo(path);
			DisplayAssemblyInformation(assemblyInfo);
		}

		private void DisplayAssemblyInformation(AssemblyInfo assemblyInfo)
		{
			FilenameLabel.Text = Path.GetFileName(assemblyInfo.Path);
			TitleLabel.Text = assemblyInfo.Title;
			DescriptionLabel.Text = assemblyInfo.Description;
			ProductLabel.Text = assemblyInfo.Product;
			CompanyLabel.Text = assemblyInfo.Company;
			CopyrightLabel.Text = assemblyInfo.Copyright;
			TrademarkLabel.Text = assemblyInfo.Trademark;
			AssemblyVersionLabel.Text = assemblyInfo.Version;
			CreatedLabel.Text = assemblyInfo.CreateDate.ToString("dd MMMM yyyy HH:mm");
			LastModifiedLabel.Text = assemblyInfo.LastModifiedDate.ToString("dd MMMM yyyy HH:mm");
			FileSizeLabel.Text = FileUtils.FriendlyFileSize(assemblyInfo.FileSize);
		}

		#endregion
	}

	public class AssemblyInfo
	{
		#region Private Variables

		private string m_Company;
		private string m_Copyright;
		private string m_Description;
		private readonly string m_Path;
		private string m_Product;
		private string m_Title;
		private string m_Trademark;
		private string m_Version;
		private long m_FileSize;
		private DateTime m_CreateDate;
		private DateTime m_LastModifiedDate;

		#endregion

		#region Accessors

		public string Company
		{
			get
			{
				return m_Company;
			}
		}

		public string Copyright
		{
			get
			{
				return m_Copyright;
			}
		}

		public string Description
		{
			get
			{
				return m_Description;
			}
		}

		public string Path
		{
			get
			{
				return m_Path;
			}
		}

		public string Product
		{
			get
			{
				return m_Product;
			}
		}

		public string Title
		{
			get
			{
				return m_Title;
			}
		}

		public string Trademark
		{
			get
			{
				return m_Trademark;
			}
		}

		public string Version
		{
			get
			{
				return m_Version;
			}
		}

		public long FileSize
		{
			get
			{
				return m_FileSize;
			}
		}

		public DateTime CreateDate
		{
			get
			{
				return m_CreateDate;
			}
		}

		public DateTime LastModifiedDate
		{
			get
			{
				return m_LastModifiedDate;
			}
		}

		#endregion

		#region Constructor

		public AssemblyInfo(string path)
		{
			m_Path = path;

			string assemblyFileName = System.IO.Path.GetFileNameWithoutExtension(path);

			Assembly assembly = Assembly.Load(assemblyFileName);

			Setup(assembly);
		}

		public AssemblyInfo(Assembly assembly)
		{
			Setup(assembly);
		}

		#endregion

		#region Helper Methods

		private void Setup(Assembly assembly)
		{
			if (assembly.Location != null)
			{
				FileInfo fi = new FileInfo(assembly.Location);
				m_CreateDate = fi.CreationTime;
				m_LastModifiedDate = fi.LastWriteTime;
				m_FileSize = fi.Length;
			}

			AssemblyName assemblyName = assembly.GetName();

			m_Version = assemblyName.Version.ToString();

			foreach (Attribute attribute in Attribute.GetCustomAttributes(assembly))
			{
				switch (attribute.GetType().ToString())
				{
					case "System.Reflection.AssemblyTitleAttribute":
						{
							AssemblyTitleAttribute attr = (AssemblyTitleAttribute)attribute;
							m_Title = attr.Title;
							break;
						}

					case "System.Reflection.AssemblyDescriptionAttribute":
						{
							AssemblyDescriptionAttribute attr = (AssemblyDescriptionAttribute)attribute;
							m_Description = attr.Description;
							break;
						}

					case "System.Reflection.AssemblyCompanyAttribute":
						{
							AssemblyCompanyAttribute attr = (AssemblyCompanyAttribute)attribute;
							m_Company = attr.Company;
							break;
						}

					case "System.Reflection.AssemblyProductAttribute":
						{
							AssemblyProductAttribute attr = (AssemblyProductAttribute)attribute;
							m_Product = attr.Product;
							break;
						}

					case "System.Reflection.AssemblyCopyrightAttribute":
						{
							AssemblyCopyrightAttribute attr = (AssemblyCopyrightAttribute)attribute;
							m_Copyright = attr.Copyright;
							break;
						}

					case "System.Reflection.AssemblyTrademarkAttribute":
						{
							AssemblyTrademarkAttribute attr = (AssemblyTrademarkAttribute)attribute;
							m_Trademark = attr.Trademark;
							break;
						}
				}
			}
		}

		#endregion
	}
}