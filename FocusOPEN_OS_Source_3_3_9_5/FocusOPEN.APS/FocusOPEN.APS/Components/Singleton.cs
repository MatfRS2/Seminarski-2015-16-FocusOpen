/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Runtime.Serialization;

namespace FocusOPEN.APS
{
	[DataContract]
	public class Singleton<T> where T : new()
	{
		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		public static T Instance
		{
			get
			{
				// Thread safe, lazy implementation of the singleton pattern.
				// See http://www.yoda.arachsys.com/csharp/singleton.html
				// for the full story.
				return SingletonInternal.instance;
			}
		}

		private class SingletonInternal
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit.
			static SingletonInternal()
			{
			}

			internal static readonly T instance = new T();
		}
	}
}