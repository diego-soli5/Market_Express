var btnGenerate = document.querySelector("#btnGenerateCart");

btnGenerate.addEventListener("click", async function (e) {

    let orderId = this.getAttribute("data-order-id");

    const url = `/Client/Cart/GenerateCart?orderId=${orderId}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'POST' });
        const json = await fetchResponse.json();

        if (json.success) {
            window.location.href = "/Client/Cart";
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