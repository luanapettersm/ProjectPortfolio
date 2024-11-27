window.onload = function () {
    Card();
    Card();
    Card();
    Card();
    Card();
    Card();
    Card();
    Card();
};


function Card() {
    $.get("AttendancePanel/Card", function (response) {
        $("#wrapper-open").append(response);
    });

}