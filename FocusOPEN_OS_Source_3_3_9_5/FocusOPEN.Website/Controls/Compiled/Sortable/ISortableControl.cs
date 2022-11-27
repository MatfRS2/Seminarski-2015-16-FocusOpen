/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using Daydream.Data;

namespace FocusOPEN.Website.Controls
{
	public interface ISortableControl
	{
		string DefaultSortExpression { get; set; }
		bool DefaultSortAscending { get; set; }

		string SortExpression { get; set; }
		bool SortAscending { get; set; }

		void UpdateSortExpression(string sortExpression);
		List<ISortExpression> GetSortExpressions();

		bool Visible { get; set; }
		object DataSource { get; set; }
		void DataBind();
	}
}