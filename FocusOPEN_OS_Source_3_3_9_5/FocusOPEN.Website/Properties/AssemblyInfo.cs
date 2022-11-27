/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Reflection;
using System.Runtime.InteropServices;
using log4net.Config;

[assembly: AssemblyTitle("FocusOPEN.Website")]
[assembly: AssemblyDescription("FocusOPEN Digital Asset Manager (TM)")]
[assembly: AssemblyCompany("Daydream Interactive Limited")]
[assembly: AssemblyProduct("FocusOPEN.Website")]
[assembly: AssemblyCopyright("Daydream Interactive Limited. 2011")]
[assembly: AssemblyTrademark("FocusOPEN Digital Asset Manager (TM)")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("3.3.9.5")]
[assembly: AssemblyFileVersion("3.3.9.5")]
[assembly: XmlConfigurator(ConfigFile = "Config/Log4net.config", Watch = true)]

#if DEBUG
	[assembly: AssemblyConfiguration("Debug")]
#else
	[assembly: AssemblyConfiguration("Release")]
#endif
