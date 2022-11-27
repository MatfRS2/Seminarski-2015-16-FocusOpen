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

namespace FocusOPEN.Shared
{
	[Serializable]
	public class ComplexCriteria
	{
		public readonly string Field;
		public readonly object Value;
		public readonly CompareType CompareType;

		public ComplexCriteria(string field, object value, CompareType compareType)
		{
			Field = field;
			Value = value;
			CompareType = compareType;
		}
	}

	public enum CompareType
	{
		Exact,
		LessOrEqualThan,
		LessThan,
		MoreOrEqualThan,
		MoreThan
	}
}