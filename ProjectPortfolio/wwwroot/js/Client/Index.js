window.onload = function () {
    $("#clientInfoId").mask("999.999.99-99");
    $("#phoneNumberId").mask("(999)99999-9999");

    new DataTable('#table', {
        ajax: 'Client/Filter',
        columns: [
            { data: `${cnpj == null ? cpf : cnpj}` },
            { data: 'name' },
            { data: 'phoneNumber' },
            { data: 'mail' },
            { data: `
                <button title="Editar" onclick="edit('${id}')" class="iconButton"><i class="glyphicon glyphicon-edit"></i></button>
                <button title="Deletar" onclick="delete('${id})" class="iconButton"><i class="glyphicon glyphicon-trash"></i></button>
            ` }
        ]
    });
};


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
            $('#table').DataTable().ajax.reload();
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