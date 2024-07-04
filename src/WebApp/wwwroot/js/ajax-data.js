/* Process ajax result in Swal 2 */
function showResultInSwal2(result, action) {
    if (isNotNullOrUndefined(result) == false || result == "") {
        reload();
    } else if (result.status == 0) {
        Swal.fire({
            title: "",
            text: result.message,
            icon: "success",
            buttonsStyling: false,
            customClass: {
                confirmButton: "btn btn-success",
            },
            allowOutsideClick: false
        }).then((res) => {
            if (res.isConfirmed) {
                if (isNotNullOrUndefined(action)) {
                    action();
                }
            }
        })
    } else if (result.status == 1) {
        showErrorSwal2("", result.message);
    } else if (result.status == 2) {
        showWarningSwal2("", result.message);
    }
}

/* Ajax Response Error Handler */
function onAjaxError(jqXHR, exception) {
    var title = "[" + jqXHR.status + "]";
    var msg = "";

    if (jqXHR.status === 0) {
        msg = "Not connect.\n Verify Network.";
    } else if (jqXHR.status === 400) {
        msg = "Bad Request.";
    } else if (jqXHR.status === 401) {
        msg = "Unauthorized.";
        showErrorSwal2(msg);
        reload();
    } else if (jqXHR.status === 403) {
        msg = "Forbidden.";
    } else if (jqXHR.status === 404) {
        msg = "Requested page not found.";
    } else if (jqXHR.status === 409) {
        msg = "Conflict exception.";
    } else if (jqXHR.status === 500) {
        msg = "Internal Server Error.";
    } else if (exception === "parsererror") {
        msg = "Requested JSON parse failed.";
    } else if (exception === "timeout") {
        msg = "Time out error.";
    } else if (exception === "abort") {
        msg = "Ajax request aborted.";
    } else {
        msg = "Uncaught Error.";
    }

    //var json = JSON.stringify(jqXHR.responseText);
    //alert(json);

    if (isNotNullOrUndefined(jqXHR.responseText)) {
        try {
            var obj = JSON.parse(jqXHR.responseText);
            msg = obj.message;
        }
        catch (err) {
            console.log(err.message);
        }
    }

    showErrorSwal2("", msg); //in Common.js
}

/* Ajax Success Response Handler */
function onAjaxSuccess(result) {
    showResultInSwal2(result, reload);
}

/* Modal ajax data extensions */
$.extend({

    // Render modal extension
    render_modal: function (modalId, url, data) {

        function processResponse(res) {
            $("" + modalId + " .modal-content").html(res);
            $.validator.unobtrusive.parse(modalId);
            $(modalId).modal({ backdrop: "static", keyboard: false })
            $(modalId).modal("show");
        };

        try {
            if (data === undefined || data === null) {
                $.ajax({
                    type: "GET",
                    url: url,
                    success: function (res) {
                        processResponse(res);
                    },
                    error: function (jqXHR, exception) {
                        onAjaxError(jqXHR, exception);
                    }
                });
            } else {
                $.ajax({
                    url: url,
                    type: "POST",
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    data: data,
                    success: function (res) {
                        processResponse(res);
                    },
                    error: function (jqXHR, exception) {
                        onAjaxError(jqXHR, exception);
                    }
                });
            }
        }
        catch (err) {
            showErrorSwal2("", err.message); //in Common.js
        }
    },

    get: function (url, action) {
        $.ajax({
            url: url,
            type: 'GET',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (res) {
                if (action == undefined || action == null) {
                    onAjaxSuccess(res);
                } else {
                    action(res);
                }
            },
            error: function (jqXHR, exception) {
                onAjaxError(jqXHR, exception);
            }
        });
    },

    post(url, data, action) {
        // add AntiforgeryToken
        let token = $('input[name="__RequestVerificationToken"]').val();
        if (token != null) {
            data["__RequestVerificationToken"] = token;
        }

        try {
            $.ajax({
                url: url,
                type: "POST",
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: data,
                success: function (res) {
                    if (action == undefined || action == null) {
                        onAjaxSuccess(res);
                    } else if (action == "toastr") {
                        showToastr(res);
                    } else if (action["submitForm"] != null) {
                        var formId = action["submitForm"];
                        document.getElementById(formId).submit();
                    } else if (action["renderTo"] != null) {
                        var areaId = action["renderTo"];
                        $(areaId).html(res);
                    } else {
                        action(res);
                    }
                },
                error: function (jqXHR, exception) {
                    onAjaxError(jqXHR, exception);
                }
            });
        }
        catch (err) {
            showErrorSwal2("", err.message); //in Common.js
        }
    }
});