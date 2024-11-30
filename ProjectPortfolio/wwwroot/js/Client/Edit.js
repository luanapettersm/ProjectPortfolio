
function ChangePeople(bool) {
    if (bool) {
        $("#clientInfoId").val("");
        $("#clientInfoId").mask("999.999.99-99");
    } else {
        $("#clientInfoId").val("");
        $("#clientInfoId").mask("99.999.999/9999-99");
    }
}

function Close() {
    $("#wrapper-edit").html("");
}

function Save() {
    let fd = $("#clientFormId").serializeArray();
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