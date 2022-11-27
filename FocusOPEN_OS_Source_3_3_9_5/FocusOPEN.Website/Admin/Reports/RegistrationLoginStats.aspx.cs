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
using System.Data.SqlTypes;
using System.Threading;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class RegistrationLoginStats : BaseAdminPage
	{
		#region Handled Events

		protected void Page_Load(object sender, EventArgs e)
		{
			BrandSelectorWrapper.Visible = (BrandManager.IsMultipleBrandMode);
		}

		protected void GenerateReportButton_Click(object sender, EventArgs e)
		{
			InitialPanel.Visible = false;
			ResultsPanel.Visible = true;

			InitialFooterPanel.Visible = false;
			ResultsFooterPanel.Visible = true;

			DisplayLoginRegistrationInformation();
		}

		#endregion

		#region Public Methods

		protected string GetChartHeader()
		{
			string company = (CompanyDropDownList1.SelectedId == 0) ? "All Companies" : CompanyDropDownList1.SelectedItem.Text;
			string brand = (GetBrandName() == string.Empty) ? "All Brands" : GetBrandName();
			string timeframe = TimeFrameDropDownList1.SelectedItem.Text;
			string accrued = (AccrueCheckBox.Checked) ? "(Figures Accrued)" : "(Figures Not Accrued)";

			string header = string.Format("{0}, {1}, {2}, {3}", company, brand, timeframe, accrued);

			return header;
		}

		#endregion

		#region Constructor

		protected RegistrationLoginStats()
		{
			LoginCountAtEnd = 0;
			LoginCountAtStart = 0;
			RegistrationCountAtEnd = 0;
			RegistrationCountAtStart = 0;
		}

		#endregion

		#region Accessors

		protected int RegistrationCountAtStart { get; set; }

		protected int RegistrationCountAtEnd { get; set; }

		protected int LoginCountAtStart { get; set; }

		protected int LoginCountAtEnd { get; set; }

		#endregion

		#region Helper Methods

		/// <summary>
		/// Changes a positive number to a negative number.
		/// Eg. Passing in 10 would return -10.
		/// </summary>
		private static int ConvertToNegative(int number)
		{
			if (number <= 0)
				return number;

			return number - (number * 2);
		}

		/// <summary>
		/// Gets the primary and/or secondary brand name from the dropdown list
		/// </summary>
		/// <returns></returns>
		private string GetBrandName()
		{
			if (RestrictedFilterOptionsPanel.Visible)
			{
				if (BrandDropDownList1.SelectedId <= 0)
					return string.Empty;

				return BrandDropDownList1.SelectedItem.Text;
			}

			return CurrentUser.PrimaryBrand.Name;
		}

		private int GetIncreaseInRegistrationsPercentage()
		{
			return GetIncreaseInPercentage(RegistrationCountAtStart, RegistrationCountAtEnd);
		}

		private int GetIncreaseInLoginsPercentage()
		{
			return GetIncreaseInPercentage(LoginCountAtStart, LoginCountAtEnd);
		}

		private static int GetIncreaseInPercentage(int countAtStart, int countAtEnd)
		{
			if (countAtStart == 0)
			{
				if (countAtEnd == 0)
					return 0;

				return countAtEnd * 100;
			}

			int difference = countAtEnd - countAtStart;

			if (difference == 0)
				return 0;

			double start = Convert.ToDouble(countAtStart);
			double diff = Convert.ToDouble(difference);
			double percent = Math.Round(diff / start * 100);

			return Convert.ToInt32(percent);
		}

		private string GetRegistrationInfoLabelText()
		{
			int percentage = GetIncreaseInRegistrationsPercentage();

			if (percentage < 0)
			{
				percentage = (percentage * 2);
				return string.Format("<span class=\"Bold\">{0}%</span> decrease in registrations", percentage);
			}
			
			return string.Format("<span class=\"Bold\">{0}%</span> increase in registrations", percentage);				
		}

		private string GetLoginsInfoLabelText()
		{
			int percentage = GetIncreaseInLoginsPercentage();

			if (percentage < 0)
			{
				percentage = (percentage * 2);
				return string.Format("<span class=\"Bold\">{0}%</span> decrease in logins", percentage);
			}

			return string.Format("<span class=\"Bold\">{0}%</span> increase in logins", percentage);
		}

		/// <summary>
		/// Gets the user finder, with criteria set from form values
		/// </summary>
		private UserFinder GetUserFinderFromForm()
		{
			UserFinder userFinder = new UserFinder();

			// Only add restrictions if the options are visible
			if (RestrictedFilterOptionsPanel.Visible)
			{
				string companyName = string.Empty;

				if (CompanyDropDownList1.SelectedId > 0)
					companyName = CompanyCache.Instance.GetById(CompanyDropDownList1.SelectedId).Name;

				userFinder.CompanyName = companyName;
				userFinder.PrimaryBrandId = BrandDropDownList1.SelectedId;
			}

			// Brand admins should only use their own users
			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
				userFinder.PrimaryBrandId = CurrentUser.PrimaryBrandId;

			return userFinder;
		}
		
		/// <summary>
		/// Gets the user id finder, with the criteria set from the form values
		/// </summary>
		private UserIdFinder GetUserIdFinderFromForm()
		{
			UserIdFinder userIdFinder = new UserIdFinder();
			
			// Only add restrictions if the options are visible
			if (RestrictedFilterOptionsPanel.Visible)
			{
				string companyName = string.Empty;

				if (CompanyDropDownList1.SelectedId > 0)
					companyName = CompanyCache.Instance.GetById(CompanyDropDownList1.SelectedId).Name;

				userIdFinder.CompanyName = companyName;
				userIdFinder.BrandId = null;

				if (BrandDropDownList1.SelectedId > 0)
					userIdFinder.BrandId = BrandDropDownList1.SelectedId;
			}

			// Brand admins should only use their own users
			if (CurrentUser.UserRole == UserRole.BrandAdministrator)
				userIdFinder.BrandId = CurrentUser.PrimaryBrandId;

			return userIdFinder;
		}

		/// <summary>
		/// Gets the count of users that registered between the selected dates
		/// based on the criteria chosen in the form
		/// </summary>
		private int GetUserCountRegisteredBetweenDates(DateTime? startDate, DateTime? endDate)
		{
			UserFinder userFinder = GetUserFinderFromForm();
			userFinder.RegisterDateRange = DateRange.Get(startDate, endDate);
			return Data.User.GetCount(userFinder);
		}

		/// <summary>
		/// Gets the count of users that logged in between the selected dates
		/// based on the criteria chosen in the form
		/// </summary>
		private int GetLoginCountBetweenDates(DateTime? startDate, DateTime? endDate)
		{
			AuditUserHistoryFinder finder = new AuditUserHistoryFinder {UserIdFinder = GetUserIdFinderFromForm(), SuccessfulLogins = true, StartDate = startDate, EndDate = endDate};
			return AuditUserHistory.GetCount(finder);
		}

		/// <summary>
		/// Adds data points for each month, for the selected number of months
		/// </summary>
		/// <param name="numberOfMonthsBack">The number of months back from the current month for which data points should be added</param>
		/// <param name="dataPoints">The datapoints collection to which new datapoints should be added</param>
		private static void AddDataPoints(int numberOfMonthsBack, List<KeyValuePair<string, DateRange>> dataPoints)
		{
			numberOfMonthsBack = ConvertToNegative(numberOfMonthsBack);
			DateTime start = DateTime.Now.AddMonths(numberOfMonthsBack);
			DateTime end = DateTime.Now;

			for (DateTime date = start; date <= end; date = date.AddMonths(1))
			{
				DateTime startDate = new DateTime(date.Year, date.Month, 1);
				DateTime endDate = startDate.AddMonths(1);
				string monthName = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(startDate.Month);

				foreach (KeyValuePair<string, DateRange> kvp in dataPoints)
				{
					if (kvp.Key.ToLower() == monthName.ToLower())
					{
						monthName += Environment.NewLine + startDate.Year;
						break;
					}
				}

				DateRange dateRange = DateRange.Get(startDate, endDate);
				dataPoints.Add(new KeyValuePair<string, DateRange>(monthName, dateRange));
			}
		}

		/// <summary>
		/// Sets the registration and login counts, and adds the relevant data points
		/// to the datapoints collection for the number of months back
		/// </summary>
		private void SetMonthTimeFrame(int numberOfMonthsBack, List<KeyValuePair<string, DateRange>> dataPoints)
		{
			numberOfMonthsBack = ConvertToNegative(numberOfMonthsBack);

			DateTime date = DateTime.Now.AddMonths(numberOfMonthsBack);
			date = new DateTime(date.Year, date.Month, 1);

			RegistrationCountAtStart = GetUserCountRegisteredBetweenDates(null, date);
			RegistrationCountAtEnd = GetUserCountRegisteredBetweenDates(null, null);

			LoginCountAtStart = GetLoginCountBetweenDates(null, date);
			LoginCountAtEnd = GetLoginCountBetweenDates(null, null);

			AddDataPoints(numberOfMonthsBack, dataPoints);
		}

		private static int GetMonthDifference(DateTime startDate, DateTime endDate)
		{
			int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
			return Math.Abs(monthsApart);
		}

		private void DisplayLoginRegistrationInformation()
		{
			List<KeyValuePair<string, DateRange>> datapoints = new List<KeyValuePair<string, DateRange>>();

			switch (EnumUtils.GetEnumFromValue<TimeFrame>(TimeFrameDropDownList1.SelectedId))
			{
				case (TimeFrame.SinceUpload): // Since System Release

					// Get the first login date
					DateTime systemLaunchDate = GetFirstLoginDate();

					// No logins yet, so just use last month
					if (systemLaunchDate == DateTime.MinValue)
						systemLaunchDate = DateTime.Now.AddMonths(-1);

					RegistrationCountAtStart = GetUserCountRegisteredBetweenDates(null, systemLaunchDate);
					RegistrationCountAtEnd = GetUserCountRegisteredBetweenDates(null, null);

					LoginCountAtStart = 0;
					LoginCountAtEnd = GetLoginCountBetweenDates(null, null);

					// Get number of month between the dates
					int numberOfMonths = GetMonthDifference(systemLaunchDate, DateTime.Now);

					// Use day intervals if less than one month
					if (numberOfMonths <= 1)
					{
						// Create a date, ignoring the time
						DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

						TimeSpan ts = now - systemLaunchDate;
						int days = Convert.ToInt32(ts.TotalDays);
						int daysBack = ConvertToNegative(days);

						if (daysBack == 0)
							daysBack = -31;

						// Get our timeframe start and end dates
						DateTime startDate = now.AddDays(daysBack);
						DateTime endDate = now;

						// Add each day between the start and end dates to the datapoints
						for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
						{
							string label = dt.Day.ToString();
							DateRange dateRange = DateRange.Get(dt, dt.AddDays(1));

							datapoints.Add(new KeyValuePair<string, DateRange>(label, dateRange));
						}
					}
					else
					{
						// Otherwise, more than a month has passed since launch
						// so use month intervals for the chart
						SetMonthTimeFrame(numberOfMonths-1, datapoints);
					}

					break;

				case (TimeFrame.LastMonth):

					DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

					RegistrationCountAtStart = GetUserCountRegisteredBetweenDates(null, date);
					RegistrationCountAtEnd = GetUserCountRegisteredBetweenDates(null, null);

					LoginCountAtStart = GetLoginCountBetweenDates(null, date);
					LoginCountAtEnd = GetLoginCountBetweenDates(null, null);

					// Get the number of days in this month and year
					int numberOfDaysInMonth = Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInMonth(date.Year, date.Month);

					for (int i = 0; i < numberOfDaysInMonth; i++)
					{
						// We're using a zero based index, so the day number is actually + 1
						int dayNumber = i + 1;
						
						// Default the label to the day number
						string label = dayNumber.ToString();

						if (dayNumber == 1)
						{
							// The first day should be prefixed with the month name
							label = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month) + " " + dayNumber;
						}
						else if (dayNumber < numberOfDaysInMonth)
						{
							// Only show every 4th day label
							// If the day doesn't devise by 4, then set it to a blank space
							if (dayNumber % 4 != 0)
								label = " ";
						}

						// Our date range is only one day
						DateTime startDate = date.AddDays(i);
						DateTime endDate = startDate.AddDays(1);
						DateRange dateRange = DateRange.Get(startDate, endDate);

						// Add the label and date range to the datapoints
						datapoints.Add(new KeyValuePair<string, DateRange>(label, dateRange));
					}

					break;

				case (TimeFrame.Last3Months):

					SetMonthTimeFrame(2, datapoints);
					break;

				case (TimeFrame.Last6Months):

					SetMonthTimeFrame(5, datapoints);
					break;

				case (TimeFrame.Last12Months):

					SetMonthTimeFrame(11, datapoints);
					break;
			}

			int lastLoginCount = 0;
			int lastRegistrationCount = 0;

			datapoints.Reverse();

			foreach (KeyValuePair<string, DateRange> datapoint in datapoints)
			{
				int registrations = GetUserCountRegisteredBetweenDates(datapoint.Value.StartDate, datapoint.Value.EndDate);
				int logins = GetLoginCountBetweenDates(datapoint.Value.StartDate, datapoint.Value.EndDate);

				// Add the previous registration and login counts if the 'accrue' option is checked
				if (AccrueCheckBox.Checked)
				{
					registrations += lastRegistrationCount;
					logins += lastLoginCount;
				}

				// Add the values to the charts
				RegistrationChart.DataPoints.Add(new KeyValuePair<string, int>(datapoint.Key, registrations));
				LoginChart.DataPoints.Add(new KeyValuePair<string, int>(datapoint.Key, logins));

				// Store the current counts, in case we need to accrue
				lastRegistrationCount = registrations;
				lastLoginCount = logins;
			}

			// Initialise the charts
			RegistrationChart.Initialise();
			LoginChart.Initialise();

			RegistrationCountPercentageRiseLabel.Text = string.Format("{0}%", GetIncreaseInRegistrationsPercentage());
			LoginCountPercentageRiseLabel.Text = string.Format("{0}%", GetIncreaseInLoginsPercentage());

			RegistrationInfoTopLabel.Text = RegistrationInfoBottomLabel.Text = GetRegistrationInfoLabelText();
			LoginInfoTopLabel.Text = LoginInfoBottomLabel.Text = GetLoginsInfoLabelText();
		}

		/// <summary>
		/// Gets the first login date.  If there are no logins, DateTime.MinValue is returned.
		/// </summary>
		private static DateTime GetFirstLoginDate()
		{
			AuditUserHistoryFinder finder = new AuditUserHistoryFinder {AuditUserAction = AuditUserAction.UserLogin};
			var auh = AuditUserHistory.FindOne(finder);
			return (auh.IsNull) ? DateTime.MinValue : auh.Date;
		}

		#endregion
	}
}