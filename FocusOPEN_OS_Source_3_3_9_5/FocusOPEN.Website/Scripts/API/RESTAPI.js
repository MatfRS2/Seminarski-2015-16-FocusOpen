/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/


//RESTAPI inherits BaseAPI
function RESTAPI() 
{
    //ensure proper instantiation of the api
    if (!(this instanceof arguments.callee))
        return new RESTAPI();

    BaseAPI.call();
}

RESTAPI.prototype = new BaseAPI();
RESTAPI.prototype.constructor = RESTAPI;


/*********** Base API Implementations **************************/

RESTAPI.prototype.ParseRequest = function() {

    //determine the function to use from the request string
    var tokens = this.RequestPath().split('.');

    if (tokens.length >= 4) {

        //get the requested function
        this.setCallName(tokens[tokens.length - 2]);

        //get the requested subfolder
        var apiCallSubFolder = System.IO.Path.Combine(this.ScriptsPath(), 'REST');

        for (var i = 2; i < (tokens.length - 2); i++) {
            apiCallSubFolder = System.IO.Path.Combine(apiCallSubFolder, tokens[i]);
        }

        //check folder exists
        if (System.IO.Directory.Exists(apiCallSubFolder)) {

            //get the requested function file
            this.setFunctionFile(System.IO.Path.Combine(apiCallSubFolder, this.getCallName() + '.js'));

            //check script file exists
            if (!System.IO.File.Exists(this.getFunctionFile())) {
                //return generic call error as requested script file was invalid
                this.SetResponseError(-7);
            }

        } else {
            //generic call error as folder was invalid
            this.SetResponseError(-7);
        }
    } else {
        //generic call error as request was invalid format
        this.SetResponseError(-7);
    }

};


//validation for all REST API calls
RESTAPI.prototype.RequestValidation = function() {

    if (this.NoErrors()) {

        //validate ip address
        this.ValidateIPAddress();

        //validate user
        if (this.NoErrors()) {

            //perform generic validation for all calls to REST API
            if (this.getCallName().toUpperCase() != 'LOGIN') {

                //validate session api token for all calls except logins
                this.ValidateUserBySessionToken();

                if (this.NoErrors()) {
               
                    //check if current user is restricted
                    if (this.UserIsRestricted()) {
                        this.SetResponseError(-10000);
                    }                   
                }


                if (this.NoErrors()) {
                    //validate user can make call
                    this.ValidateUserCanMakeCall();
                }
            }
        }

        //validate http method
        if (this.NoErrors()) {
            this.ValidateHttpMethod('GET'); //rest api requires GET only
        }

        //validate response format
        if (this.NoErrors()) {
            this.ValidateResponseFormat(this.QueryString('ResponseFormat'));
        }
    }

};




RESTAPI.prototype.ProcessRequest = function() {

    if (this.NoErrors()) {
        //validated successfully so try and load the function's script file
        if (LoadScriptFile(this.getFunctionFile())) {

            //set the function parameters
            this.SetNamedParametersFromRequest();
            //call function
            this.CallUsingNamedParameters(this.getCallName(), this);

        } else {
            //did not load script file successfully so return generic call error
            this.SetResponseError(-7);
        }

    }
    
    return this.Results();
};


RESTAPI.prototype.LoadRequestData = function() {
    
    //loads the request data from the query string
    var qs = CurrentHttpRequest().QueryString;

    for (var i = 0; i < qs.Count; i++) {
        var key = qs.GetKey(ToInt32(i));
        this.getRequest().Data.Add(key, qs[ToInt32(i)]);      
    }
};


RESTAPI.prototype.SessionToken = function() { return this.QueryString('SessionAPIToken'); };
RESTAPI.prototype.UserToken = function() { return this.QueryString('UserAPIToken'); };

/******************** REST API Validation routines ****************************************/


RESTAPI.prototype.ValidateIPAddress = function() {

    if (FocusOPEN.Business.Scripts.ScriptEngine.APIObeyIpAddressRestrictions) {
        //check ip restrictions are enabled
        if (FocusOPEN.Shared.Settings.IpAddressRestrictionEnabled) {

            var requestIP = this.getRequest().IpAddress;
            if (!FocusOPEN.Business.UserSecurityManager.IsApprovedIpAddress(requestIP)) {
                //ip address is not approved so return error
                this.SetResponseError(-10003);
            }
        }
    }
};


RESTAPI.prototype.ValidateUserByUserToken = function() {

    //check user token is valid
    this.GetUserByUserToken();
    if (!this.getUser().IsNull) {
        //found user by user token
        if (this.UserIsRestricted()) {
            this.SetResponseError(-10000);
        }else if (this.getUser().IsSuspended) {
            this.SetResponseError(-10001); //suspended user
        }else if (this.getUser().IsPasswordExpired && !this.getUser().IsPasswordNonExpiring) {
            this.SetResponseError(-10002); //expired user
        }
    } else {
        this.SetResponseError(-10000); //unknown user
    }

};

RESTAPI.prototype.UserIsRestricted = function() {

    //check that user's account is not restricted 
    var restrict = FocusOPEN.Business.Scripts.ScriptEngine.APIRestrictAccounts;
    
    if (!restrict || restrict.length == 0) {
        //if empty string then nobody is restricted
        return false;
    }

    //search list for user's email
    var emails = restrict.split(',');
    for (var i = 0; i < emails.length; i++) {

        if (emails[i] && emails[i].length > 0) {
            if (emails[i].toLowerCase() == this.getUser().Email.toLowerCase()) {
                //email was found so not restricted
                return false;
            }
        }
    }

    return true;
};