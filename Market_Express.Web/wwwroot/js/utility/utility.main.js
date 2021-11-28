const alertResult = (message) => {
    return `<div class="alert alert-dismissible fade show alert-danger" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <p class="mb-0">${message}</p>
            </div>`;
}

document.querySelector("#frmModalLogin")?.addEventListener("submit", async function (e) {
    e.preventDefault();

    const url = `/Account/Login`;
    const body = new FormData(this);

    try {
        showLoading();

        const fetchResponse = await fetch(url, { body: body, method: 'POST' });
        const json = await fetchResponse.json();

        if (json.success) {
            if (json.resultCode == 0) {
                window.location.href = "/";
            } else {
                await setAuthenticatedUserInterface();

                $("#modalLogin").modal('hide');
            }
        } else {
            popUp(false, json.message, "");
        }

        hideLoading();

    } catch (e) {
        hideLoading();
        console.error(e);
        popUp(false, "Ocurrio un error inesperado..");
    }
});

async function setAuthenticatedUserInterface() {
    try {
        let urlForNavButtons = `/Account/GetUserNavigationButtons`;
        let urlForAccButtons = `/Account/GetUserAccountButtons`;

        const fetchResponseNav = await fetch(urlForNavButtons, { method: 'GET' });
        const fetchResponseAcc = await fetch(urlForAccButtons, { method: 'GET' });

        const textNav = await fetchResponseNav.text();
        const textAcc = await fetchResponseAcc.text();

        updateCartCount();

        document.querySelector("#divNavigationButtons").innerHTML = textNav;
        document.querySelector("#divAccountButtons").innerHTML = textAcc;
    } catch (e) {
        console.error(e);
        popUp(false, "Ocurrio un error inesperado al cargar los botones de navegación..");
    }
}