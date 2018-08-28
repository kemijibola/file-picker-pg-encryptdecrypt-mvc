
function successMessage(message) {
    swal({
        title: "Success!",
        text: message,
        type: "success",
        confirmButtonClass: 'btn-default btn-md waves-effect waves-light',
        confirmButtonText: 'Ok'
    });
}

function informationMessage(message) {
    swal({
        title: "Information!",
        text: message,
        type: "info",
        confirmButtonClass: 'btn-default btn-md waves-effect waves-light',
        confirmButtonText: 'Ok'
    });
}

function errorMessage(message) {
    swal({
        title: "Operation failed!",
        text: message,
        type: "error",
        showCancelButton: false,
        confirmButtonClass: 'btn btn-white waves-effect',
        confirmButtonText: 'Ok'
    });
}

function confirmDelete(message, btnDelete, btnComfirmText) {
    swal({
        title: "Are you sure?",
        text: message,
        type: "error",
        showCancelButton: true,
        cancelButtonClass: 'btn-white btn-md waves-effect',
        confirmButtonClass: 'btn-danger btn-md waves-effect waves-light',
        confirmButtonText: btnComfirmText,
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            $('#' + btnDelete).click();
            //swal("Deleted!", "Record was successfully deleted.", "success");
        }
    });
}

function confirmGenerate(message, btnGenerate) {
    swal({
        title: "Are you sure?",
        text: message,
        type: "info",
        showCancelButton: true,
        cancelButtonClass: 'btn-white btn-md waves-effect',
        confirmButtonClass: 'btn-warning btn-md waves-effect waves-light',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            $('#' + btnGenerate).click();
            //swal("Deleted!", "Record was updated deleted.", "success");
        }
    });
}

function confirmUpload(message, btnUpload) {
    //$('#' + btnUpload).click().hide();
    $('.modal fade').modal('hide');
    swal({
        title: "Are you sure?",
        text: message,
        type: "info",
        showCancelButton: true,
        cancelButtonClass: 'btn-white btn-md waves-effect',
        confirmButtonClass: 'btn-warning btn-md waves-effect waves-light',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            $("#btnExcelUpload").click();
            //$('#' + btnUpload).click().show();
            //swal("Deleted!", "Record was updated deleted.", "success");
        }
    });

}

$(document).ready(function () {

    //Allow numbers
    // Add notext as a class to any textbox to use this feature.
    $(".no-text").keypress(function (evt) {
        var keycode = evt.charCode || evt.keyCode;
        if (keycode == 8 || keycode == 9 || keycode == 37 || keycode == 39 || keycode == 46 || (keycode >= 48 && keycode <= 57)) {
            //allow backspace, Tab, <--, -->, delete, . and 0 - 9 through
        }
        else {
            return false;
        }
    });
});