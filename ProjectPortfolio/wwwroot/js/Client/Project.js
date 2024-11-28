function CloseProject() {
    $("#wrapper-project").html("");
}

function CloseProjectEdit() {
    $("#wrapper-projectEdit").html("");
}


function ProjectList() {
    $.get("Client/ProjectList", function (response) {
        $("#wrapper-projectList").html(response);
    });
}


function ProjectEdit(id) {
    var url = id == undefined ? "Client/ProjectEdit" : `Client/ProjectEdit/${id}`
    $.get(url, function (response) {
        $("#wrapper-projectEdit").html(response);
        $("#projectEditModal").show();
    });
}



function SaveProjectEdit() {
    var fd = $("#projectFormId").serializeArray();

    fd.push({ name: 'project.ClientId', value: $("#clientId").val() });

    $.post(`/Client/ProjectSave`, fd)
        .done(function (response) {
            $('#wrapper-projectEdit').html("");
            AlertSaveSuccess();
            ProjectList();
        }).fail(function (response) {
            AlertSaveError(response);
        });

}

function DeleteProject(id) {
    $.get(`Client/${id}/ProjectDelete`)
        .done(function (response) {
            ProjectList();
            AlertDeleteSuccess();
        }).fail(function (response) {
            AlertDeleteError(response);
        });
}