using System.Collections.Generic;
using Daydream.Data;

namespace FocusOPEN.Website.Controls
{
	internal class SortExpressionParser
	{
		/// <summary>
		/// Gets the sort expression
		/// </summary>
		public string SortExpression { get; private set; }

		/// <summary>
		/// Gets a boolean value specifying whether we need to sort ascending
		/// </summary>
		public bool SortAscending { get; private set; }

		/// <summary>
		/// Creates a new instance of the sort expression parser
		/// </summary>
		/// <param name="sortExpression">The current sort expression</param>
		/// <param name="sortAscending">The current sort direction</param>
		public SortExpressionParser(string sortExpression, bool sortAscending)
		{
			SortExpression = sortExpression;
			SortAscending = sortAscending;
		}

		/// <summary>
		/// Updates the sort expression and direction
		/// </summary>
		/// <param name="sortExpression">The new sort expression</param>
		public void UpdateSortExpression(string sortExpression)
		{
			if (sortExpression == SortExpression)
			{
				SortAscending = !SortAscending;
			}
			else
			{
				SortAscending = true;
			}

			SortExpression = sortExpression;
		}

		/// <summary>
		/// Gets the current sort expression list
		/// </summary>
		public List<ISortExpression> GetSortExpressions()
		{
			List<ISortExpression> sortExpressions = new List<ISortExpression>();

			if (SortExpression != string.Empty)
			{
				// Don't split the sort expression when it contains "dbo.", as this means it is a function
				// and might have be taking multiple values.  This still needs improvement though, as it would
				// break if we're mixing sort expressions to use a function and a column name.
				// For example: dbo.MyFunction(Col1,Col2),Col3 wouldn't work correctly.
				string[] array = (SortExpression.Contains("dbo.")) ? new[] { SortExpression } : SortExpression.Split(',');

				foreach (string sortExpression in array)
				{
					if (SortAscending)
					{
						sortExpressions.Add(new AscendingSort(sortExpression));
					}
					else
					{
						sortExpressions.Add(new DescendingSort(sortExpression));
					}
				}
			}

			return sortExpressions;
		}
	}
}