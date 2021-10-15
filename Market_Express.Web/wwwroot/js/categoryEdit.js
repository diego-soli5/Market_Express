var newImagePreview = document.querySelector("#ImagePreview");
var newImageInput = document.querySelector("#NewImage");

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

    document.querySelector("figcaption").innerHTML = "Imagen Cargada";
}