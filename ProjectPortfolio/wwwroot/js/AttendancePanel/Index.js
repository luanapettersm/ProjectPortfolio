window.onload = function () {
    LoadColumns();
};

function LoadColumns() {
    CardListOpen();
    CardListPendent();
    CardListInProgress();
    CardListInClose();
}

function CardListOpen() {
    $.get("AttendancePanel/ListCardOpen", function (response) {
        $("#wrapper-open").append(response);
    });

}

function CardListPendent() {
    $.get("AttendancePanel/ListCardPending", function (response) {
        $("#wrapper-pendent").append(response);
    });

}

function CardListInProgress() {
    $.get("AttendancePanel/ListCardInProgress", function (response) {
        $("#wrapper-inProgress").append(response);
    });

}

function CardListInClose() {
    $.get("AttendancePanel/ListCardClose", function (response) {
        $("#wrapper-close").append(response);
    });

}

const columns = document.querySelectorAll(".column");

document.addEventListener("dragstart", (e) => {
    e.target.classList.add("dragging");
});

document.addEventListener("dragend", (e) => {
    e.target.classList.remove("dragging");
});

columns.forEach((item) => {
    item.addEventListener("dragover", (e) => {
        e.preventDefault();
        const dragging = document.querySelector(".dragging");
        const applyAfter = getNewPosition(item, e.clientY);
        if (applyAfter) {
            applyAfter.insertAdjacentElement("afterend", dragging);
        } else {
            item.prepend(dragging);
        }
    });

    item.addEventListener("drop", (e) => {
        const dragging = document.querySelector(".dragging");
        executeOnDrop(dragging, item);
    });
});

function getNewPosition(column, posY) {
    const cards = column.querySelectorAll(".item:not(.dragging)");
    let result;

    for (let refer_card of cards) {
        const box = refer_card.getBoundingClientRect();
        const boxCenterY = box.y + box.height / 2;

        if (posY >= boxCenterY) result = refer_card;
    }
    return result;
}

function executeOnDrop(card, column) {
    let columsStatus = column.querySelector(".cardStatus").value;
    let cardId = card.querySelector(".d-flex .cardId").value;
    let cardStatus = card.querySelector(".d-flex .statusCardId").value;

    if (columsStatus != cardStatus) {
        card.querySelector(".d-flex .statusCardId").value = columsStatus;
        $.get(`AttendancePanel/ChangeStatusCard/${cardId}/${columsStatus}`, function (response) {});
    }

}

function Edit(id) {
    let url = id == undefined ? "AttendancePanel/Edit" : `AttendancePanel/Edit/${id}`
    $.get(url, function (response) {
        $("#wrapper-edit").html(response);
        $("#editModal").show();
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

function Close() {
    $("#wrapper-edit").html("");
}

function Save() {
    let fd = $("#ticketFormId").serializeArray();

    $.post(`/AttendancePanel/Save`, fd)
        .done(function (response) {
            $('#wrapper-edit').html("");
            AlertSaveSuccess();
            LoadColumns();
            
        }).fail(function (response) {
            AlertSaveError(response);
        });

}

function Note(ticketId) {
    $.get(`AttendancePanel/Note/${ticketId}`, function (response) {
        $("#wrapper-note").html(response);
        NoteList();
        $("#noteModal").show();
    });
}

function GetProjects() {
    let clientId = $("#clientId").val();

    $.get(`AttendancePanel/GetProjectsByClient/${clientId}`, function (response) {
        $("#clientProjectId").empty();
        $("#clientProjectId").append('<option selected>Projeto</option>');

        if (response && response.length > 0) {
            response.forEach(function (project) {
                $("#clientProjectId").append(
                    `<option value="${project.id}">${project.title}</option>`
                );
            });
        }

        if (isEdit && projectId !== null) {
            isEdit = false;
            setTimeout(function () {
                $("#clientProjectId").val(projectId).change();
            }, 100); 
        }
    });
}



