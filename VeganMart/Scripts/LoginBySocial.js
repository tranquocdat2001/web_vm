$(function () {
    if (window.location.hash && window.location.hash == '#_=_') {
        window.location.hash = '';
    }
});
var strError = 'Login failed please try again';

var googleUser = {};

function initLoginGoogle() {
    gapi.load('auth2', function () {
        // Retrieve the singleton for the GoogleAuth library and set up the client.
        auth2 = gapi.auth2.init({
            client_id: SocialGoogleClientId,
            cookiepolicy: 'single_host_origin',
            'scope': 'https://www.googleapis.com/auth/plus.login https://www.googleapis.com/auth/plus.profile.emails.read'
        });
        auth2.attachClickHandler('customBtn', {}, onSignIn, onSignInFailure);
    });
};
initLoginGoogle();

// Google Sign-in (new)
function onSignIn(googleUser) {
    // Handle successful sign-in
    var userToken = googleUser.getAuthResponse();
    $.ajax({
        type: 'POST',
        url: '/Account/GoogleLoginCallback',
        //contentType: 'application/octet-stream; charset=utf-8',
        data: { id_token: userToken.id_token },
        dataType: "JSON",
        success: function (result) {
            if (result.ErrorCode == 'success') {
                if (SocialReturnUrl != '')
                    location.href = SocialReturnUrl;
                else
                    location.href = location.href;
            }
            else
                alert(strError);
        }
    });
}
function onSignInFailure() {
    // Handle sign-in errors    
    alert(strError);
}