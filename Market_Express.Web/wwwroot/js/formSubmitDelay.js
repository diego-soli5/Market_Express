var frm = document.querySelector("#frm");
var btnSubmit = document.querySelector("#btnSubmit");

frm.addEventListener("submit", async function (e) {

    btnSubmit.disabled = true;

    toastr.info("El botón se habilitará en 3 segundos.");

    await delayAsync(3000);

    btnSubmit.disabled = false;

});