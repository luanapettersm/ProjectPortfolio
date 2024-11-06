$(document).ready(function () {
    alert("sss");
});

function ChangePeople(bool) {
    if (bool) {
        $("#clientInfoId").val("");
        $("#clientInfoId").mask("999.999.99-99");
        $("#surnameId").prop('disabled', false);
    } else {
        $("#clientInfoId").val("");
        $("#clientInfoId").mask("99.999.999/9999-99");
        $("#surnameId").val("");
        $("#surnameId").prop('disabled', true);
    }
} 