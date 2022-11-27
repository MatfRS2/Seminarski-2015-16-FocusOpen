using System;
using System.IO;
using System.Reflection;

namespace FocusOPEN.APS
{
	internal class AssemblyInfo
	{
		#region Private Variables

		private readonly string m_Path;
		private string m_Company;
		private string m_Copyright;
		private DateTime m_CreateDate;
		private string m_Description;
		private long m_FileSize;
		private DateTime m_LastModifiedDate;
		private string m_Product;
		private string m_Title;
		private string m_Trademark;
		private Version m_Version;

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

		public Version Version
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

			if (string.IsNullOrEmpty(assemblyFileName))
				return;

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
			if (assembly == null)
				return;

			if (!string.IsNullOrEmpty(assembly.Location))
			{
				FileInfo fi = new FileInfo(assembly.Location);
				m_CreateDate = fi.CreationTime;
				m_LastModifiedDate = fi.LastWriteTime;
				m_FileSize = fi.Length;
			}

			AssemblyName assemblyName = assembly.GetName();

			m_Version = assemblyName.Version;

			foreach (Attribute attribute in Attribute.GetCustomAttributes(assembly))
			{
				switch (attribute.GetType().ToString())
				{
					case "System.Reflection.AssemblyTitleAttribute":
						{
							AssemblyTitleAttribute attr = (AssemblyTitleAttribute) attribute;
							m_Title = attr.Title;
							break;
						}

					case "System.Reflection.AssemblyDescriptionAttribute":
						{
							AssemblyDescriptionAttribute attr = (AssemblyDescriptionAttribute) attribute;
							m_Description = attr.Description;
							break;
						}

					case "System.Reflection.AssemblyCompanyAttribute":
						{
							AssemblyCompanyAttribute attr = (AssemblyCompanyAttribute) attribute;
							m_Company = attr.Company;
							break;
						}

					case "System.Reflection.AssemblyProductAttribute":
						{
							AssemblyProductAttribute attr = (AssemblyProductAttribute) attribute;
							m_Product = attr.Product;
							break;
						}

					case "System.Reflection.AssemblyCopyrightAttribute":
						{
							AssemblyCopyrightAttribute attr = (AssemblyCopyrightAttribute) attribute;
							m_Copyright = attr.Copyright;
							break;
						}

					case "System.Reflection.AssemblyTrademarkAttribute":
						{
							AssemblyTrademarkAttribute attr = (AssemblyTrademarkAttribute) attribute;
							m_Trademark = attr.Trademark;
							break;
						}
				}
			}
		}

		#endregion
	}
}