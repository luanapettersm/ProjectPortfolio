function CloseProject() {
    $("#wrapper-project").html("");
}

function CloseProjectEdit() {
    $("#wrapper-projectEdit").html("");
}


function ProjectList() {
    let clientId = $("#clientId").val();
    $.get(`Client/ProjectList/${clientId}`, function (response) {
        $("#wrapper-projectList").html(response);
    });
}


function ProjectEdit(id) {
    let url = id == undefined ? "Client/ProjectEdit" : `Client/ProjectEdit/${id}`
    $.get(url, function (response) {
        $("#wrapper-projectEdit").html(response);
        $("#projectEditModal").show();
    });
}



function SaveProjectEdit() {
    let fd = $("#projectFormId").serializeArray();

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