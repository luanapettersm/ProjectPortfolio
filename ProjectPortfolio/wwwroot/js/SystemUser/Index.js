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