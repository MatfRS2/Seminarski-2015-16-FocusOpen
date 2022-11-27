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

[assembly: AssemblyTitle("FocusOPEN.APS")]
[assembly: AssemblyDescription("FocusOPEN Digital Asset Manager (TM) - Asset Processing Service (Core)")]
[assembly: AssemblyCompany("Daydream Interactive Ltd")]
[assembly: AssemblyProduct("FocusOPEN.APS")]
[assembly: AssemblyCopyright("Copyright (c) Daydream Interactive Ltd. 2011")]
[assembly: AssemblyTrademark("FocusOPEN Digital Asset Manager (TM)")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
	[assembly: AssemblyConfiguration("Release")]
#endif
