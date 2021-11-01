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


function valideKey(evt) {

    // code is the decimal ASCII representation of the pressed key.
    var code = (evt.which) ? evt.which : evt.keyCode;

    if (code == 8) { // backspace.
        return true;
    } else if (code >= 48 && code <= 57) { // is a number.
        return true;
    } else { // other keys.
        return false;
    }
}