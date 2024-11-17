// old $('.loading')

$(document).ready(function () {
    $(document).bind('ajaxStart', function () {
        $(".loading").show();
    }).bind('ajaxStop', function () {
        $(".loading").hide();
    });
});

$(window).on('load', function () {
    // Animate loader off screen
    $(".loading").fadeOut('fast');
});