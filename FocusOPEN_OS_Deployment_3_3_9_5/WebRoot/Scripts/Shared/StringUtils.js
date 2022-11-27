﻿/**********************************************************************************************************************
FocusOPEN Digital Asset Manager (TM) 
(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
without specific, written prior permission. Title to copyright in this software and any associated documentation
will at all times remain with copyright holders.

Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
software. 
**********************************************************************************************************************/


function trim(str) {
    if (!str || typeof str != 'string') { return null; }
    return str.replace(/^[\s]+/, '').replace(/[\s]+$/, '').replace(/[\s]{2,}/, ' ');
}