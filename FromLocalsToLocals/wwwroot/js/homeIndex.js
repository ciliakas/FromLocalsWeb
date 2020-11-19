$(document).ready(function () {
    // Handler for .ready() called.
    $("#discover").click(function () {
        $('html, body').animate({
            scrollTop: $('#mapTitle').offset().top
        }, 'fast');
    });
});