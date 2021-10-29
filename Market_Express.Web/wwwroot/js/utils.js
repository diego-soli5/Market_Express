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