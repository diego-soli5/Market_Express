function popUp(success, message) {
    let mode = success ? 'success' : 'error';
    let title = success ? 'Exito!' : 'Algo salió mal..';

    Swal.fire(
        title,
        message,
        mode
    )
}

function popUp(success, message, title) {
    let mode = success ? 'success' : 'error';

    Swal.fire(
        title,
        message,
        mode
    )
}

function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "10",
        "timeOut": "3000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}