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
using System.Collections;

namespace FocusOPEN.Shared
{
	public class JoinableList : ArrayList
	{
		private const string m_DefaultDelimiter = ", ";
		private readonly string m_Delimiter = null;
		private string m_FinalDelimiter = null;
		private readonly ITextExtractor m_TextExtractor = null;

		#region Properties

		public string FinalDelimiter
		{
			get
			{
				if (m_FinalDelimiter == null)
					return m_Delimiter;

				return m_FinalDelimiter;
			}
			set
			{
				m_FinalDelimiter = value;
			}
		}

		#endregion

		#region Various Constructors

		public JoinableList()
			: this(new ArrayList(), m_DefaultDelimiter, new TextExtractor())
		{
		}

		public JoinableList(string delimiter)
			: this(new ArrayList(), delimiter, new TextExtractor())
		{
		}

		public JoinableList(ITextExtractor textExtractor)
			: this(new ArrayList(), m_DefaultDelimiter, textExtractor)
		{
		}

		public JoinableList(string delimiter, ITextExtractor textExtractor)
			: this(new ArrayList() , delimiter, textExtractor)
		{
		}

		public JoinableList(IList list)
			: this(list, m_DefaultDelimiter, new TextExtractor())
		{
		}

		public JoinableList(IList list, string delimiter)
			: this(list, delimiter, new TextExtractor())
		{
		}

		public JoinableList(IList list, ITextExtractor textExtractor)
			: this(list, m_DefaultDelimiter, textExtractor)
		{
		}

		public JoinableList(IList list, string delimiter, ITextExtractor textExtractor)
			: base(list)
		{
			m_Delimiter = delimiter;
			m_TextExtractor = textExtractor;
		}

		#endregion

		public override string ToString()
		{
			int count = 1;
			string result = String.Empty;

			foreach (object o in this)
			{
				result += m_TextExtractor.GetText(o);
				
				if (count < (Count - 1))
				{
					result += m_Delimiter;
				}
				else if (count < Count)
				{
					result += FinalDelimiter;
				}
				
				++count;
			}

			return result;
		}
	}
}