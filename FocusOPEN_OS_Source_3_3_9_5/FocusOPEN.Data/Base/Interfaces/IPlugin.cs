/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace Daydream.Data
{
	public interface IPlugin
	{
		string Name { get; set; }
		decimal PluginVersion { get; set; }
		string RegistrationKey { get; set; }
		bool RequireAllResourceFiles { get; set; }
		bool ForcePreviewFormat { get; set; }
		string[] FileExtensions { get; set; }
		IPluginContext[] ContextItems { get; set; }
		decimal SchemaVersion { get; set; }
		bool Disabled { get; set; }
	}
}