window.onload = function () {
    CardListOpen();
    CardListPendent();
    CardListInProgress();
    CardListInClose();
};


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
        const dragging = document.querySelector(".dragging");
        const applyAfter = getNewPosition(item, e.clientY);

        if (applyAfter) {
            applyAfter.insertAdjacentElement("afterend", dragging);
        } else {
            item.prepend(dragging);
        }
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