window.onload = function () {
    $("#clientInfoId").mask("999.999.99-99");
    $("#phoneNumberId").mask("(999)99999-9999");

    var gridColumns =
        [
            { name: 'cpf/cnpj', class: 'text-left', orderable: false, render: item => item.cnpj == null ? item.cpf : item.cnpj },
            { name: 'name', class: 'text-left', orderable: true, render: item => item.name },
            { name: 'phoneNumber', class: 'text-left', orderable: false, render: item => item.phoneNumber },
            { name: 'mail', class: 'text-left', orderable: false, render: item => item.mail },
            {
                name: 'action', class: 'text-left', orderable: false, render: item =>
                    ` 
                        <button title="Editar" onclick="edit('${item.id}')" class="iconButton"><i class="glyphicon glyphicon-edit"></i></button>
                        <button title="Deletar" onclick="delete('${item.id})" class="iconButton"><i class="glyphicon glyphicon-trash"></i></button>
                    `
            }
        ];

    new DataTable('#table', {
        ajax: 'Client/Filter',
        language: {
            sLengthMenu: "_MENU_",
            search: "",
            searchPlaceholder: "Pesquisar",
        },
        showNEntries: false,
        columns: gridColumns
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