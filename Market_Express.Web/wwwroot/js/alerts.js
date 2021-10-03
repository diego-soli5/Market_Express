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