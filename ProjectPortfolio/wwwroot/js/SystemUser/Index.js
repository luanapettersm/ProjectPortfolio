window.onload = function () {
    var gridColumns =
        [
            { name: 'name', class: 'text-left', orderable: true, render: Render(item => item.name) },
            { name: 'surname', class: 'text-left', orderable: false, render: Render(item => item.surname) },
            { name: 'displayName', class: 'text-left', orderable: false, render: Render(item => item.displayName) },
            { name: 'businessRole', class: 'text-left', orderable: false, render: Render(item => item.businessRole == 1 ? "Estagiário" : businessRole == 2 ? "Analista" : "Gestor") },
            {
                name: 'action', class: 'text-left', orderable: false, render: Render(item =>
                    ` 
                        <button title="Editar" onclick="edit('${item.id}')" class="iconButton"><i class="glyphicon glyphicon-edit"></i></button>
                        <button title="Deletar" onclick="delete('${item.id})" class="iconButton"><i class="glyphicon glyphicon-trash"></i></button>
                    `
                )
            }
        ];

    new DataTable('#table', {
        ajax: 'SystemUser/Filter',
        columns: gridColumns
    });
};

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