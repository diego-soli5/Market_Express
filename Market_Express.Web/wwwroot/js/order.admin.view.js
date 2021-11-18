var btn = document.querySelector("#btnSetFinished");

btn?.addEventListener("click",async function (e) {
    let orderId = this.getAttribute("data-order-id");

    const url = `/Admin/Order/SetFinished?orderId=${orderId}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'POST' });
        const json = await fetchResponse.json();

        if (json.success) {
            toastr.success(json.message);
            btn.disabled = true;
            document.querySelector("#ddStatus").innerHTML = '<p class="text-success">Terminado</p>';
        } else {
            popUp(false, json.message);
        }

        hideLoading();
    } catch (e) {
        hideLoading();

        console.error(e);
        popUp(false, "Ocurrio un error inesperado..");
    }
});