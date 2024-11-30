window.onload = function () {
    let gridColumns = [
        {
            name: 'name',
            class: 'text-left',
            orderable: true,
            render: function (data, type, row) {
                return row.name;
            }
        },
        {
            name: 'surname',
            class: 'text-left',
            orderable: false,
            render: function (data, type, row) {
                return row.surname;
            }
        },
        {
            name: 'displayName',
            class: 'text-left',
            orderable: false,
            render: function (data, type, row) {
                return row.displayName;
            }
        },
        {
            name: 'businessRole',
            class: 'text-left',
            orderable: false,
            render: function (data, type, row) {
                return row.businessRole == 1 ? "Estagiario" :
                    row.businessRole == 2 ? "Analista" : "Gestor";
            }
        },
        {
            name: 'action',
            class: 'text-left',
            orderable: false,
            render: function (data, type, row) {
                return `
                    <button title="Editar" onclick="Edit('${row.id}')" class="iconButton">
                        <i class="glyphicon glyphicon-edit"></i>
                    </button>
                    <button title="Deletar" onclick="Delete('${row.id}')" class="iconButton">
                        <i class="glyphicon glyphicon-trash"></i>
                    </button>`;
            }
        }
    ];

    $('#table').DataTable({
        ajax: {
            url: '/SystemUser/Filter',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: gridColumns,
        serverSide: false,
        processing: true,
        language: {
            sLengthMenu: "_MENU_",
            search: "",
            searchPlaceholder: "Pesquisar",
            sLoadingRecords: "Carregando...",
            sEmptyTable: "Nenhum registro encontrado",
            sProcessing: "",
            sInfo: "Mostrando de _START_ ate _END_ de _TOTAL_ registros",
            sInfoEmpty: "Mostrando 0 ate 0 de 0 registros"
        }
    });
};

function Edit(id) {
    let url = id == undefined ? "SystemUser/Edit" : `SystemUser/Edit/${id}`
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