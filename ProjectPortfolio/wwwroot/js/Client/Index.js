window.onload = function () {
    $("#clientInfoId").mask("999.999.999-99");
    $("#phoneNumberId").mask("(999)99999-9999");

    var gridColumns = [
        {
            name: 'cpf/cnpj',
            class: 'text-left',
            width: '200px', 
            orderable: false,
            render: function (data, type, row) {
                return row.cnpjformatado === "" ? row.cpfformatado : row.cnpjformatado;
            }
        },
        {
            name: 'name',
            class: 'text-left',
            orderable: true,
            render: function (data, type, row) {
                return row.name;
            }
        },
        {
            name: 'phoneNumber',
            class: 'text-left',
            width: '200px', 
            orderable: false,
            render: function (data, type, row) {
                return row.phoneNumber;
            }
        },
        {
            name: 'mail',
            class: 'text-left',
            orderable: false,
            render: function (data, type, row) {
                return row.mail;
            }
        },
        {
            name: 'action',
            class: 'text-left',
            width: '75px', 
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
            url: '/Client/Filter',
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
            sInfo: "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            sInfoEmpty: "Mostrando 0 até 0 de 0 registros"
        }
    });
};


function Edit(id) {
    var url = id == undefined ? "Client/Edit" : `Client/Edit/${id}`
    $.get(url, function (response) {
        $("#wrapper-edit").html(response);
        $("#clientInfoId").mask("999.999.999-99");
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