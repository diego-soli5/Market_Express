var frm = document.querySelector("#frm");

frm.addEventListener("submit", async function (e) {

    btnSubmit.disabled = true;

    toastr.info("El botón se habilitará en 3 segundos.");

    await delayAsync(3000);

    btnSubmit.disabled = false;

});