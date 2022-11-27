/**********************************************************************************************************************
FocusOPEN Digital Asset Manager (TM) 
(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
without specific, written prior permission. Title to copyright in this software and any associated documentation
will at all times remain with copyright holders.

Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
software. 
**********************************************************************************************************************/

LoadScriptFile('~\\Shared\\StringUtils.js');

function BaseAPI() {

    var namedParameters = {};
    var user = FocusOPEN.Data.User.Empty;
    var response = new FocusOPEN.Business.Scripts.ScriptResponse();
    var request = new FocusOPEN.Business.Scripts.ScriptRequest();
    var functionFile = '';

    //properties
    this.getSessionToken = function() { return request.SessionToken; };
    this.setSessionToken = function(sessionToken) { request.SessionToken = sessionToken; };
   
    this.getCallName = function() { return request.CallName; };
    this.setCallName = function(callName) { request.CallName = callName; response.CallName=callName; };
    
    this.getUser = function() { return user; };
    this.setUser = function(newUser) { user = newUser; response.UserId =(user.IsNull?ToInt32(0):user.UserId); };
    
    this.getNamedParameters = function() { return namedParameters; };
    this.setNamedParameters = function(params) { namedParameters = params; };

    this.getFunctionFile = function() { return functionFile; };
    this.setFunctionFile = function(filePath) { functionFile = filePath; };

    //readonly properties
    this.getResponse = function() { return response; };
    this.getRequest = function() { return request; };
}

//Inherited APIs should override these methods
BaseAPI.prototype.ParseRequest = function() { return; };
BaseAPI.prototype.RequestValidation = function() { return; };
BaseAPI.prototype.ProcessRequest = function() { this.SetResponseError(-7); return this.Results(); };
BaseAPI.prototype.SessionToken = function() { return ''; };
BaseAPI.prototype.UserToken = function() { return ''; };
BaseAPI.prototype.LoadRequestData = function() { return; };

//Initialise data by loading up request values
BaseAPI.prototype.InitialiseData = function() {

    this.getResponse().Format = this.DefaultResponseFormat();

    var responseId = System.Guid.NewGuid();
    this.getResponse().ResponseId = responseId;
    this.getRequest().ResponseId = responseId;

    var requestId = System.Guid.NewGuid();
    this.getResponse().RequestId = requestId;
    this.getRequest().RequestId = requestId;
    
    this.getRequest().HttpMethod = CurrentHttpRequest().HttpMethod;
    this.getRequest().IpAddress = CurrentIPAddress();
    this.getRequest().SessionToken = this.SessionToken();
    this.LoadRequestData();
};


/*********** Standard utility functions for APIs ********************/
BaseAPI.prototype.ScriptsPath = function() { return FocusOPEN.Business.Scripts.ScriptEngine.ScriptsPath; };
BaseAPI.prototype.QueryString = function(key) { return CurrentHttpRequest().QueryString[key]; };
BaseAPI.prototype.RequestPath = function() { return CurrentHttpRequest().Path; };


/*********** Generic validation functions for APIs ******************/

//validates that the verb required matches the HTTP method
BaseAPI.prototype.ValidateHttpMethod = function(callVerbRequired) {

   if(callVerbRequired)
   {
        if (callVerbRequired.toUpperCase() != this.getRequest().HttpMethod.toUpperCase())
        {
            //verb doesn't match so return
            //appropriate error code
            switch(callVerbRequired.toUpperCase())
            {
                case 'GET':
                    this.SetResponseError(-1); //get required
                case 'POST':
                    this.SetResponseError(-2); //post required               
                default:
                    this.SetResponseError(-7); //unexpected verb
            }
        }
   }
};

//validates the user by Session API Token and assign them to the current user if valid
BaseAPI.prototype.ValidateUserBySessionToken = function() {

    this.GetUserBySessionToken();
    if (this.getUser().IsNull) {
        //invalid session token
        this.SetResponseError(-4);
    }
};


