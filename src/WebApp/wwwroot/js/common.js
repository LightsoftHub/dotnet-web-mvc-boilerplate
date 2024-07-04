/* Validate data */
function isNotNullOrUndefined(object) {
    return object != undefined && isNotNullOrUndefined != null;
}

/* Show error result by SweetAlert */
function showErrorSwal(title, message) {
    swal({
        type: "error",
        title: "" + title,
        text: "" + message,
    });
}

function showWarningSwal(title, message) {
    swal({
        type: "warning",
        title: "" + title,
        text: "" + message,
    });
}

/* Show error result by SweetAlert2 */
function showErrorSwal2(title, message) {
    Swal.fire({
        icon: "error",
        title: "" + title,
        html: "" + message,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-danger",
        },
        allowOutsideClick: false
    });
}

function showWarningSwal2(message) {
    Swal.fire({
        icon: "warning",
        title: "Warning",
        html: "" + message,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-warning",
        },
        allowOutsideClick: false
    });
}

/* Show result in Toastr */
function showToastr(result, action) {
    if (result.status == 0) {
        toastr.success(result.message);
    } else if (result.status == 1) {
        toastr.error(result.message);
    } else if (result.status == 2) {
        toastr.warning(result.message);
    };

    if (isNotNullOrUndefined(action)) {
        action();
    }
}

// reload current page with no post data
function reload() {
    setTimeout(function () {
        location.reload();
    }, 100);
}

// redirect to url
function redirect(url) {
    setTimeout(function () {
        window.location = url;
    }, 100);
}

// redirect to url if confirm
function redirectIfConfirm(message, url) {
    if (confirm(message)) {
        window.location.href = url;
    }
    else
        return false;
}

// redirect to url if confirm in Sweet Alert
function redirectInSwal(message, url) {
    Swal.fire({
        title: "",
        text: message + "",
        icon: "warning",
        showCancelButton: true,
        buttonsStyling: true,
        customClass: {
            confirmButton: "btn btn-warning",
            cancelButton: "btn btn-secondary",
        },
    }).then((res) => {
        if (res.isConfirmed) {
            redirect(url);
        }
    })
}

// Serialize the form data into a JSON object
function formToJson(formId) {
    const jsonObject = {};

    var formElement = document.getElementById(formId);

    // check element exists
    if (typeof(formElement) != 'undefined' && formElement != null) {
        const formData = new FormData(formElement);

        formData.forEach((value, key) => {
            jsonObject[key] = value;
        });
    }

    return jsonObject;
}

// get value by ID
function getValue(id) {
    return document.getElementById(id).value;
}