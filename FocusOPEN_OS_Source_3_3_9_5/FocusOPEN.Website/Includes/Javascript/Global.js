$(function() {
	$("table.AutoStripe tr td").css("height", "25px");
	$("table.AutoStripe tr:even").find("td").addClass("TblCell1");
	$("table.AutoStripe tr:odd").find("td").addClass("TblCell2");
	setupDatePicker();
});

function GetAppRoot() {
	if (typeof (APP_ROOT) == 'undefined')
		APP_ROOT = (document.domain == "localhost") ? "/FocusOPEN/" : "/";
	return APP_ROOT;
}

function checkRedirect(page) {
	var key = "bumper_page_viewed";
	if ($.cookie(key) == 1) {
		window.location = page;
		return;
	}
	$.cookie(key, "1")
}

function setupDatePicker() {
    if (typeof (jQuery) != "undefined" && typeof ($.datepicker) != "undefined") {
        $(function() {
            $(".datePicker").datepicker({ showAnim: 'slideDown', dateFormat: 'dd MM yy' });
            $(".datePicker.pastDaysDisallowed").datepicker('option', 'minDate', new Date());
        });
    }
}

function getFileExtension(filename) {
	if (!filename)
		return "";
		
	var index = filename.lastIndexOf(".");

	if (index < 0)
		return filename;

	return filename.substr(index+1);
}

function validateUpload(fileControlName, assetTypeDropDownControlName, assetPathDropDownControlName, allowedExtensions, isPending) {
    var typeControl = document.getElementById(assetTypeDropDownControlName);
    var pathControl = document.getElementById(assetPathDropDownControlName);
	var fileControl = document.getElementById(fileControlName);

	var errormessage = "";
	var pathUpload = false;

	if (typeControl) {
		if (typeControl.options[typeControl.options.selectedIndex].value == "0")
			errormessage += "\n  - No asset type specified";

		if (allowedExtensions)
			allowedExtensions += "zip;";
	}

	if (pathControl) {
	    if (pathControl.value != "#BROWSERUPLOAD#") {
	        pathUpload = true;
	    }
	}


	if (!pathUpload) {
	    if (!fileControl.value) {
	        errormessage += "\n  - No file specified";
	    }
	    else {
	        if (fileControl.value && allowedExtensions) {
	            var path = fileControl.value.toLowerCase();
	            var ext = getFileExtension(path);

	            if (ext) {
	                var pattern = ext + ";";
	                if (!allowedExtensions.match(pattern))
	                    errormessage += "\n  - The extension '" + ext + "' is invalid for the selected asset type";
	            }
	            else {
	                errormessage += "\n  - File has missing extension";
	            }
	        }
	    }
	}
	

	if (errormessage) {
		alert("Unable to upload assets.  Please correct the following errors and try again:\n" + errormessage);
		return false;
	}

	if (isPending) {
		var c = confirm('Replacing the asset will require it to be checked by administrators\nbefore publication.  Are you sure you wish to proceed?');
		if (!c)
			return false;
	}

	if (typeof (jQuery) != "undefined") {
		jQuery("#ProgressBarWrapper").show();
	}

	return true;
}

function showAssetInfo(assetId, returnWin)
{
	var url = GetAppRoot() + "Popups/AssetInfo.aspx?assetId=" + assetId;
	var width = 730;
	var height = 630;

	var leftPosition = (screen.width)?(screen.width-width)/2:100;
	var topPosition = (screen.height)?(screen.height-height)/2:100;
	var settings = 'width='+width+',height='+height+',top='+topPosition+',left='+leftPosition+',scrollbars=yes,location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=yes';	
	var win = window.open(url, 'win_'+assetId, settings);
	
	if (returnWin)
		return win;
}

function showTermsConditions()
{
	var url = GetAppRoot() + "Popups/TermsConditions.aspx";
	
	var width = 730;
	var height = 600;
	var leftPosition = (screen.width)?(screen.width-width)/2:100;
	var topPosition = (screen.height)?(screen.height-height)/2:100;
	
	var settings = 'width='+width+',height='+height+',top='+topPosition+',left='+leftPosition+',scrollbars=yes,location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=yes';	
	
	var win = window.open(url, 'termsconditions', settings);
}

function showToolTip(c, t)
{
	overlib(t, CAPTION, c)
}

function hideToolTip()
{
	return nd();
}

function toggleCheckboxes(container, state)
{
	var e = document.getElementById(container);
	var chks = e.getElementsByTagName("input");
	
	for (var chkindex in chks)
	{
		var chk = chks[chkindex];
		if (chk.type == 'checkbox')
		{
			chk.checked = state;
		}
	}
}

function toggleCheckboxesEnabled(container, enabled)
{
	// Get all of the input elements
	var e = document.getElementById(container);
	var inputs = e.getElementsByTagName("input");
	
	// Internet Explorer puts a 'disabled' attribute on the container
	// table so remove it from there (otherwise, the javascript below
	// won't result in any change in the UI, even though the properties will change)
	// if (e.getElementsByTagName("table"))
	//	e.getElementsByTagName("table")[0].disabled = !enabled;
	
	// Iterate through all of the elements toggling their disabled
	// or checked properties according to the 'enabled' parameter.
	for (var i=0; i < inputs.length; i++)
	{
		var ie = inputs[i];
		
		if (ie.type == 'checkbox')
		{
			if (enabled)
			{
				ie.disabled = false;
			}
			else
			{
				ie.disabled = true;
				ie.checked = false;
			}
		}
	}
}

function toggleFileSize(dd, tbid)
{
	var tb = document.getElementById(tbid);
	tb.disabled = (dd.options.selectedIndex == 0);
}

function setUniqueRadioButton(nameregex, current)
{
	// http://www.codeguru.com/csharp/csharp/cs_controls/custom/article.php/c12371/
	
	re = new RegExp(nameregex);
	for (i = 0; i < document.forms[0].elements.length; i++)
	{
		elm = document.forms[0].elements[i]
		if (elm.type == 'radio')
		{
			if (re.test(elm.name))
			{
				elm.checked = false;
			}
		}
	}
	current.checked = true;
}