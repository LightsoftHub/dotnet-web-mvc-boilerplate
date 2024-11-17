function OnLoginSubmit() {
    alert("Hello!");
    $("#loginForm").on("submit", function () {
        var isValid = $(this).valid();
        //check validate if all field are validate true
        if (isValid == true) {
            $(".btn-submit").css('display', 'none');
            $(".sk-spinner").css('display', '');
        }
    });
}