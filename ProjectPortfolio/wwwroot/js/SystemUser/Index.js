window.onload = function () {
    Filter();
};


function CleanEdit() {
    $("#systemUserId").val("");
    $("#nameId").val("");
    $("#surnameId").val("");
    $("#userNameId").val("");
    $("#isEnabledId").prop('checked', true);
}

function Filter() {
    $.get("SystemUser/Filter", function (response) {
        $("#wrapper").html(response);
    });
}