var newImagePreview = document.querySelector("#newImagePreview");
var newImageInput = document.querySelector("#Image");


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


