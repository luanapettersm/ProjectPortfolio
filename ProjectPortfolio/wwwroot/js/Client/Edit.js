
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

function CleanEdit() {
    $("#clientId").val("");
    $("#cpfId").prop('checked', true);
    $("#clientInfoId").mask("999.999.99-99");
    $("#clientInfoId").val("");
    $("#nameId").val("");
    $("#surnameId").val("");
    $("#surnameId").prop('disabled', false);
    $("#nameId").val("");
    $("#phoneNumberId").val("");
    $("#mailId").val("");
    $("#zipCodeId").val("");
    $("#addressId").val("");
    $("#cityId").val("");
    $("#stateId").val("");
    $("#isEnabledId").prop('checked', true);
}

function Close() {
    $("#wrapper-edit").html("");
}

function Save() {
    var fd = $("#clientFormId").serializeArray();
    if ($("#cpfId").is(":checked")) {
        fd.push({ name: 'client.CPF', value: $("#clientInfoId").val() });
    } else {
        fd.push({ name: 'client.CNPJ', value: $("#clientInfoId").val() });
    } 

    $.post(`/Client/Save`, fd)
        .done(function (response) {
            $('#wrapper-edit').html("");
            AlertSaveSuccess();
            $('#table').DataTable().ajax.reload();
        }).fail(function (response) {
            AlertSaveError(response);
        });

}