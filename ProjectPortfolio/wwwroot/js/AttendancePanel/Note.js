function CloseNote() {
    $("#wrapper-note").html("");
}

function CloseNoteEdit() {
    $("#wrapper-noteEdit").html("");
}

function NoteList() {
    var ticketId = $("#ticketId").val();
    $.get(`AttendancePanel/NoteList/${ticketId}`, function (response) {
        $("#wrapper-noteList").html(response);
    });
}

function NoteEdit(id) {
    var url = id == undefined ? "AttendancePanel/NoteEdit" : `AttendancePanel/NoteEdit/${id}`
    $.get(url, function (response) {
        $("#wrapper-noteEdit").html(response);
        $("#noteEditModal").show();
    });
}


function SaveNoteEdit() {
    var fd = $("#noteFormId").serializeArray();

    fd.push({ name: 'note.IssueId', value: $("#ticketId").val() });

    $.post(`/AttendancePanel/NoteSave`, fd)
        .done(function (response) {
            $('#wrapper-noteEdit').html("");
            AlertSaveSuccess();
            NoteList();
        }).fail(function (response) {
            AlertSaveError(response);
        });

}

function NoteDelete(id) {
    $.get(`AttendancePanel/${id}/NoteDelete`)
        .done(function (response) {
            NoteList();
            AlertDeleteSuccess();
        }).fail(function (response) {
            AlertDeleteError(response);
        });
}


