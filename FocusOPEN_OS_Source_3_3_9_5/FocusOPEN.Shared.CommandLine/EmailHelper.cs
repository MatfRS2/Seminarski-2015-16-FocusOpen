using FocusOPEN.Data;

namespace FocusOPEN.Shared.CommandLine
{
	public static class EmailHelper
	{
		/// <summary>
		/// Gets the asset expiry email with default properties set
		/// </summary>
		public static Email GetEmailTemplate(User user, string template, string subject)
		{
			// The brand to be used for this email
			var brand = user.PrimaryBrand;

			// Create the email template
			Email email = Email.CreateFromTemplate(template);

			// Set from name and email
			email.FromName = brand.ApplicationName;
			email.FromEmail = brand.EmailFrom;

			// Set default from email if none define
			if (!StringUtils.IsEmail(email.FromEmail))
				email.FromEmail = "do-not-reply@digitalassetmanager.com";

			// Set default from name if none defined
			if (StringUtils.IsBlank(email.FromName))
				email.FromName = email.FromEmail;

			// Prefix brand application name onto subject
			email.Subject = brand.ApplicationName + ": " + subject;

			// Get website
			var websiteUrl = brand.WebsiteUrl.EnsureEndsWith("/");

			// Add email params
			email.AddBodyParameter("appPath", websiteUrl);
			email.AddBodyParameter("first-name", user.FirstName);
			email.AddBodyParameter("last-name", user.LastName);
			email.AddBodyParameter("orgName", brand.OrganisationName);
			email.AddBodyParameter("appName", brand.ApplicationName);

			return email;
		}
	}
}