//validates the Response Format requested and assigns it to the current Response object
BaseAPI.prototype.ValidateResponseFormat = function(responseFormat) {

    if (responseFormat) {
        switch (responseFormat.toUpperCase()) {
            case 'XML':
                this.getResponse().Format = FocusOPEN.Business.Scripts.ResponseFormat.XML;
                break;
            case 'JSON':
                this.getResponse().Format = FocusOPEN.Business.Scripts.ResponseFormat.JSON;
                break;
            default:
                this.SetResponseError(-3); //unrecognised response format
        }
    }
};

//validates that the current user has access to an asset
BaseAPI.prototype.ValidateUserAccessToAsset = function(asset) {

    if (FocusOPEN.Business.AssetManager.GetAssetStatusForUser(asset, this.getUser()) != FocusOPEN.Shared.AssetStatus.Available) {
        this.SetResponseError(-6); //asset is not available for user
    }
};


//validate that users are able to make the requested call
BaseAPI.prototype.ValidateUserCanMakeCall = function() {

    if (this.getUser().IsNull) {
        this.SetResponseError(-5); //empty users cannot issue a call
    }

}


/************************* Base API Methods ******************************/

//function calls specified function using the named parameters object
//which should have been set previous to the call
BaseAPI.prototype.CallUsingNamedParameters = function(functionName, context) {

    var fn = eval('return ' + functionName);
    var fnString = fn.toString();
    
    var params = fnString.substring(fnString.indexOf('(') + 1, fnString.indexOf(')'));
    var args = [];
    if (params) {
        var arrParams = params.split(',');
        for (i = 0; i < arrParams.length; i++) {
            if (trim(arrParams[i]).length > 0) {
                args.push(eval('api.getNamedParameters().' + trim(arrParams[i]).toLowerCase()));
            }
        }
    }

    return fn.apply(context, args);
};


/// Gets a User object by session api token providing their session has not expired
BaseAPI.prototype.GetUserBySessionToken = function() {

    var user = FocusOPEN.Data.User.GetBySessionAPIToken(this.SessionToken());
    if (!user.IsNull) {
        //check that user's session is still valid
        if (!FocusOPEN.Business.UserManager.APISessionIsValid(user)) {
            //session has expired so pass empty user
            user = FocusOPEN.Data.User.Empty;
        }
    }

    this.setUser(user);
};


/// Gets a User object by user api token
BaseAPI.prototype.GetUserByUserToken = function() {

    var user = FocusOPEN.Data.User.GetByUserAPIToken(this.UserToken());
    this.setUser(user);

};

/// Sets the appropriate script response for an error
BaseAPI.prototype.SetResponseError = function(errorCode) {
    this.getResponse().Error = FocusOPEN.Business.Scripts.ScriptErrorFactory.Instance.LookupError(ToInt32(errorCode));
};


/// Sets a data in the response object for a successful call
BaseAPI.prototype.SetResponseData = function(data) {

    for (var key in data) {
        if (data.hasOwnProperty(key)) {
            this.getResponse().Data.Add(key, data[key]);
        }
    }

};

//creates named parameters collection from the querystring
BaseAPI.prototype.SetNamedParametersFromRequest = function() {

    var parameters = {};
    var requestData = ConvertDictionaryToList(this.getRequest().Data);

    for (var i = 0; i < requestData.Count; i++) {
    
        var key = requestData[ToInt32(i)].Key;
        parameters[key.toLowerCase()] = requestData[ToInt32(i)].Value;   
    }
    this.setNamedParameters(parameters);

};


//returns boolean indicating whether response error has been set
BaseAPI.prototype.NoErrors = function() {
    return (this.getResponse().Error.Code == 0);
};

//creates a new result object to return the data to the script engine
BaseAPI.prototype.Results = function() {

    var result = new FocusOPEN.Business.Scripts.ScriptResult();
    result.Response = this.getResponse();
    result.Request = this.getRequest();
    return result;
};


//uses querystring to initially assign the response format
BaseAPI.prototype.DefaultResponseFormat = function() {

    var responseFormat = this.QueryString('ResponseFormat');

    if (responseFormat && responseFormat.toUpperCase() == 'XML') {
        return FocusOPEN.Business.Scripts.ResponseFormat.XML;
    }

    return FocusOPEN.Business.Scripts.ResponseFormat.JSON;
};

