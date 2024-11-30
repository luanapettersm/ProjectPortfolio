function Authentication() {
    var fd = $("#loginFormId").serializeArray();

    $.post(`/Login/Authentication`, fd)
        .done(function (response) {
            window.location.replace("/Home");
        });
}