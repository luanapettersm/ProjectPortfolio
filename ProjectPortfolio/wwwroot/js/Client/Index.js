window.onload = function () {
    $("#clientInfoId").mask("999.999.99-99");
    $("#phoneNumberId").mask("(999)99999-9999");
    Filter();
};

function Filter() {
    $.get("Client/Filter", function (response) {
        $("#wrapper").html(response);
    });
}

function Edit(id) {
    var url = id == undefined ? "Client/Edit" : `Client/Edit/${id}`
    $.get(url, function (response) {
        $("#wrapper-edit").html(response);
        $("#clientInfoId").mask("999.999.99-99");
        $("#phoneNumberId").mask("(999)999999999");
        $("#editModal").show();
    });
}

function Delete(id) {
    $.get(`Client/${id}/Delete`)
        .done(function (response) {
            Filter();
            AlertDeleteSuccess();
        }).fail(function (response) {
            AlertDeleteError(response);
        });
}

function AlertDeteleSuccess() {
    $("#wrapper-alert").html(`
         <div class="alert alert-success" role="alert">
            Deletado com sucesso!
        </div>
    `)
    setTimeout(() => {
        $("#wrapper-alert").html("");
    }, 3000);
}

function AlertDeleteError(error) {
    $("#wrapper-alert").html(`
         <div class="alert alert-danger" role="alert">
            Erro ao deletar - ${error}
        </div>
    `)
    setTimeout(() => {
        $("#wrapper-alert").html("");
    }, 3000);
}