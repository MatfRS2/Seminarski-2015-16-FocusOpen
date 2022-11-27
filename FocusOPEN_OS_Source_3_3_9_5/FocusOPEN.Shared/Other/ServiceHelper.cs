using System;
using System.ServiceModel;

namespace FocusOPEN.Shared.Other
{
	// Class for working around the issue with calling WCF services
	// http://nimtug.org/blogs/damien-mcgivern/archive/2009/05/26/wcf-communicationobjectfaultedexception-quot-cannot-be-used-for-communication-because-it-is-in-the-faulted-state-quot-messagesecurityexception-quot-an-error-occurred-when-verifying-security-for-the-message-quot.aspx

	public static class ServiceHelper
	{
		/// <summary>
		/// WCF proxys do not clean up properly if they throw an exception. This method ensures that the service proxy is handled correctly.
		/// Do not call TService.Close() or TService.Abort() within the action lambda.
		/// </summary>
		/// <typeparam name="TService">The type of the service to use</typeparam>
		/// <param name="action">Lambda of the action to performwith the service</param>
		public static void Using<TService>(Action<TService> action) where TService : ICommunicationObject, IDisposable, new()
		{
			var service = new TService();
			bool success = false;
			try
			{
				action(service);
				if (service.State != CommunicationState.Faulted)
				{
					service.Close();
					success = true;
				}
			}
			finally
			{
				if (!success)
				{
					service.Abort();
				}
			}
		}
	}
}