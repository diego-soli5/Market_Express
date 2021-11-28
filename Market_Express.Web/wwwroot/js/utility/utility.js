async function delayAsync(time) {
    await new Promise(r => setTimeout(r, time));
}

function showLoading() {
    if (document.querySelector(".lds-dual-ring") == undefined) {
        let divLoading = document.createElement("div");

        divLoading.className = "lds-dual-ring";

        document.querySelector("body").appendChild(divLoading);
    }
}

function hideLoading() {
    $(".lds-dual-ring").remove();
}

function createQueryStringParams(formData) {
    return [...formData.entries()]
        .map(x => `${encodeURIComponent(x[0])}=${encodeURIComponent(x[1])}`)
        .join('&');
}

function updateCartCount() {
    fetch('/Client/Cart/GetCartArticlesCount', { method: 'GET' })
        .then(response => response.text())
        .then(text => $("#cartArticlesCount").html(text));
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