/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public class TextExtractor : ITextExtractor
	{
		#region Private Variables

		private string m_Prefix = string.Empty;
		private string m_Suffix = string.Empty;

		#endregion

		#region Accessors

		public string Prefix
		{
			get
			{
				return m_Prefix;
			}
			set
			{
				m_Prefix = value;
			}
		}

		public string Suffix
		{
			get
			{
				return m_Suffix;
			}
			set
			{
				m_Suffix = value;
			}
		}

		#endregion

		#region ITextExtractor Implementation

		public string GetText(object o)
		{
			return string.Concat(Prefix, o.ToString(), Suffix);
		}

		#endregion

		#region Constructor

		public TextExtractor(string prefix, string suffix)
		{
			Prefix = prefix;
			Suffix = suffix;
		}

		#endregion

	}
}
