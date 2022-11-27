

function logout() {

    //clear session api token
    FocusOPEN.Business.UserManager.ClearSessionAPIToken(this.getUser());
}