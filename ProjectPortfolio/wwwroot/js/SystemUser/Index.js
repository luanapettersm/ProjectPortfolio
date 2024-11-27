window.onload = function () {
    Filter();
};

function Filter() {
    $.get("SystemUser/Filter", function (response) {
        $("#wrapper").html(response);
    });
}

function Edit(id) {
    var url = id == undefined ? "SystemUser/Edit" : `SystemUser/Edit/${id}`
    $.get(url, function (response) {
        $("#wrapper-edit").html(response);
        $("#editModal").show();
    });
}

function Delete(id) {
    $.get(`SystemUser/${id}/Delete`)
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