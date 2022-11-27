/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.ComponentModel;

namespace FocusOPEN.Shared
{
    public enum SelectableMetadataType
    {
        [Description("Drop-down")]
        DropDown = 1,

        [Description("Combo Box")]
        ComboBox = 2,

        [Description("Radio Buttons")]
        RadioButtons = 3,

        [Description("Checkboxes")]
        Checkboxes = 4,

        [Description("Preset Text Area")]
        PresetTextArea = 5,
    }
    public enum SelectableMetadataSortType
    {
        [Description("Sort alphanumerically")]
        AlphaNumeric = 1,

        [Description("Sort order as designed")]
        AsDesigned = 2,
    }

    public enum SelectableMetadataOrderType
    {
        [Description("Order by row")]
        ByRow = 1,

        [Description("Order by column")]
        ByColumn = 2,
    }
}