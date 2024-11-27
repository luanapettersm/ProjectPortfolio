function Close() {
    $("#wrapper-edit").html("");
}

function Save() {
    var fd = $("#systemUserFormId").serializeArray();

    $.post(`/SystemUser/Save`, fd)
        .done(function (response) {
            $('#wrapper-edit').html("");
            AlertSaveSuccess();
            Filter();
        }).fail(function (response) {
            AlertSaveError(response);
        });
}

function AlertSaveSuccess() {
    $("#wrapper-alert").html(`
         <div class="alert alert-success" role="alert">
            Salvo com sucesso!
        </div>
    `)
    setTimeout(() => {
        $("#wrapper-alert").html("");
    }, 3000);
}

function AlertSaveError(error) {
    $("#wrapper-alert").html(`
         <div class="alert alert-danger" role="alert">
            Erro ao salvar - ${error}
        </div>
    `)
    setTimeout(() => {
        $("#wrapper-alert").html("");
    }, 3000);
}