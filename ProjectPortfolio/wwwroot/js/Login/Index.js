function Login() {
    var fd = $("#loginFormId").serializeArray();

    $.post(`/Login/Login`, fd)
        .done(function (response) {
            window.location.replace("/Home");
        });
}