
let token = getCookie("token");
if (token != "" & typeof( token) !== "undefined")
    window.location.href = '/Account/Index/';

$(document).ready(function () {

    $("#signIn").click(function (event) {
        event.preventDefault();
         SignIn();
    });

});

$(document).ready(function () {

    $("#registration").click(function (event) {
        event.preventDefault();
        Registration();
    });

});

function SignIn() {


    let token = getCookie("token");


    if (token != "" & typeof (token) !=="undefined")
         deleteCookie("token");

    let loginModel = {
        Email: $('#email').val(),
        Password: $('#password').val(),
    };

    $.ajax({
        url: '/api/v1/auth/login',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(loginModel),
        success: function (token) {
          
            RedirectToAccount(token);
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}

function RedirectToAccount(token) {
   
    var header = "Bearer " + token;
 
    $.ajax({
        url: '/Account/Index/',
        type: 'GET',
        contentType: "application/json",
        beforeSend: function (xhrObj) {
            xhrObj.setRequestHeader("Authorization", header);
         
        },
        success: function () {

            document.cookie = "token=" + token + ";  path=/; ";
            window.location.href = '/Account/Index/';

        }, error: function(){

            alert("Error");
        }

    });
   
}

function Registration() {

    let registModel = {
        Email: $('#emailRegistration').val(),
        Password: $('#passwordRegistration').val(), 
    };

    $.ajax({
        url: '/api/v1/auth/registration',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify(registModel),
        success: function (response) {

            RedirectToAccount();
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });

}

function deleteCookie(name) {
    setCookie(name, "", {
        'max-age': -1
    })
}

function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1, c.length);
        }
        if (c.indexOf(nameEQ) == 0) {
            return c.substring(nameEQ.length, c.length);
        }
    }
    return null;
}