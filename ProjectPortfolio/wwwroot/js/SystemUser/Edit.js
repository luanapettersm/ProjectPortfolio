function Close() {
    $("#wrapper-edit").html("");
}

function Save() {
    var fd = $("#systemUserFormId").serializeArray();

    $.post(`/SystemUser/Save`, fd)
        .done(function (response) {
            $('#wrapper-edit').html("");
            Filter();
        })
}
