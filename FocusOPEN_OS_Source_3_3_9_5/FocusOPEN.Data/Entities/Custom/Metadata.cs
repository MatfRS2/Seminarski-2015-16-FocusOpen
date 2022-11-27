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

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Metadata.
	/// </summary>
	public partial class Metadata
	{
		public Metadata Parent
		{
			get
			{
				return (ParentMetadataId.HasValue) ? MetadataCache.Instance.GetById(ParentMetadataId.Value) : Empty;
			}
		}

		public virtual IList<Metadata> Children
		{
			get
			{
				return MetadataCache.Instance.GetList(MetadataId.GetValueOrDefault());
			}
		}

		public List<int> GetIdList()
		{
			return GetIdList(this);
		}

		private static List<int> GetIdList(Metadata metadata)
		{
			List<Int32> list = new List<int> { metadata.MetadataId.GetValueOrDefault() };

			foreach (Metadata child in metadata.Children)
				list.AddRange(GetIdList(child));

			return list;
		}
	}
}
