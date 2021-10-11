var newImagePreview = document.querySelector("#newImagePreview");
var newImageInput = document.querySelector("#Image");
var frm = document.querySelector("#frm");
var btnSubmit = document.querySelector("#btnSubmit");


newImageInput.addEventListener("change", previewNewImage);

function previewNewImage() {
    const filesOfImageInput = newImageInput.files;

    if (!filesOfImageInput || !filesOfImageInput.length) {
        newImagePreview.src = "";
        return;
    }

    const firstFileOfImageInput = filesOfImageInput[0];
    const objectURL = URL.createObjectURL(firstFileOfImageInput);

    newImagePreview.src = objectURL;
}


frm.addEventListener("submit",async function (e) {

    btnSubmit.disabled = true;

    toastr.info("El botón se habilitará en 3 segundos.");

    await new Promise(r => setTimeout(r, 3000));

    btnSubmit.disabled = false;

});