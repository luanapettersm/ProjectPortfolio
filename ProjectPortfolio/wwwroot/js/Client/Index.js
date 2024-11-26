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
        $("#phoneNumberId").mask("(999)99999-9999");
        $("#editModal").show();
    });
}

function Delete(id) {
    $.get(`Client/${id}/Delete`)
        .done(function (response) {
            Filter();
        });
}
