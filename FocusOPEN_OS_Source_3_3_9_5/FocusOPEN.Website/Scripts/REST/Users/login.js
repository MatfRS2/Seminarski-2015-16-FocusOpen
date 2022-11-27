
function login() {

    //login using the user api token
    this.ValidateUserByUserToken();

    if (this.NoErrors()) {

        var user = FocusOPEN.Business.UserManager.RenewSessionAPIToken(this.getUser());
        this.setUser(user);
        var data = { 'sessiontoken': this.getUser().SessionAPIToken };
        this.SetResponseData(data);   
        
    }
}